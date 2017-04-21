using QLVB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLVB.WebUI.Common
{
    public class CommonUtils
    {
        //#region khoitao

        //private static IMenuRepository _menuRepo;
        //private static ILogger _logger;
        //private static IUyQuyenRepository _uyquyenRepo;

        //public CommonUtils(IMenuRepository menuRepo, ILogger logger,
        //                    IUyQuyenRepository uyquyenRepo)
        //{
        //    _menuRepo = menuRepo;
        //    _logger = logger;
        //    _uyquyenRepo = uyquyenRepo;
        //}

        //#endregion khoitao




        private static QLVBDatabase context = new QLVBDatabase();

        /// <summary>
        /// Lấy tên menu đang yêu cầu dựa trên request controller và action
        /// cho biết người dùng đang ở vị trí nào
        /// </summary>
        /// <returns>string tên menu</returns>
        public static string GetMenuName()
        {
            string controllername = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            string actionname = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            string myroute = "";

            var menucontroller = context.Menus
                        .Where(p => p.strcontroller == controllername);
            //var menucontroller = _menuRepo.Menus
            //            .Where(p => p.strcontroller == controllername);

            if (menucontroller.Count() != 0)
            {
                var menuaction = menucontroller.FirstOrDefault(p => p.straction == actionname);
                if (menuaction != null)
                {
                    myroute = menuaction.strmota;
                }
                else
                {
                    myroute = menucontroller.FirstOrDefault().strmota;
                }
            }

            if (String.IsNullOrEmpty(myroute))
            {
                myroute = "Trang Chủ";
            }
            return myroute;
        }


    }
}