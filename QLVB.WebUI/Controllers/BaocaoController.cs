using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using QLVB.DTO.Baocao;
using QLVB.Common.Date;
using Microsoft.Reporting.WebForms;
using System.Data;
using QLVB.Common.Utilities;


namespace QLVB.WebUI.Controllers
{
    public class BaocaoController : Controller
    {
        #region Constructor

        private IBaocaoManager _baocao;

        public BaocaoController(IBaocaoManager baocao)
        {
            _baocao = baocao;
        }

        #endregion Constructor

        #region FormBaocao
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult _Toolbar()
        {
            //ngayhientai.DayOfWeek
            ViewBag.strngaybd = DateTime.Now;
            ViewBag.strngaykt = DateTime.Now.AddDays(7);

            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult _Category()
        {
            CategoryBaocaoViewModel model = _baocao.GetCategory();
            return PartialView(model);
        }


        public ActionResult _DetailSovb(int intloai)
        {
            var sovb = _baocao.GetSovanban(intloai);
            if (intloai == 1)
            {
                // so van ban den
                var donvi = _baocao.GetListDonvi();
                ViewBag.listdonvi = donvi;
            }

            return PartialView(sovb);
        }

        public ActionResult _DetailLoaivb(int intloai)
        {
            var loaivb = _baocao.GetLoaivanban(intloai);

            return PartialView(loaivb);
        }
        public ActionResult _Detailvbdt(int intloai)
        {
            return PartialView();
        }

        #endregion FormBaocao

        #region Report
        public ActionResult Sovanbanden(string strngaybd, string strngaykt, int? sovb, string listidso, bool IsXuly, int iddonvi)
        {
            SoVanbandenViewModel sovbDTO = new SoVanbandenViewModel();
            sovbDTO.idsovanban = sovb;
            sovbDTO.strTungay = strngaybd;
            sovbDTO.strDenngay = strngaykt;
            sovbDTO.IsXuly = IsXuly;
            sovbDTO.iddonvi = iddonvi;
            sovbDTO.strtenphong = _baocao.GetTenPhong(iddonvi, IsXuly);
            sovbDTO.strTenso = _baocao.GetTensovanban(listidso); //_baocao.GetTensovanban((int)sovb);
            sovbDTO.strTendonvi = _baocao.GetTenDonvi();

            List<int> listid = new List<int>();
            string[] split = listidso.Split(',');
            foreach (var s in split)
            {
                try
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        int id = Convert.ToInt32(s);
                        listid.Add(id);
                    }
                }
                catch { }
            }
            sovbDTO.listidso = listid;


            var vanban = _baocao.GetSovanbanden(sovbDTO);
            ReportSoVanbandenViewModel model = new ReportSoVanbandenViewModel();
            model.Sovanban = QLVB.Common.Utilities.ListtoDataTableConverter.ToDataTable(vanban);
            model.Thamso = sovbDTO;

            return View(model);
        }

        public ActionResult Sovanbandi(string strngaybd, string strngaykt, int sovb)
        {
            SovanbandiViewModel sovbDTO = new SovanbandiViewModel();
            sovbDTO.idsovanban = sovb;
            sovbDTO.strTungay = strngaybd;
            sovbDTO.strDenngay = strngaykt;
            sovbDTO.strTenso = _baocao.GetTensovanban(sovb);
            sovbDTO.strTendonvi = _baocao.GetTenDonvi();

            var vanban = _baocao.GetSovanbandi(sovbDTO);
            ReportSovanbandiViewModel model = new ReportSovanbandiViewModel();
            model.Sovanban = ListtoDataTableConverter.ToDataTable(vanban);
            model.Thamso = sovbDTO;

            return View(model);
        }

        public ActionResult Loaivanbanden(string strngaybd, string strngaykt, int loaivb)
        {
            LoaivanbandenViewModel loaivbDTO = new LoaivanbandenViewModel();
            loaivbDTO.idloaivanban = loaivb;
            loaivbDTO.strTungay = strngaybd;
            loaivbDTO.strDenngay = strngaykt;
            loaivbDTO.strTenloai = _baocao.GetTenLoaivanban(loaivb);
            loaivbDTO.strTendonvi = _baocao.GetTenDonvi();

            var vanban = _baocao.GetLoaivanbanden(loaivbDTO);
            ReportLoaivanbandenViewModel model = new ReportLoaivanbandenViewModel();
            model.Loaivanban = ListtoDataTableConverter.ToDataTable(vanban);
            model.Thamso = loaivbDTO;

            return View(model);
        }


        public ActionResult Loaivanbandi(string strngaybd, string strngaykt, int loaivb)
        {
            LoaivanbandiViewModel loaivbDTO = new LoaivanbandiViewModel();
            loaivbDTO.idloaivanban = loaivb;
            loaivbDTO.strTungay = strngaybd;
            loaivbDTO.strDenngay = strngaykt;
            loaivbDTO.strTenloai = _baocao.GetTenLoaivanban(loaivb);
            loaivbDTO.strTendonvi = _baocao.GetTenDonvi();

            var vanban = _baocao.GetLoaivanbandi(loaivbDTO);
            ReportLoaivanbandiViewModel model = new ReportLoaivanbandiViewModel();
            model.Loaivanban = ListtoDataTableConverter.ToDataTable(vanban);
            model.Thamso = loaivbDTO;

            return View(model);
        }


        #endregion Report

        #region InphieunhanHS

        public ActionResult InPhieuNhanVBDen(int id)
        //(string strngaybd, string strngaykt, string strngayhientai,
        //string strtendonvi, string strtrichyeu, string strnguoitiepnhan, string strtensovanban)
        {
            PhieunhanVBDenViewModels model = _baocao.GetNoidungPhieuVBDen(id);


            return View(model);
        }

        #endregion InphieunhanHS

    }
}