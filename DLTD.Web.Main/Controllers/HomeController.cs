using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using DLTD.Web.Main.Common;
using DLTD.Web.Main.DAL;
using DLTD.Web.Main.Models.Enum;
using DLTD.Web.Main.Models.MetaData;
using DLTD.Web.Main.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace DLTD.Web.Main.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
      
        public ActionResult Index(TrangThaiVanBan? trangThai)
        {
            ViewBag.TrangThai = trangThai;
            //ViewBag.Nhom = GroupUserLogin;
            
            if((GroupUserLogin==NhomNguoiDung.ChuyenVien)||(GroupUserLogin==NhomNguoiDung.QuanTriHeThong))
            {
                ViewBag.TinhHinhThucHien = "";
                ViewBag.HoanThanh = "";
                ViewBag.ThemNhiemvu = "";
            }
            else
            {
                ViewBag.TinhHinhThucHien = "none";
                ViewBag.HoanThanh = "none";
                ViewBag.ThemNhiemvu = "none";
            }


            if (trangThai==TrangThaiVanBan.Moi)
            {
                ViewBag.TieuDe = "Nhiệm vụ mới";
            }
           else if (trangThai==TrangThaiVanBan.DangXuLy)
            { ViewBag.TieuDe = "Nhiệm vụ đang thực hiện"; }
            else if (trangThai==TrangThaiVanBan.HoanThanh)
            { ViewBag.TieuDe = "Nhiệm vụ đã hoàn thành"; }
            else if (trangThai==TrangThaiVanBan.QuaHan)
            { ViewBag.TieuDe = "Nhiệm vụ đã quá hạn"; }
            else if (trangThai == TrangThaiVanBan.Undefined)
            {
                ViewBag.TieuDe = "Tất cả nhiệm vụ"; }
         
            else
            {
                return Index(TrangThaiVanBan.Undefined);

            }
            return View();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DanhSachNhiemVu(string search, TrangThaiVanBan? trangThai)
        {
            ViewBag.SearchText = search;
            ViewBag.TrangThai = trangThai;
            return PartialView("_DanhSachNhiemVu");
        }

        public async Task<ActionResult> GetVanBanChiDao([DataSourceRequest]DataSourceRequest request, string search, TrangThaiVanBan? trangThai)
        {
            var data = await VanBanChiDaoManagement.Go.GetVanBanChiDao(UserIdLogin, GroupUserLogin, trangThai);

            var dataViewModel = data.Select(x => x.Transform());

            if(string.IsNullOrWhiteSpace(search)) return Json(dataViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            search = search.ToLower();

            var dateSearch = search.ToDateTimeExt();

            var viewModel =
                dataViewModel.Where(
                    x =>                      
                        x.ThoiHanXuLy == dateSearch ||
                        x.SoKH.ToLower().Contains(search) ||
                        x.Trichyeu.ToLower().Contains(search) ||
                        x.TenDonVi.ToLower().Contains(search) ||
                        x.NguoiGui.ToLower().Contains(search) ||
                        x.YKienChiDao.ToLower().Contains(search)
                    );

            return Json(viewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdateCompleteVanBan(int? id)
        {
            await VanBanChiDaoManagement.Go.UpdateCompleteVanBan(id);

            return Json(new {Code = 1, Message = "Updated"}, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetTinhHinhThucHien(int? idVanBanChiDao)
        {
            var dataThth = await TinhHinhThucHienManagement.Go.GetTinhHinhThucHien(idVanBanChiDao);
            var dataThph = await TinhHinhPhoiHopManagement.Go.GetTinhHinhPhoiHop(idVanBanChiDao);


            if (dataThph == null && dataThth == null)
                return Json(new { Code = 2, Results = "Không có dữ liệu." }, JsonRequestBehavior.AllowGet);

            if (dataThph == null)
                return Json(new { Code = 1, Results = dataThth.Select(x=>x.Transform()) }, JsonRequestBehavior.AllowGet);

            if (dataThth == null)
                return Json(new { Code = 1, Results = dataThph.Select(x => x.Transform()) }, JsonRequestBehavior.AllowGet);


            var data =
                dataThth.Select(x => x.Transform())
                    .Union(dataThph.Select(x => x.Transform()))
                    .OrderByDescending(x => x.NgayBaoCao);


            return Json(new {Code = 1, Results = data}, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> SaveTinhHinhThucHien()
        {
            string fileName = null;
            string filePath = null;
            string fileUrl = null;
            try
            {
                if (Request.Files.Count > 0)
                {
                    foreach (string file in Request.Files)
                    {
                        var postedFile = Request.Files[file];
                        if (postedFile == null || postedFile.ContentLength == 0) continue;
                        var folderUpload = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(folderUpload)) Directory.CreateDirectory(folderUpload);
                        fileName = postedFile.FileName;
                        var fileNameBeSave = string.Format("{0:yyyyMMddHHmmss}-{1}", DateTime.Now, fileName);
                        fileUrl = string.Format("/Uploads/{0}", fileNameBeSave);
                        filePath = string.Format("{0}{1}", folderUpload, fileNameBeSave);

                        postedFile.SaveAs(filePath);
                    }
                }
                var idVanBanChiDao = Request.Form["IdVanBan"].ToIntExt();
                var noidungbaocao = Request.Form["TinhHinhXuLy"];
                var donvi = Request.Form["DonVi"].ToIntExt();

                var input = new TinhHinhThucHienInput
                {
                    UserId = UserIdLogin,
                    IdDonVi = donvi,
                    FileDinhKem = fileName,
                    FileUrl = fileUrl,
                    IdVanBanChiDao = idVanBanChiDao,
                    NgayBaoCao = DateTime.Now,
                    NoiDungBaoCao = noidungbaocao
                };

                var results = donvi.HasValue
                    ? await TinhHinhPhoiHopManagement.Go.SaveTinhHinhPhoiHop(input)
                    : await TinhHinhThucHienManagement.Go.SaveTinhHinhThucHien(input);
                
                if (!results)
                {
                    if (!string.IsNullOrWhiteSpace(filePath)) System.IO.File.Delete(filePath);

                    return Json(new { Code = -2, Message = "Có lỗi khi lưu dữ liệu." }, JsonRequestBehavior.AllowGet);
                }

                return await GetTinhHinhThucHien(idVanBanChiDao);
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(filePath)) System.IO.File.Delete(filePath);

                return Json(new {Code = -1, Message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> GetDonViPhoiHop(int? idVanBan)
        {
            var vbcd = await VanBanChiDaoManagement.Go.GetVanBanChiDaoById(idVanBan);

            if (vbcd == null) return Json(new {Error = "Không tìm thấy văn bản chỉ đạo"}, JsonRequestBehavior.AllowGet);

            var data = await TinhHinhPhoiHopManagement.Go.GetDonViPhoiHop(idVanBan);

            return data != null
                ? Json(
                    data.Select(
                        x => new DonViComboBoxViewModal {Id = x.IdDonvi, Name = x.DonVi.Ten + " (Phối hợp xử lý)"})
                        .Prepend(new DonViComboBoxViewModal {Id = null, Name = vbcd.DonVi.Ten + " (Xử lý chính)"}),
                    JsonRequestBehavior.AllowGet)
                : Json(new DonViComboBoxViewModal {Id = null, Name = vbcd.DonVi.Ten + " (Xử lý chính)"},
                    JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DeleteTinhHinhThucHien(int? id, LoaiBaoCao loaiBaoCao, int? idVanBan)
        {
            var results = loaiBaoCao == LoaiBaoCao.PhoiHop
                ? await TinhHinhPhoiHopManagement.Go.DeleteTinhHinhPhoiHop(id)
                : await TinhHinhThucHienManagement.Go.DeleteTinhHinhThucHien(id);

            if (results)
                return await GetTinhHinhThucHien(idVanBan);

            return Json(new { Code = -2, Message = "Có lỗi khi xóa dữ liệu." }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetFileVanBan(int id)
        {
            var model = VanBanChiDaoManagement.Go.GetFileVanBanChiDao(id);
            return PartialView("_Attachments", model);
        }

        public async Task<ActionResult> GetVanBanDetail(int id) 
        {
            var vanban = await VanBanChiDaoManagement.Go.GetVanBanChiDaoById(id);

            VanBanChiDaoViewModel model = vanban.Transform();

            return PartialView("_XemChitietVanban", model);
        }

        public async Task<ActionResult> DeleteVanBan(int id)
        {
            var result = await VanBanChiDaoManagement.Go.DeleteVanBan(id);
            return Json(new {Result = result}, JsonRequestBehavior.AllowGet);
        }

        public class DonViComboBoxViewModal
        {
            public int? Id { get; set; }
            public string Name { get; set; }
        }

        
    }
}