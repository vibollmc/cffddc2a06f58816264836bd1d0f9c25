using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Domain.Entities;
using QLVB.Core.Contract;
using QLVB.Common.Sessions;
using QLVB.Common.Utilities;
using QLVB.DTO.Menu;

namespace QLVB.WebUI.Controllers
{
    public class MenuController : Controller
    {
        #region Constructor

        private IMenuManager _menu;
        private IVanbandientuManager _vbdt;
        private ISessionServices _session;

        public MenuController(IMenuManager menu, ISessionServices session, IVanbandientuManager vbdt)
        {
            _menu = menu;
            _session = session;
            _vbdt = vbdt;
        }

        #endregion Constructor

        /// <summary>
        /// HIEN KHONG SU DUNG
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult KendoMenu()
        {
            int userid = _session.GetUserId();
            IQueryable<Menu> kmenu = _menu.GetMainMenu(userid);
            return PartialView(kmenu);
        }


        [ChildActionOnly]
        public PartialViewResult Header()
        {
            ViewBag.isAutoReceiveMail = _menu.CheckAutoReceiveMail();
            ViewBag.isAutoSendMail = _menu.CheckAutoSendMail();
            ViewBag.intTimeAutoMail = AppSettings.TimeAutoEmail;

            QLVB.DTO.Hethong.SettingSendTonghopVBViewModel thvb = _menu.GetSettingSendTHVB();
            ViewBag.isSendTonghopVB = thvb.IsSendTonghopVb;
            ViewBag.intTimeAutoSendTHVB = thvb.TimeAutoSend;
            ViewBag.intTimeStartSendTHVB = thvb.TimeSend;

            ViewBag.isStartSendTHVB = _menu.CheckTimeStartSendTHVB(thvb);

            int userid = _session.GetUserId();

            if (userid == 0)
            {   // kiem tra login tu cookie da luu
                var currentUser = HttpContext.User.Identity.Name;
                if (!string.IsNullOrEmpty(currentUser))
                {
                    userid = _menu.GetIdUser(currentUser);
                    //============================================
                    // chua load quyen cua user vao session
                    //============================================
                }
            }

            HeaderViewModel model = _menu.GetHeaderUser(userid);

            ViewBag.strMenuType = _menu.GetMenuType(userid);

            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult SideBarLeft()
        {
            int userid = _session.GetUserId();

            if (userid == 0)
            {   // kiem tra login tu cookie da luu
                var currentUser = HttpContext.User.Identity.Name;
                if (!string.IsNullOrEmpty(currentUser))
                {
                    userid = _menu.GetIdUser(currentUser);
                    //============================================
                    // chua load quyen cua user vao session
                    //============================================
                }
                // testing--------------------
                //var context = Request.GetOwinContext();
                //context.Request.Cookies()

            }

            string controllername = HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            string actionname = HttpContext.Request.RequestContext.RouteData.Values["action"].ToString();

            MenuViewModels metis = new MenuViewModels();
            metis.SubMenu = _menu.GetSubMenu(userid);
            metis.ActiveMenu = _menu.GetParentIdMenu(controllername, actionname);
            metis.RootMenu = _menu.GetMainMenu(userid);

            //metis.headerViewModel = new HeaderViewModel();
            //metis.headerViewModel.UserName = SessionServices.GetObject(AppConts.SessionUserName).ToString();
            //metis.headerViewModel.Chucdanh =
            //metis.headerViewModel.uyquyen = _menu.GetListUserUyquyen();

            //metis.headerViewModel = _menu.GetHeaderUser(userid);

            ViewBag.isclickMenu = _menu.CheckIsClickMenu(userid);
            ViewBag.strMenuType = _menu.GetMenuType(userid);

            return PartialView(metis);
        }

        [ChildActionOnly]
        public PartialViewResult SideBarRight()
        {
            return PartialView();
        }
    }
}