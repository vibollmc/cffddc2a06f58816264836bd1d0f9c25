using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DLTD.Web.Main.DAL;
using DLTD.Web.Main.ViewModels;
using DLTD.Web.Main.Common;
using DLTD.Web.Main.Models.Enum;

namespace DLTD.Web.Main.Controllers
{
    public enum Report
    {
        ChiTiet = 1,
        TongHop = 2
    }
    public class BaoCaoController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index(Report id)
        {
            var data = await VanBanChiDaoManagement.Go.GetVanBanChiDao(userId: UserIdLogin, tuNgay: null, denNgay: null);

            if (id == Report.ChiTiet)
            {
                ViewBag.Title = "Báo cáo chi tiết";
                return View(new BaoCaoVanBanChiDaoViewModel
                {
                    Report = id,
                    ReportDataString = "VanBanChiDaoData",
                    ReportPath = "Reports/VanBanChiDao.rdlc",
                    Data = data.Select(x => new VanBanChiDaoReportModel
                    {
                        HanXuLy = string.Format("{0:dd/MM/yyyy}", x.ThoiHanXuLy),
                        TinhHinhThucHien = x.TrangThai.GetDisplayValue(),
                        DonViTheoDoi = x.DonVi.Ten,
                        NgayHoanThanh = string.Format("{0:dd/MM/yyyy}", x.NgayHoanThanh),                     
                        NoiDungTheoDoi = x.YKienChiDao,
                        SoKyHieu = x.SoKH,
                        Trichyeu = x.Trichyeu
                    }).ToList().ListToDataTable(),
                });
            }
            ViewBag.Title = "Báo cáo tổng hợp";
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

            var rptData = new List<TongHopVanBanReportModel>();

            rptData.Add(new TongHopVanBanReportModel
            {
                HoanThanh =  sumHT,
                DangThucHien = sumDangXL,
                QuaHan = sumDangXLQuaHan,
                HoanThanhQuaHan = sumHTQuaHan,
                TongNhiemVu = sum
            });

            return View(new BaoCaoVanBanChiDaoViewModel
            {
                Report = id,
                ReportDataString = "TongHopVanBanData",
                ReportPath = "Reports/TongHopVanBan.rdlc",
                Data = rptData.ListToDataTable()
            });
        }

        [HttpPost]
        public async Task<ActionResult> Index(Report id, string tuNgay, string denNgay)
        {

            var data = await VanBanChiDaoManagement.Go.GetVanBanChiDao(UserIdLogin, tuNgay.ToDateTimeExt(), denNgay.ToDateTimeExt());

            if (id == Report.ChiTiet)
            {
                ViewBag.Title = "Báo cáo chi tiết";
                return View(new BaoCaoVanBanChiDaoViewModel
                {
                    Report = id,
                    ReportDataString = "VanBanChiDaoData",
                    ReportPath = "Reports/VanBanChiDao.rdlc",
                    Data = data.Select(x => new VanBanChiDaoReportModel
                    {
                        HanXuLy = string.Format("{0:dd/MM/yyyy}", x.ThoiHanXuLy),
                        TinhHinhThucHien = x.TrangThai.GetDisplayValue(),
                        DonViTheoDoi = x.DonVi.Ten,
                        NgayHoanThanh = string.Format("{0:dd/MM/yyyy}", x.NgayHoanThanh),
                        NoiDungTheoDoi = x.YKienChiDao,
                        SoKyHieu = x.SoKH,
                        Trichyeu = x.Trichyeu,
                        Ngayky = string.Format("{0:dd/MM/yyyy}", x.Ngayky),
                    }).ToList().ListToDataTable(),
                    FromDate = tuNgay,
                    ToDate = denNgay
                });

            }

            ViewBag.Title = "Báo cáo tổng hợp";

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

            var rptData = new List<TongHopVanBanReportModel>();

            rptData.Add(new TongHopVanBanReportModel
            {
                HoanThanh =  sumHT,
                //DangThucHien = string.Format("{0:#,###}", sumDangXL),
                DangThucHien =  sumDangXL,
                QuaHan = sumDangXLQuaHan,
                HoanThanhQuaHan = sumHTQuaHan,
                TongNhiemVu =  sum
            });

            return View(new BaoCaoVanBanChiDaoViewModel
            {
                Report = id,
                ReportDataString = "TongHopVanBanData",
                ReportPath = "Reports/TongHopVanBan.rdlc",
                Data = rptData.ListToDataTable()
            });
        }

        public ActionResult TinhHinhXuLy(LoaiThongKe loai, string tuNgay, string denNgay, int? key, string name,
            int? keyext, string nameext)
        {

            var model = new BaoCaoTinhHinhXuLyViewModel();
            switch (loai)
            {
                case LoaiThongKe.Chuyenvientheodoi:
                    model.BaoCaoTheo = "Người theo dõi";
                    if (key.HasValue) model.DieuKienTimKiem = "Chuyên viên: " + name;

                    model.Data = VanBanChiDaoManagement.Go.ThongKeVanBanTheoNguoiTheoDoi(UserIdLogin, tuNgay.ToDateTimeExt(),
                        denNgay.ToDateTimeExt(), key).ListToDataTable();

                    break;
                case LoaiThongKe.DonViXuLyChinh:
                    model.BaoCaoTheo = "Đơn vị xử lý chính";
                    if (keyext.HasValue) model.DieuKienTimKiem = "Khối: " + nameext;
                    if (key.HasValue) model.DieuKienTimKiem += "Đơn vị: " + name;

                    model.Data = VanBanChiDaoManagement.Go.ThongKeVanBanTheoDonViXuLy(UserIdLogin, tuNgay.ToDateTimeExt(),
                        denNgay.ToDateTimeExt(), keyext, key).ListToDataTable();

                    break;
                case LoaiThongKe.Nguoichidao:
                    model.BaoCaoTheo = "Người chỉ đạo";
                    if (key.HasValue) model.DieuKienTimKiem = "Lãnh đạo: " + name;
                    model.Data = VanBanChiDaoManagement.Go.ThongKeVanBanTheoNguoiChiDao(UserIdLogin, tuNgay.ToDateTimeExt(),
                        denNgay.ToDateTimeExt(), key).ListToDataTable();
                    break;
                default: //LoaiThongKe.NguonChiDao:
                    model.BaoCaoTheo = "Nguồn chỉ đạo";
                    if (key.HasValue) model.DieuKienTimKiem = "Nguồn chỉ đạo: " + name;
                    model.Data = VanBanChiDaoManagement.Go.ThongKeVanBanTheoNguonChiDao(UserIdLogin, tuNgay.ToDateTimeExt(),
                        denNgay.ToDateTimeExt(), key, null).ListToDataTable();
                    break;
            }

            if (tuNgay.ToDateTimeExt().HasValue) model.DieuKienTimKiem += "; Từ ngày: " + tuNgay;
            if (denNgay.ToDateTimeExt().HasValue) model.DieuKienTimKiem += "; Đến ngày: " + denNgay;

            return View(model);
        }
    }
}