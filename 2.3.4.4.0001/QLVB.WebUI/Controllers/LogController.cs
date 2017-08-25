using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Domain.Entities;
using QLVB.Core.Contract;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using QLVB.DTO.Log;

namespace QLVB.WebUI.Controllers
{
    public class LogController : Controller
    {
        #region Constructor
        private ILogManager _logManager;
        public LogController(ILogManager logManager)
        {
            _logManager = logManager;
        }
        #endregion Constructor

        //
        // GET: /Log/
        public ActionResult Index(int? intloai)
        {
            if (intloai > 0)
            {
                ViewBag.intloai = (int)intloai;
            }
            else
            {
                ViewBag.intloai = 1;
            }
            return View();
        }

        public ActionResult Log_Read([DataSourceRequest]DataSourceRequest request, int intloai)
        {
            var nhatky = GetLog(intloai).ToDataSourceResult(request);

            return Json(nhatky);

        }

        private IEnumerable<LogViewModel> GetLog(int intloai)
        {
            var log = _logManager.GetNhatkysudung(intloai).OrderByDescending(p => p.time_stamp);
            return log;
        }
        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}