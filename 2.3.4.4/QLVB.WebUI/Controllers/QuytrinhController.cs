using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;
using QLVB.DTO;
using QLVB.DTO.Quytrinh;
using QLVB.Core.Contract;

namespace QLVB.WebUI.Controllers
{
    public class QuytrinhController : Controller
    {
        #region Constructor

        private IQuytrinhManager _quytrinh;
        public QuytrinhController(IQuytrinhManager quytrinh)
        {
            _quytrinh = quytrinh;
        }

        #endregion Constructor

        #region Index
        public ActionResult Index(int? id)
        {
            if (QLVB.Common.Utilities.AppSettings.IsWorkflow)
            {
                ViewBag.id = (id > 0) ? id : 0;
                string strtenquytrinh = _quytrinh.GetFlowChartName(ViewBag.id);
                ViewBag.strtenquytrinh = (!string.IsNullOrEmpty(strtenquytrinh)) ? strtenquytrinh : "Quy trình xử lý";
                return View();
            }
            else
            {
                return View("Error");
            }
        }


        public ActionResult _Listloaiquytrinh()
        {
            ListLoaiQuytrinhViewModel model = _quytrinh.GetListLoaiQuytrinh();
            return PartialView(model);
        }

        public ActionResult _Chitietquytrinh(int? id)
        {
            string jsFlowchart = _quytrinh.ReadFlowChart((int)id);
            ViewBag.jsFlowchart = jsFlowchart;
            ViewBag.idquytrinh = (int)id;
            return PartialView();
        }
        #endregion Index

        #region Loai quy trinh

        public ActionResult _formEditLoaiquytrinh(int id)
        {
            EditLoaiQuytrinhViewModel model = _quytrinh.GetEditLoaiQuytrinh(id);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SaveLoaiquytrinh(int id, string strten)
        {
            ResultFunction kq = _quytrinh.SaveLoaiQuytrinh(id, strten);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }
        public ActionResult _formDeleteLoaiquytrinh(int id)
        {
            return PartialView();
        }

        #endregion Loai quy trinh


        #region Quy trinh

        public ActionResult _formEditQuytrinh(int id)
        {
            EditQuytrinhViewModel model = _quytrinh.GetEditQuytrinh(id);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Savequytrinh(EditQuytrinhViewModel model, string dteThoigianApdung)
        {
            model.dteThoigianApdung = QLVB.Common.Date.DateServices.FormatDateEn(dteThoigianApdung);

            ResultFunction kq = _quytrinh.SaveQuytrinh(model);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        [HttpPost]
        public ActionResult SaveFlowChart(int id, string strjson)
        {
            ResultFunction kq = _quytrinh.SaveFlowChart(id, strjson);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        [HttpPost]
        public ActionResult DeleteFlowChart(int id)
        {
            ResultFunction kq = _quytrinh.DeleteFlowChart(id);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        [HttpPost]
        public ActionResult SaveVersion(int id)
        {
            ResultFunction kq = _quytrinh.SaveVersion(id);
            return Json(kq.message);
        }
        #endregion Quy trinh

        #region Thongtin xuly

        public ActionResult _formThongtinXuly(int idquytrinh, string NodeId)
        {
            EditThongtinXulyViewModel model = _quytrinh.GetThongtinXuly(idquytrinh, NodeId);
            return PartialView(model);
        }


        [HttpPost]
        public ActionResult _SaveThongtinXuly(EditThongtinXulyViewModel model)
        {
            ResultFunction kq = _quytrinh.SaveThongtinXuly(model);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }


        public JsonResult GetCanbothuchien(int? iddonvi)
        {
            if (iddonvi != null)
            {
                var canbo = _quytrinh.GetListCanbo((int)iddonvi);
                return Json(canbo, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }
        #endregion Thongtin xuly

        #region Version

        public ActionResult Phienban(int id)
        {
            ViewBag.id = id;
            ViewBag.strtenquytrinh = _quytrinh.GetFlowChartName(id);
            return View();
        }

        [ChildActionOnly]
        public ActionResult CategoryVersion(int id)
        {
            CategoryNgayViewModel model = _quytrinh.GetCategoryNgay(id);
            model.idquytrinh = id;
            return PartialView(model);
        }

        public ActionResult ChitietVersion(int id, string ngay)
        {
            string jsFlowchart = _quytrinh.ReadFlowChartVersion(id, ngay);
            ViewBag.jsFlowchart = jsFlowchart;
            ViewBag.idquytrinh = id;
            ViewBag.strngay = ngay;
            return PartialView();
        }
        [HttpPost]
        public ActionResult _LoadThongtinXuly(int idquytrinh, string strngay, string NodeId)
        {
            ViewThongtinXulyViewModel model = _quytrinh.LoadThongtinXuly(idquytrinh, strngay, NodeId);
            return Json(model);
        }

        #endregion Version

    }
}