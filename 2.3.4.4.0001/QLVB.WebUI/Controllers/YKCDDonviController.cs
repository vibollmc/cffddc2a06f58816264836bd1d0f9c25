using QLVB.Common.Sessions;
using QLVB.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLVB.WebUI.Controllers
{
    public class YKCDDonviController : Controller
    {
        #region Constructor
        private IVanbandiManager _vanban;
        private ISessionServices _session;
        private IYKCDDonviManager _ykcdDonvi;
        public YKCDDonviController(IVanbandiManager vanban, ISessionServices session, IYKCDDonviManager ykcdDonvi)
        {
            _vanban = vanban;
            _session = session;
            _ykcdDonvi = ykcdDonvi;
        }

        #endregion Constructor

        public ActionResult Index()
        {
            return View();
        }
    }
}