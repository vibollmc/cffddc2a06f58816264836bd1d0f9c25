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
    public class TinhHinhXLController : Controller
    {
        //
        // GET: /TinhHinhXL/
        [HttpGet]
        public ActionResult Index()
        {
            var model = new TonghopTinhhinhXulyViewModel();
            model.Khoi = DonViManagement.Go.GetNguonChiDao();
            return View(model);
        }
         [HttpPost]
        public async Task<ActionResult> ThongKe(string tuNgay, string denNgay, int? idKhoi, int? idDonvi)
        {
            var model = new TonghopTinhhinhXulyViewModel();      

            var data = await VanBanChiDaoManagement.Go.ThongKeVanBanChiDao(tuNgay.ToDateTimeExt(), denNgay.ToDateTimeExt(),idKhoi, idDonvi);

            var sum = data.Count();
            var sumDangXL =
                data.Count(
                    x =>
                        x.TrangThai != TrangThaiVanBan.HoanThanh &&
                        (x.ThoiHanXuLy == null || x.ThoiHanXuLy <= DateTime.Today));
            var sumDangXLQuaHan =
                data.Count(
                    x =>
                        x.TrangThai != TrangThaiVanBan.HoanThanh &&
                        (x.ThoiHanXuLy != null && x.ThoiHanXuLy > DateTime.Today));

            var sumHT =
                data.Count(
                    x =>
                        x.TrangThai == TrangThaiVanBan.HoanThanh &&
                        (x.ThoiHanXuLy == null || x.ThoiHanXuLy >= x.NgayHoanThanh));
            var sumHTQuaHan =
                data.Count(
                    x =>
                        x.TrangThai == TrangThaiVanBan.HoanThanh &&
                        (x.ThoiHanXuLy != null && x.ThoiHanXuLy < x.NgayHoanThanh));
            model.HoanThanh = sumHT;
            //DangThucHien = string.Format("{0:#,###}", sumDangXL),
            model.DangThucHien = sumDangXL;
            model.QuaHan = sumDangXLQuaHan;
            model.HoanThanhQuaHan = sumHTQuaHan;
            model.TongNhiemVu = sum;
            return PartialView("_TonghopTHXL",model);
        }
         public ActionResult ListVanbanChiDao(int intloai, int iddonvi, int idcanbo, string strngaybd, string strngaykt)
         {

             ListTonghopVanBanChiDaoViewModel model = new ListTonghopVanBanChiDaoViewModel();
             ViewBag.intPage = 1;
             string strloaivanban = string.Empty;
             switch (intloai)
             {
                 case (int)enumtinhtrangxuly.intloai.Moi:
                     strloaivanban = "DANH SÁCH VĂN BẢN CHỈ ĐẠO MỚI";
                     break;
                 case (int)enumtinhtrangxuly.intloai.DangXuLy:
                     strloaivanban = "DANH SÁCH VĂN BẢN ĐANG XỬ LÝ";
                     break;
                 case (int)enumtinhtrangxuly.intloai.QuaHan:
                     strloaivanban = "DANH SÁCH VĂN BẢN QUÁ HẠN XỬ LÝ";
                     break;
                 case (int)enumtinhtrangxuly.intloai.HoanThanh:
                     strloaivanban = "DANH SÁCH VĂN BẢN ĐÃ HOÀN THÀNH XỬ LÝ";
                     break;
                 case (int)enumtinhtrangxuly.intloai.HoanThanhTreHan:
                     strloaivanban = "DANH SÁCH VĂN BẢN HOÀN THÀNH TRỄ HẠN";
                     break;          
             }
             model.intloai = intloai;
             model.strloaivanban = strloaivanban;
             model.idcanbo = idcanbo;
             model.iddonvi = iddonvi;
             model.strngaybd = strngaybd;
             model.strngaykt = strngaykt; 
             return View(model);

         }
         public async Task<ActionResult> GetVanBanChiDao([DataSourceRequest]DataSourceRequest request, string search, TrangThaiVanBan? trangThai, string tuNgay, string denNgay, int? idKhoi, int? idDonvi)
         {
             var identity = (ClaimsIdentity)User.Identity;

             var userId = identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

             var data = await VanBanChiDaoManagement.Go.GetListVanBanChiDaoThongKe(userId.ToIntExt(), trangThai, tuNgay.ToDateTimeExt(), denNgay.ToDateTimeExt(), idKhoi, idDonvi);

             var dataViewModel = data.Select(x => x.Transform());

             if (string.IsNullOrWhiteSpace(search)) return Json(dataViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

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
	}

}