using QLVB.Common.Date;
using QLVB.Common.Sessions;
using QLVB.Common.Utilities;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO.Menu;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLVB.Core.Implementation
{
    public class MenuManager : IMenuManager
    {
        #region Constructor

        //private ILogger _logger;
        private ISessionServices _session;
        private IMenuRepository _menuRepo;

        private IQuyenRepository _quyenRepo;
        private IQuyenNhomQuyenRepository _quyennhomRepo;
        private ICanboRepository _canboRepo;
        private IHomeRepository _homeRepo;
        private IUyQuyenRepository _uyquyenRepo;
        private IChucdanhRepository _chucdanhRepo;
        private INhomQuyenRepository _nhomquyenRepo;

        private IRoleManager _roleManager;

        private ITuychonRepository _tuychonRepo;
        private ITuychonCanboRepository _tuychonCanboRepo;
        private IConfigRepository _configRepo;

        public MenuManager(IMenuRepository menurepo, IQuyenRepository quyenRepo,
                    IQuyenNhomQuyenRepository quyennhomRepo,
                    ICanboRepository canboRepo, IHomeRepository homeRepo,
                    IUyQuyenRepository uyquyenRepo, IChucdanhRepository chucdanhRepo,
                    INhomQuyenRepository nhomquyenRepo, ISessionServices session,
                    IRoleManager roleManager,
                    ITuychonRepository tuychonRepo, ITuychonCanboRepository tuychonCanboRepo,
                    IConfigRepository configRepo)
        {
            _menuRepo = menurepo;
            _quyenRepo = quyenRepo;
            _quyennhomRepo = quyennhomRepo;
            _canboRepo = canboRepo;
            _homeRepo = homeRepo;
            //_logger = logger;
            _uyquyenRepo = uyquyenRepo;
            _chucdanhRepo = chucdanhRepo;
            _nhomquyenRepo = nhomquyenRepo;
            _session = session;
            _roleManager = roleManager;
            _tuychonRepo = tuychonRepo;
            _tuychonCanboRepo = tuychonCanboRepo;
            _configRepo = configRepo;
        }

        #endregion Constructor

        #region Interface Implementation

        public IQueryable<Menu> GetMainMenu(int iduser)
        {
            var idnhom = _canboRepo.GetActiveCanbo.FirstOrDefault(p => p.intid == iduser);
            int intidnhomquyen = (int)idnhom.intnhomquyen;

            // chon tat ca cac menu ma user co quyen truy cap
            var mainmenu = _menuRepo.Menus
                        .Where(m => m.inttrangthai == (int)enummenu.inttrangthai.IsActive)
                        .Where(m => //(m.intlevel == 0) ||
                            _quyenRepo.Quyens
                                       .Join(
                                           _quyennhomRepo.QuyenNhomQuyens,
                                           t => t.intid,
                                           u => u.intidquyen,
                                           (t, u) => new { t, u }
                                       ).Where(p => p.u.intidnhomquyen == intidnhomquyen)
                                       .Any(p => p.t.strquyen == m.strquyen)
                        )
                        .OrderBy(p => p.intorder)
                        .ToList();

            // tao menu cha tu cac menu con
            var menu3 = mainmenu.Select(p => p.ParentId);
            var menu4 = _menuRepo.Menus.Where(m => menu3.Contains(m.Id)).OrderBy(p => p.intorder);

            // to menu hierachy tu cac menu da chon
            var submenu = menu4.Where(p => p.ParentId == null);

            //IQueryable<MenuDTO> finalMenu ;//= submenu;

            //return finalMenu;
            return submenu;
        }

        public string GetMenuName(string controllername, string actionname)
        {
            string myroute = "";

            var menucontroller = _menuRepo.Menus
                        .Where(p => p.strcontroller == controllername);

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

        public int GetParentIdMenu(string controllername, string actionname)
        {
            int parentId = 5;
            var menucontroller = _menuRepo.Menus
                        .Where(p => p.strcontroller == controllername);

            if (menucontroller.Count() != 0)
            {
                var menuaction = menucontroller.FirstOrDefault(p => p.straction == actionname);
                if (menuaction != null)
                {
                    parentId = menuaction.ParentId ?? 5;
                }
            }
            // neu parentid null thi lay gia tri 5 : menu Van ban
            //int kq = parentId ?? 5;
            return parentId;
        }

        public IEnumerable<Menu> GetSubMenu(int idcanbo)
        {
            var idnhom = _canboRepo.GetActiveCanbo.FirstOrDefault(p => p.intid == idcanbo);
            int intidnhomquyen = (int)idnhom.intnhomquyen;

            // chon tat ca cac menu ma user co quyen truy cap
            var mainmenu = _menuRepo.Menus
                        .Where(m => m.inttrangthai == (int)enummenu.inttrangthai.IsActive)
                        .Where(m => //(m.intlevel == 0) ||
                            _quyenRepo.Quyens
                                       .Join(
                                           _quyennhomRepo.QuyenNhomQuyens,
                                           t => t.intid,
                                           u => u.intidquyen,
                                           (t, u) => new { t, u }
                                       ).Where(p => p.u.intidnhomquyen == intidnhomquyen)
                                       .Any(p => p.t.strquyen == m.strquyen)
                        )
                        .OrderBy(p => p.intorder);

            var menu = mainmenu.ToList();

            return menu;
        }

        public int GetIdUser(string username)
        {
            int iduser = _canboRepo.GetActiveCanbo.FirstOrDefault(p => p.strusername == username).intid;
            return iduser;
        }

        public IEnumerable<UyquyenUserViewModel> GetListUserUyquyen()
        {
            int RealUserId = _session.GetRealUserId();
            //Convert.ToInt32(_session.GetObject(AppConts.SessionRealUserId));
            try
            {
                var user = _uyquyenRepo.UyQuyens.Where(p => p.intPersRec == RealUserId)
                        .Join(
                            _canboRepo.GetActiveCanbo,
                            u => u.intPersSend,
                            c => c.intid,
                            (u, c) => new { c, u }
                        )
                        .Select(p => new UyquyenUserViewModel
                        {
                            intid = p.c.intid,
                            isRealUser = (p.c.intid == RealUserId) ? true : false,
                            strkyhieu = p.c.strkyhieu,
                            strhoten = p.c.strhoten
                        })
                        .Union(
                            _canboRepo.GetActiveCanbo.Where(p => p.intid == RealUserId)
                            .Select(p => new UyquyenUserViewModel
                            {
                                intid = p.intid,
                                isRealUser = (p.intid == RealUserId) ? true : false,
                                strkyhieu = p.strkyhieu,
                                strhoten = p.strhoten
                            })
                        )
                        .OrderBy(p => p.strkyhieu)
                        .ThenBy(p => p.strhoten)
                        ;

                return user;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }

        }

        public HeaderViewModel GetHeaderUser(int iduser)
        {
            try
            {
                var users = _canboRepo.GetActiveCanbo.Where(p => p.intid == iduser)
               .GroupJoin(
                   _chucdanhRepo.Chucdanhs,
                   u1 => u1.intchucvu,
                   cd => cd.intid,
                   (u1, cd) => new { u1, cd.FirstOrDefault().strtenchucdanh }
               )
               .Join(
                   _nhomquyenRepo.GetActiveNhomQuyens,
                   u2 => u2.u1.intnhomquyen,
                   q => q.intid,
                   (u2, q) => new { u2, q.strtennhom }
               )
               .Select(p => new HeaderViewModel
               {
                   UserName = p.u2.u1.strhoten,
                   Chucdanh = p.u2.strtenchucdanh,
                   Nhomquyen = p.strtennhom,
                   //uyquyen = GetListUserUyquyen()
                   strImageProfile = p.u2.u1.strImageProfile
               })
               .FirstOrDefault();

                users.uyquyen = GetListUserUyquyen();
                users.strngay = DateServices.FormatDateVN(DateTime.Now);

                return users;
            }
            catch
            {
                var users = new HeaderViewModel();

                users.uyquyen = GetListUserUyquyen();
                users.strngay = DateServices.FormatDateVN(DateTime.Now);

                return users;
            }

        }

        public bool CheckIsClickMenu(int iduser)
        {
            string kq = _GetTuychoncanbo(iduser, ThamsoTuychon.IsMenuClickable);

            if (kq.ToLower() == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetMenuType(int iduser)
        {
            return _GetTuychoncanbo(iduser, ThamsoTuychon.MenuType);
        }

        private string _GetTuychoncanbo(int iduser, string strthamso)
        {
            var model = _tuychonRepo.Tuychons.Where(p => p.strthamso == strthamso)
                .GroupJoin(
                    _tuychonCanboRepo.TuychonCanbos.Where(p => p.intidcanbo == iduser),
                    t => t.intid,
                    c => c.intidoption,
                    (t, c) => new { t, c }
                )
                .Select(p => new
                {
                    //intid = p.c.FirstOrDefault().intid,
                    //intgroup = p.t.intgroup,
                    //intorder = p.t.intorder,
                    strmota = p.t.strmota,
                    strgiatri = p.c.FirstOrDefault().strgiatri,
                    strgiatridefault = p.t.strgiatri,
                    strthamso = p.t.strthamso
                })
                .FirstOrDefault();

            string kq = string.IsNullOrEmpty(model.strgiatri) ? model.strgiatridefault : model.strgiatri;

            return kq;
        }

        /// <summary>
        /// kiem tra cau hinh cho phep tu dong nhan mail va quyen nhan vbdt cua user
        /// </summary>
        /// <returns></returns>
        public bool CheckAutoReceiveMail()
        {
            bool kq = false;
            var IsReceiveMail = AppSettings.IsAutoReceiveMail;
            if (IsReceiveMail)
            {
                var isCapnhat = _roleManager.IsRole(RoleVanbandendientu.Capnhat);
                if (isCapnhat)
                {
                    kq = true;
                }
            }
            return kq;
        }
        public bool CheckAutoReceiveVBTructinh()
        {
            bool kq = false;
            var IsReceiveTructinh = AppSettings.IsAutoReceiveTructinh;
            if (IsReceiveTructinh)
            {
                var isCapnhat = _roleManager.IsRole(RoleVanbandendientu.Capnhat);
                if (isCapnhat)
                {
                    kq = true;
                }
            }
            return kq;
        }

        /// <summary>
        ///  kiem tra cau hinh cho phep tu dong send mail va quyen gui vbdt cua user
        /// </summary>
        /// <returns></returns>
        public bool CheckAutoSendMail()
        {
            bool kq = false;
            bool isSendMail = AppSettings.IsAutoSendMail;
            if (isSendMail)
            {
                var isSend = _roleManager.IsRole(RoleVanbandi.GuiEmail);
                if (isSend)
                {
                    kq = true;
                }
            }
            return kq;
        }

        public QLVB.DTO.Hethong.SettingSendTonghopVBViewModel GetSettingSendTHVB()
        {
            QLVB.DTO.Hethong.SettingSendTonghopVBViewModel model = new QLVB.DTO.Hethong.SettingSendTonghopVBViewModel();

            //model.IsSendTonghopVb = _configRepo.GetConfigToBool(ThamsoHethong.IsSendTonghopVB);
            model.IPAddress = _configRepo.GetConfig(ThamsoHethong.IPAddressUBT);
            model.TimeAutoSend = _configRepo.GetConfigToInt(ThamsoHethong.TimeAutoSendVB);
            model.TimeSend = _configRepo.GetConfigToInt(ThamsoHethong.TimeSend);

            bool kq = false;
            bool isSendTHVB = _configRepo.GetConfigToBool(ThamsoHethong.IsSendTonghopVB);
            if (isSendTHVB)
            {
                var isSend = _roleManager.IsRole(RoleVanbandi.GuiEmail);
                if (isSend)
                {
                    kq = true;
                }
            }
            model.IsSendTonghopVb = kq;

            return model;
        }

        public bool CheckTimeStartSendTHVB(QLVB.DTO.Hethong.SettingSendTonghopVBViewModel model)
        {
            bool isSend = false;
            DateTime giohientai = DateTime.Now;
            if (model.IsSendTonghopVb)
            {
                if (giohientai.Hour >= model.TimeSend)
                {
                    isSend = true;
                }
            }           
            return isSend;
        }

        #endregion Interface Implementation


    }
}