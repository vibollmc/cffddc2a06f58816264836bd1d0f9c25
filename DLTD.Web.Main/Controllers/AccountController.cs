using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using DLTD.Web.Main.Common;
using DLTD.Web.Main.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using DLTD.Web.Main.Models;
using DLTD.Web.Main.Models.Enum;
using DLTD.Web.Main.ViewModels;
using DotNetOpenAuth.OAuth2;

namespace DLTD.Web.Main.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private WebServerClient _webServerClient;
        private readonly string _authorizationServerUri;
        private readonly string _oauthPath = "/OAuth/Authorize";
        private readonly string _tokenPath = "/OAuth/Token";
        private readonly string _mePath = "/Api/Me";
        private readonly string _clientId;
        private readonly string _clientSecret;

        public AccountController()
        {
            var serverQLVBOauthPath = WebConfigurationManager.AppSettings["ServerQLVBOauthPath"];
            _clientId = WebConfigurationManager.AppSettings["ClientId"];
            _clientSecret = WebConfigurationManager.AppSettings["ClientSecret"];

            _authorizationServerUri = serverQLVBOauthPath;
            var subpath = string.Empty;
            while (this._authorizationServerUri.LastIndexOf("/") > 7)
            {
                subpath = this._authorizationServerUri.Substring(this._authorizationServerUri.LastIndexOf("/")) + subpath;
                _authorizationServerUri = this._authorizationServerUri.Substring(0, this._authorizationServerUri.LastIndexOf("/"));
            }
            _oauthPath = subpath + _oauthPath;
            _tokenPath = subpath + _tokenPath;
            _mePath = subpath + _mePath;

            InitializeWebServerClient();
        }

        private void InitializeWebServerClient()
        {
            var authorizationServerUri = new Uri(_authorizationServerUri);
            var authorizationServer = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(authorizationServerUri, _oauthPath),
                TokenEndpoint = new Uri(authorizationServerUri, _tokenPath),
                ProtocolVersion = ProtocolVersion.V20
            };
            _webServerClient = new WebServerClient(authorizationServer, _clientId, _clientSecret);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        private void SignIn(DangNhap user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, string.Format("{0}", user.Email)),
                new Claim(ClaimTypes.NameIdentifier, string.Format("{0}", user.Id)),
                new Claim(ClaimTypes.Name, string.Format("{0}", user.TenDangNhap)),
                new Claim(ClaimTypes.GivenName, string.Format("{0}", user.Ten)),
                new Claim(ClaimTypes.Uri, string.Format("{0}", user.UrlImage)),
                new Claim(ClaimTypes.Locality, string.Format("{0}",user.DangNhapTu)),
                new Claim(ClaimTypes.PrimaryGroupSid, string.Format("{0}", user.NhomNguoiDung))
            };

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(
                new AuthenticationProperties()
                {
                    IsPersistent = isPersistent,
                }, identity);
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public async Task<ActionResult> Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            var accessToken = string.Format("{0}", Session["AccessToken"]);

            if (string.IsNullOrEmpty(accessToken))
            {
                var authorizationState = _webServerClient.ProcessUserAuthorization(Request);
                if (authorizationState != null)
                {
                    var cookie = new HttpCookie("QLVBTokens");
                    cookie["AccessToken"] = authorizationState.AccessToken;
                    cookie["RefreshToken"] = authorizationState.RefreshToken;

                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(cookie);
                    Session["AccessToken"] = authorizationState.AccessToken;

                    return await SignInWithTokenKey(authorizationState.AccessToken);
                }
                else //auto redirect to qlvb's login
                {
                    SingleSignOnWithQlvb();
                }
            }
            else
            {
                return await SignInWithTokenKey(accessToken);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private void SingleSignOnWithQlvb()
        {
            Session["AccessToken"] = null;
            Response.Cookies.Remove("QLVBTokens");

            var userAuthorization = _webServerClient.PrepareRequestUserAuthorization(new[] { "SignIn", "Roles" });
            userAuthorization.Send(HttpContext);
            Response.End();
        }

        private async Task<ActionResult> SignInWithTokenKey(string tokenKey)
        {
            var resourceServerUri = new Uri(_authorizationServerUri);
            var client =
                new HttpClient(_webServerClient.CreateAuthorizingHandler(tokenKey));
            var body = await client.GetStringAsync(new Uri(resourceServerUri, _mePath));

            if (string.IsNullOrWhiteSpace(body.Trim('"')))
            {
                Session["AccessToken"] = null;
                Response.Cookies.Remove("QLVBTokens");

                return await Login("/");
            }

            body = body.Trim('"');
            var user = body.Split('$');

            var userData = new DangNhap
            {
                TenDangNhap = user[0],
                Ma = user[1],
                Ten = user[2],
                Email = user[3],
                SoDienThoai = user[4],
                NgaySinh = user[5].ToDateTimeExt(),
                UrlImage = string.IsNullOrWhiteSpace(user[6]) ? null : resourceServerUri + user[6],
                IdDonVi = user[7].ToIntExt(),
                GioiTinh = (GioiTinh)user[8].ToIntExt(),
                NhomNguoiDung =
                    string.IsNullOrWhiteSpace(user[9])
                        ? NhomNguoiDung.Undefined
                        : (NhomNguoiDung)user[9].ToIntExt(),
                DangNhapTu = LoaiQlvb.Qlvb,
                TrangThai = TrangThai.Active
            };

            userData = await DangNhapManagement.Go.SaveUserLoginWithThirdParty(userData);

            SignIn(userData, false);

            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!string.IsNullOrEmpty(Request.Form.Get("submit.Authorize")))
            {
                SingleSignOnWithQlvb();
            }
            else if (!string.IsNullOrEmpty(Request.Form.Get("submit.Login")))
            {
                var user = await DangNhapManagement.Go.Login(model.UserName, model.Password);
                if (user != null)
                {
                    SignIn(user, model.RememberMe);
                    if (string.IsNullOrWhiteSpace(returnUrl)) return RedirectToAction("Index", "Home");

                    return Redirect(returnUrl);
                }

                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        [HttpPost]
        public ActionResult Logout()
        {
            Session["AccessToken"] = null;
            Response.Cookies.Remove("QLVBTokens");

            AuthenticationManager.SignOut();

            var urlBuilder =
                        new System.UriBuilder(Request.Url.AbsoluteUri)
                        {
                            Path = Url.Action("Login", "Account"),
                            Query = null,
                        };

            var uri = urlBuilder.Uri;
            var url = urlBuilder.ToString();

            var logoutUrl = WebConfigurationManager.AppSettings["ServerQLVBOauthPath"] + "/Account/LogOff?returlUrl=" + Server.UrlEncode(url);

            return Redirect(logoutUrl);
        }

    }
}