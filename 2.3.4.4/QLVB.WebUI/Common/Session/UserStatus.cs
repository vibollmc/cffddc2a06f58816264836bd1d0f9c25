using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using QLVB.WebUI.Models;
using QLVB.WebUI.Common.Role;
using QLVB.Common.Utilities;

namespace QLVB.WebUI.Common.Session
{
    /// <summary>
    ///  kiem tra trang thai cua user: login/logout from owin
    /// </summary>
    public class UserStatus
    {
        public static UserOwin GetOwinCookie()
        {
            var ctx = HttpContext.Current.Request.GetOwinContext();
            ClaimsPrincipal user = ctx.Authentication.User;
            IEnumerable<Claim> claims = user.Claims;
            int userid = 0;
            string username = string.Empty;
            string id = string.Empty;
            foreach (var p in claims)
            {
                if (p.Type == ClaimTypes.Role)
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
            UserOwin userOwin = new UserOwin
            {
                Identity = id,
                UserName = username,
                UserId = userid
            };
            return userOwin;
        }

        public static object[] GetUserRole()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session[AppConts.SessionUserId]);
            if (userid == 0)
            {
                // load tu cookie
                userid = GetOwinCookie().UserId;
                HttpContext.Current.Session[AppConts.SessionUserId] = userid;
                string username = GetOwinCookie().UserName;
                HttpContext.Current.Session[AppConts.SessionUserName] = username;
                QLVB.WebUI.Common.NLog.NLogLogger _logger = new NLog.NLogLogger();
                _logger.Info("Đăng nhập thành công từ cookie");
            }

            var userroles = RoleServices.GetUserRole(userid);
            return userroles;
        }

    }
}