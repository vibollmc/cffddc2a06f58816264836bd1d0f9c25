using Microsoft.Owin;
using QLVB.Common.Sessions;
using QLVB.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace QLVB.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IAccountManager _account;
        private ISessionServices _session;

        public HomeController(IAccountManager account, ISessionServices session)
        {
            _account = account;
            _session = session;
        }
        public ActionResult Index()
        {
            int iduser = _session.GetUserId();
            QLVB.DTO.Account.HomeViewModel homepage = _account.GetHomePage(iduser);
            return RedirectToAction(homepage.straction, homepage.strcontroller);

        }

        public ActionResult About()
        {
            var ctx = Request.GetOwinContext();
            ClaimsPrincipal user = ctx.Authentication.User;
            IEnumerable<Claim> claims = user.Claims;
            int userid = 0;
            string username = string.Empty;
            string id = string.Empty;
            foreach (var p in claims)
            {
                var role = p;
                if (p.Type == ClaimTypes.Role) // http://schemas.microsoft.com/ws/2008/06/identity/claims/role: 1
                {
                    userid = Convert.ToInt32(p.Value);
                }
                if (p.Type == ClaimTypes.Name)
                {
                    username = p.Value;
                }
                if (p.Type == ClaimTypes.NameIdentifier)
                {
                    id = p.Value;
                }
            }
            ViewBag.userid = userid;
            ViewBag.username = username;
            ViewBag.id = id;

            var currentUserId = User.Identity.Name;
            ViewBag.User = currentUserId;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}