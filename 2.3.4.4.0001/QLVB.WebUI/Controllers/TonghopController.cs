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
using QLVB.DTO.Tonghop;

namespace QLVB.WebUI.Controllers
{
    public class TonghopController : Controller
    {
        #region Constructor

        private ITonghopManager _tonghopManager;
        public TonghopController(ITonghopManager tonghop)
        {
            _tonghopManager = tonghop;
            //_vbdenManager = vbdenManager;
        }

        #endregion Constructor

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _ListVanbanden()
        {
            return PartialView();
        }

        public ActionResult Vanbanden_Read
           ([DataSourceRequest]DataSourceRequest request)
        {
            return Json(_tonghopManager.GetTonghopVanbanden()
                .OrderByDescending(p => p.dtengayden)
                .ThenByDescending(p => p.intsoden)
                .ToDataSourceResult(request));

        }

        [HttpPost]
        public ActionResult _UpdateStatusVBDen(int idvanban)
        {
            //int intloaitailieu = (int)enumTonghopCanbo.intloaitailieu.Vanbanden;
            ResultFunction kq = _tonghopManager.UpdateStatusVanbanden(idvanban);
            return Json(kq.id);
        }

        public ActionResult _ListHosoXLVBDen()
        {
            return PartialView();
        }

        public ActionResult HosoXLVBDen_Read
           ([DataSourceRequest]DataSourceRequest request)
        {
            return Json(_tonghopManager.GetTonghopHosoXLVBDen()
                .OrderByDescending(p => p.dtengayden)
                .ThenByDescending(p => p.intsoden)
                .ToDataSourceResult(request));

        }
    }
}