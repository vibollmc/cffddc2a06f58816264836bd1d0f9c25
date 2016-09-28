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
    public enum LoaiThongKe
    {
        NguonChiDao = 0,
        DonViXuLyChinh = 1,
        Chuyenvientheodoi =2,
        Nguoichidao=3
    }
    public class TinhHinhThucHienController : BaseController
    {
        //
        // GET: /TinhHinhXL/
        [HttpGet]
        public ActionResult Index()
        {
            var model = new TonghopTinhhinhXulyViewModel();
            model.Khoi = DonViManagement.Go.GetNguonChiDao();
            model.KhoiDonVi = DonViManagement.Go.GetKhoi(6, 7, 9);
            //model.DonVi = DonViManagement.Go.GetDonViTheoDoi(null, null);
            model.Nguoichidao = DangNhapManagement.Go.GetTonghopNguoichidao();
            model.Nguoitheodoi = DangNhapManagement.Go.GetTonghopNguoitheoDoi();
            return View(model);
        }

        public JsonResult GetCascadeDonVi(int? idKhoiDonVi, string donviFilter)
        {
            var donVi = DonViManagement.Go.GetDonViTheoDoi(idKhoiDonVi, donviFilter);

            return Json(donVi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ThongKe(LoaiThongKe thongKeTheo, 
            string tuNgay, 
            string denNgay, 
            int? idNguonChiDao, 
            int? idDonvi, 
            int? idKhoiDonVi,
            int? idnguoitheodoi, 
            int? idnguoichidao,
            TrangThaiVanBan?trangthai)
         {

             ViewBag.LoaiThongKe = thongKeTheo;             
             IList<TinhHinhXuLyViewModel> model;
             if (thongKeTheo == LoaiThongKe.NguonChiDao)
             {
                 model = VanBanChiDaoManagement.Go.ThongKeVanBanTheoNguonChiDao(UserIdLogin, tuNgay.ToDateTimeExt(),
                     denNgay.ToDateTimeExt(), idNguonChiDao, trangthai);
             }
             else if(thongKeTheo==LoaiThongKe.DonViXuLyChinh)
             {
                 model = VanBanChiDaoManagement.Go.ThongKeVanBanTheoDonViXuLy(UserIdLogin, tuNgay.ToDateTimeExt(),
                     denNgay.ToDateTimeExt(), idKhoiDonVi, idDonvi);
             }
             else if (thongKeTheo == LoaiThongKe.Chuyenvientheodoi)
             {
                 model = VanBanChiDaoManagement.Go.ThongKeVanBanTheoNguoiTheoDoi(UserIdLogin, tuNgay.ToDateTimeExt(),
                    denNgay.ToDateTimeExt(), idnguoitheodoi);
             }
             else if (thongKeTheo == LoaiThongKe.Nguoichidao)
             {
                 model = VanBanChiDaoManagement.Go.ThongKeVanBanTheoNguoiChiDao(UserIdLogin, tuNgay.ToDateTimeExt(),
                    denNgay.ToDateTimeExt(), idnguoichidao);
             }
             else
             {
                 model = new List<TinhHinhXuLyViewModel>();
             }

             return PartialView("_TonghopTHTH", model);
        }  
	}
}