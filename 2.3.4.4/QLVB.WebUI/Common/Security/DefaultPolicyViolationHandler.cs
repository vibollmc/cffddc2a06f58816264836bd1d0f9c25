using FluentSecurity;
using QLVB.Common.Utilities;
using QLVB.WebUI.Common.NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QLVB.WebUI.Common.Security
{
    public class DefaultPolicyViolationHandler : IPolicyViolationHandler
    {
        public string ViewName = AppConts.ErrAccessDenied;
        private NLogLogger logger = new NLogLogger();

        public ActionResult Handle(PolicyViolationException exception)
        {
            ////return new HttpUnauthorizedResult(exception.Message);

            ////return new RedirectResult("../Error/AccessDenied");
            //return new RedirectToRouteResult(new RouteValueDictionary(new
            //{
            //    action = "Index",
            //    controller = "Account",
            //    area = ""
            //}));

            logger.Warn(exception.Message);

            string menu = CommonUtils.GetMenuName();

            if (SecurityServices.UserIsAuthenticated())
            {
                logger.Warn("Bạn không có quyền truy cập menu: " + menu + " , địa chỉ: " + HttpContext.Current.Request.RawUrl);
                return new ViewResult { ViewName = ViewName };
            }

            else
            {
                RouteValueDictionary rvd = new RouteValueDictionary();

                //if (System.Web.HttpContext.Current.Request.RawUrl != "/")
                //{
                //    rvd["ReturnUrl"] = System.Web.HttpContext.Current.Request.RawUrl;
                //}

                rvd["controller"] = "Account";
                rvd["action"] = "LogOff";
                rvd["area"] = "";

                //logger.Warn("TRUY CẬP CHƯA ĐĂNG NHẬP HỆ THỐNG: " + menu + " " + HttpContext.Current.Request.Url);
                return new RedirectToRouteResult(rvd);
            }
        }

    }
}