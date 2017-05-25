using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace QLVB.WebUI.Controllers
{
    public class OAuthController : Controller
    {
        public ActionResult Authorize()
        {
            if (Response.StatusCode != 200)
            {
                return View("AuthorizeError");
            }

            var authentication = HttpContext.GetOwinContext().Authentication;
            var ticket = authentication.AuthenticateAsync(DefaultAuthenticationTypes.ApplicationCookie).Result;
            var identity = ticket != null ? ticket.Identity : null;
            if (identity == null)
            {
                authentication.Challenge(DefaultAuthenticationTypes.ApplicationCookie);
                return new HttpUnauthorizedResult();
            }

            var scopes = (Request.QueryString.Get("scope") ?? "").Split(' ');

            identity = new ClaimsIdentity(identity.Claims, "Bearer", identity.NameClaimType, identity.RoleClaimType);
            foreach (var scope in scopes)
            {
                identity.AddClaim(new Claim("urn:oauth:scope", scope));
            }
            authentication.SignIn(identity);

            //TODO: Handler in case not allow login into CSDLTD 
            if (Request.HttpMethod == "POST")
            {  
                if (!string.IsNullOrEmpty(Request.Form.Get("submit.Login")))
                {
                    authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    authentication.Challenge(DefaultAuthenticationTypes.ApplicationCookie);
                    return new HttpUnauthorizedResult();
                }
            }

            return View();
        }
	}
}