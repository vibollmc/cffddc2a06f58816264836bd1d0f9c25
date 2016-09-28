using QLVB.Common.Sessions;
using QLVB.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.WebUI.Common.Security;

namespace QLVB.WebUI.Controllers
{
    public class SessionController : Controller
    {
        private ISessionServices _session;
        public SessionController(ISessionServices session)
        {
            _session = session;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KeepSession()
        {
            _session.InsertObject("KeepSessionAlive", DateTime.Now);
            return Json("nothing");
        }

        public ActionResult CheckSession()
        {
            int userid = _session.GetUserId(); //UserStatus.GetOwinCookie().UserId;
            if (userid == 0)
            {
                return Json(new { status = 0 });
            }
            else
            {
                return Json(new { status = 1 });
            }

            //if (SessionServices.GetObject(AppConts.SessionUserId) == null)
            //{
            //    return Json(new { status = 0 });
            //}
            //else
            //{
            //    return Json(new { status = 1 });
            //}
        }
    }
}