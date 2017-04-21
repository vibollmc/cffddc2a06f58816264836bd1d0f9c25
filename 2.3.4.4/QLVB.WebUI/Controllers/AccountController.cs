using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Core.Contract;
using System.Web.Security;
using QLVB.Common.Sessions;
//using QLVB.DTO.Account;
using QLVB.WebUI.Common.Role;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QLVB.WebUI.Models;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Owin;
using QLVB.DTO.Account;
using QLVB.DTO;
using QLVB.WebUI.Certificate;
using QLVB.Common.Certificate;

using QLVB.Common.Utilities;

namespace QLVB.WebUI.Controllers
{
    public class AccountController : Controller
    {
        #region Constructor

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private IAccountManager _account;
        private ISessionServices _session;
        private ICheckCA _checkCA;
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public AccountController(IAccountManager account, ISessionServices session, ICheckCA checkCA)
        {
            _account = account;
            _session = session;
            _checkCA = checkCA;
        }

        #endregion Constructor

        #region Login

        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Login()
        {
            // CheckCA obj = new CheckCA();
            // bool ca = AppSettings.IsCA;
            string checkca = _checkCA.get_strCheckCA();

            if ((checkca == ""))
            {
                return RedirectToAction("CheckCA", "Certificate");
            }
            else
            {
                //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignOut();
                _session.ClearAllObject();
                QLVB.DTO.Account.LoginViewModel model = new QLVB.DTO.Account.LoginViewModel();

                model.TenDonvi = _account.GetTenDonvi();
                return View(model);

            }


        }


        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(QLVB.DTO.Account.LoginViewModel model, string returnUrl)
        {
            string strtendonvi = _account.GetTenDonvi();

            if (ModelState.IsValid)
            {
                //var user = await UserManager.FindAsync(model.UserName, model.Password);
                //var result = _account.ValidateUser(model.UserName, model.Password);
                int iduser = _account.ValidateUser(model.UserName, model.Password);
                if (iduser > 0)
                {
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = model.UserName;
                    user.PasswordHash = model.Password;
                    user.UserId = iduser;

                    // check luu cookie dang nhap
                    SignInAsync(user, model.RememberMe);
                    //await SignInAsync(user, model.RememberMe);

                    _account.SetTenDonvi(strtendonvi);
                    _account.CheckYKCD();


                    HomeViewModel homepage = _account.GetHomePage(iduser);
                    return RedirectToAction(homepage.straction, homepage.strcontroller);
                }
                else
                {
                    if (iduser == -1)
                    {
                        ModelState.AddModelError("", "Không kết nối được với LDAP Server.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                    }
                }
            }
            model.TenDonvi = strtendonvi;
            return View(model);
        }

        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            _session.ClearAllObject();
            // return RedirectToAction("Login", "Account");
            bool ca = AppSettings.IsCA;
            if (ca == true)
            {
                return RedirectToAction("CheckCA", "Certificate");

            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
            //return RedirectToAction("CheckCA", "Certificate");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #endregion Login


        #region Helpers



        private void SignInAsync(ApplicationUser user, bool isPersistent)
        {
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            //var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            //AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);

            //http://www.codeproject.com/Articles/639458/Claims-Based-Authentication-and-Authorization
            //http://brockallen.com/2013/10/24/a-primer-on-owin-cookie-authentication-middleware-for-the-asp-net-developer/

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, user.UserId.ToString()));
            //claims.Add(new Claim(ClaimTypes.UserData, user.UserId.ToString()));

            var identity = new ClaimsIdentity(claims,
                                        DefaultAuthenticationTypes.ApplicationCookie);

            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(
                new AuthenticationProperties() { IsPersistent = isPersistent },
                identity
            );

            //http://stackoverflow.com/questions/19770001/setting-up-forms-authentication-for-multiple-web-apps-in-mvc-5-based-on-owin

        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion

        #region Doimatkhau
        public ActionResult Doimatkhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(FormCollection collection)
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            string strgiatri = "";
            foreach (string p in collection)
            {
                strgiatri = collection[p];
                if (!string.IsNullOrWhiteSpace(strgiatri))
                {
                    if (p == "OldPassword") model.OldPassword = strgiatri;
                    if (p == "NewPassword") model.NewPassword = strgiatri;
                    if (p == "ConfirmPassword") model.ConfirmPassword = strgiatri;

                }
            }
            ResultFunction kq = _account.ChangePassword(model);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }

        }

        #endregion Doimatkhau

        #region Uyquyen

        public ActionResult Uyquyen()
        {
            ListUserUyquyenViewModel model = _account.GetListUserUyquyen();
            return View(model);
        }


        [HttpPost]
        public ActionResult _SaveUyquyen(string strAddIdUser)
        {
            ResultFunction kq = _account.SaveUyquyen(strAddIdUser);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        /// <summary>
        /// login voi vai tro uyquyen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetUyquyen(int id)
        {
            _account.GetUyquyen(id);

            HomeViewModel homepage = _account.GetHomePage(id);
            return RedirectToAction(homepage.straction, homepage.strcontroller);

        }

        #endregion Uyquyen

        #region Option

        public ActionResult Option()
        {
            OptionViewModel model = _account.GetOption();
            ViewBag.message = TempData["Message"];
            return View(model);
        }

        [HttpPost]
        public ActionResult _SaveOption(FormCollection collection)
        {
            OptionViewModel model = new OptionViewModel();
            List<OptionUserViewModel> listOption = new List<OptionUserViewModel>();
            string strRightOption = string.Empty;
            //string strgiatri = string.Empty;
            foreach (string p in collection)
            {
                //strgiatri = collection[p].ToLower();
                //if (!string.IsNullOrEmpty(strgiatri))
                //{
                //    if (p == "0")
                //    {
                //        model.idOption = Convert.ToInt32(p);
                //        model.blgiatri = strgiatri.Contains("true") ? true : false;
                //    }
                //}
                string col = collection[p];
                if (col.Contains("false") || col.Contains("true"))
                {
                    try
                    {
                        string strgiatri = collection[p].ToLower();
                        OptionUserViewModel option = new OptionUserViewModel();
                        option.intVitri = Convert.ToInt32(p);
                        option.IsValue = strgiatri.Contains("true") ? true : false;

                        listOption.Add(option);
                    }
                    catch { }
                }
            }
            model.ListOption = listOption;

            int kq = _account.SaveOption(model);
            return Json(kq);
        }

        public ActionResult Tuychon()
        {
            var model = _account.GetTuychon();

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult _SaveTuychon(FormCollection collection)
        {

            string strgiatri = "";
            Dictionary<string, string> fCollection = new Dictionary<string, string>();
            foreach (string p in collection)
            {
                strgiatri = collection[p];
                fCollection.Add(p, collection[p]);
            }
            var kq = _account.SaveTuychon(fCollection);
            return Json(kq);
        }

        #endregion Option
    }
}