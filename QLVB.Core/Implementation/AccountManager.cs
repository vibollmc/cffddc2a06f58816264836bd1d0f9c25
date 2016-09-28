using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO.Account;
using QLVB.Core.Contract;
using QLVB.Common.Logging;
using QLVB.Common.Crypt;
using QLVB.Common.Utilities;
using QLVB.Common.Sessions;
using QLVB.Donvi;
using QLVB.DTO;


namespace QLVB.Core.Implementation
{
    public class AccountManager : IAccountManager
    {
        #region Constructor

        private ILogger _logger;
        private ISessionServices _session;
        private ICanboRepository _canboRepo;
        private IDonvitructhuocRepository _donviRepo;
        private IHomeRepository _homeRepo;
        //private ICanboDonviRepository _canbodonviRepo;
        private IUyQuyenRepository _uyquyenRepo;
        private IConfigRepository _configRepo;
        private IMenuRepository _menuRepo;
        private IQuyenRepository _quyenRepo;
        private ITuychonRepository _tuychonRepo;
        private ITuychonCanboRepository _tuychonCanboRepo;

        private IMenuManager _menuManager;

        public AccountManager(ICanboRepository canborepo, IDonvitructhuocRepository dvrepo,
                IHomeRepository hrepo, //ICanboDonviRepository canbodonviRepo,
                ILogger logger, IUyQuyenRepository uyquyenRepo,
                IConfigRepository configRepo, ISessionServices session,
                IMenuRepository menuRepo, IQuyenRepository quyenRepo,
                ITuychonRepository tuychonRepo, ITuychonCanboRepository tuychonCanboRepo,
                IMenuManager menuManager)
        {
            _canboRepo = canborepo;
            _donviRepo = dvrepo;
            _homeRepo = hrepo;
            // _canbodonviRepo = canbodonviRepo;
            _logger = logger;
            _uyquyenRepo = uyquyenRepo;
            _configRepo = configRepo;
            _session = session;
            _menuRepo = menuRepo;
            _quyenRepo = quyenRepo;
            _tuychonRepo = tuychonRepo;
            _tuychonCanboRepo = tuychonCanboRepo;
            _menuManager = menuManager;
        }
        #endregion Constructor

        #region Interfacer Implementation

        /// <summary>
        /// kiểm tra tên và mật khẩu đăng nhập, return iduser!=0
        /// </summary>
        /// <param name="strusername"></param>
        /// <param name="strpassword"></param>
        /// <returns>id user
        /// 0: khong dang nhap dc
        /// iduser!=0: thanh cong
        /// </returns>
        public int ValidateUser(string strusername, string strpassword)
        {
            if ((String.IsNullOrEmpty(strusername)) || (String.IsNullOrEmpty(strpassword)))
            {
                return 0;
            }
            try
            {
                string strPassMD5 = CryptServices.HashMD5(strpassword);
                var user = _canboRepo.GetActiveCanbo
                    //.Where(p => p.inttrangthai == (int)enumcanbo.inttrangthai.IsActive)
                            .Where(p => p.strpassword == strPassMD5)
                            .Where(p => p.strusername == strusername)
                            .FirstOrDefault();
                if (user != null)
                {
                    // set realuserid 1 lan duy nhat khi bat dau login
                    //_session.InsertObject(AppConts.SessionRealUserId, 0);

                    // chi duoc phep insert RealUserId vao session
                    // mot lan duy nhat khi bat dau login
                    _session.InsertObject(AppConts.SessionRealUserId, user.intid);

                    _StartLogin(user);

                    return user.intid;
                }
                else
                {
                    string status = "Username: " + strusername + " Đăng nhập không thành công";
                    _logger.Warn(status);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return 0;
            }

        }

        /// <summary>
        /// set ten don vi sau khi login
        /// </summary>
        /// <param name="strtendonvi"></param>
        public void SetTenDonvi(string strtendonvi)
        {
            //string strten = GetTenDonvi(strtendonvi);
            _donviRepo.SetTen(strtendonvi, 1);
        }


        public ResultFunction ChangePassword(ChangePasswordViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            string strenoldpassword = CryptServices.HashMD5(model.OldPassword);
            int iduser = Convert.ToInt32(_session.GetObject(AppConts.SessionUserId));
            var canbo = _canboRepo.GetActiveCanbo
                        .Where(p => p.intid == iduser)
                        .Where(p => p.strpassword == strenoldpassword)
                        .FirstOrDefault();
            if (canbo != null)
            {
                string strennewpassword = CryptServices.HashMD5(model.NewPassword);
                string strconfirmPass = CryptServices.HashMD5(model.ConfirmPassword);
                if (strennewpassword == strconfirmPass)
                {
                    _canboRepo.ResetPassword(iduser, strennewpassword);
                    _logger.Info("Đổi mật khẩu thành công");
                    kq.id = (int)ResultViewModels.Success;
                }
                else
                {
                    kq.id = (int)ResultViewModels.Error;
                    kq.message = "Mật khẩu xác nhận không đúng";
                }
            }
            else
            {
                kq.id = (int)ResultViewModels.Error;
                kq.message = "Không tìm thấy cán bộ";
            }
            return kq;
        }

        public HomeViewModel GetHomePage(int idcanbo)
        {
            var Opt = _GetTuychoncanbo(idcanbo, ThamsoTuychon.HomePage);
            try
            {
                int idmenu = Convert.ToInt32(Opt);
                var home = _menuRepo.Menus.FirstOrDefault(p => p.Id == idmenu);
                HomeViewModel modelHome = new HomeViewModel
                {
                    straction = home.straction,
                    strcontroller = home.strcontroller
                };
                return modelHome;
            }
            catch
            {
                HomeViewModel modelHome = new HomeViewModel
                {
                    straction = "Index",
                    strcontroller = "Vanbanden"
                };
                return modelHome;
            }
        }



        public void GetUyquyen(int idcanbo)
        {
            // quan trong nhat: kiem tra quyen user
            int UserId = _session.GetUserId();

            int RealUserId = _session.GetRealUserId();

            if (RealUserId == idcanbo)
            {   // quay ve quyen cua minh
                var user = _canboRepo.GetActiveCanbo.FirstOrDefault(p => p.intid == idcanbo);
                if (user != null)
                {
                    _StartLogin(user);
                }
            }
            else
            {   // lay quyen cua user khac nen 
                // kiem tra user nay co duoc uy quyen khong
                var uyquyen = _uyquyenRepo.UyQuyens
                            .Where(p => p.intPersRec == RealUserId)
                            .Where(p => p.intPersSend == idcanbo)
                            .FirstOrDefault();
                if (uyquyen != null)
                {
                    var user = _canboRepo.GetActiveCanbo.FirstOrDefault(p => p.intid == idcanbo);
                    if (user != null)
                    {
                        _StartLogin(user);
                    }
                }
                else
                {
                    string strcurrentuser = _canboRepo.GetActiveCanboByID(UserId).strhoten;
                    string struyquyenuser = _canboRepo.GetActiveCanboByID(idcanbo).strhoten;
                    _logger.Warn("User: " + strcurrentuser + " không được quyền đăng nhập ủy quyền của User: " + struyquyenuser);
                }
            }
        }

        /// <summary>
        /// lay ds cac user dc uy quyen
        /// </summary>
        /// <returns></returns>
        public ListUserUyquyenViewModel GetListUserUyquyen()
        {
            var idcanbo = _session.GetUserId();

            int RealUserId = Convert.ToInt32(_session.GetObject(AppConts.SessionRealUserId));

            ListUserUyquyenViewModel user = new ListUserUyquyenViewModel();

            user.AllUser = _canboRepo.GetActiveCanbo
                        .Where(p => p.intid != idcanbo)
                        .Select(p => new UserUyquyenViewModel
                        {
                            intid = p.intid,
                            isRealUser = (p.intid == RealUserId) ? true : false,
                            strhoten = p.strhoten,
                            strkyhieu = p.strkyhieu
                        })
                        .OrderBy(p => p.strkyhieu)
                        .ThenBy(p => p.strhoten)
                        ;
            user.UyquyenUser = _uyquyenRepo.UyQuyens
                        .Where(p => p.intPersSend == idcanbo)
                        .Join(
                            _canboRepo.GetActiveCanbo,
                            u => u.intPersRec,
                            c => c.intid,
                            (u, c) => new { u, c }
                        )
                        .Select(p => new UserUyquyenViewModel
                        {
                            intid = p.c.intid,
                            //isRealUser = (p.intid == RealUserId) ? true : false,
                            strhoten = p.c.strhoten,
                            strkyhieu = p.c.strkyhieu
                        })
                        .OrderBy(p => p.strkyhieu)
                        .ThenBy(p => p.strhoten)
                        ;

            return user;
        }

        public ResultFunction SaveUyquyen(string strAddIdUser)
        {
            ResultFunction kq = new ResultFunction();
            // kiem tra cac gia tri id duoc uy quyen
            int idcanboSend = Convert.ToInt32(_session.GetObject(AppConts.SessionUserId));

            //int RealUserId = Convert.ToInt32(SessionService.GetObject(AppConts.SessionRealUserId));

            if (string.IsNullOrWhiteSpace(strAddIdUser))
            {   //xoa toan bo cac uy quyen
                _uyquyenRepo.Xoa(idcanboSend);
                kq.id = (int)ResultViewModels.Success;
            }
            else
            {   // thay doi uy quyen
                //string strChoseId = "";
                int len = strAddIdUser.Length - 1;
                strAddIdUser = strAddIdUser.Substring(0, len);
                string[] strIdUser = strAddIdUser.Split(new Char[] { ';' });

                string strlistUsername = "";

                try
                {
                    // xoa toan bo uy quyen truoc khi them moi
                    _uyquyenRepo.Xoa(idcanboSend);

                    foreach (var a in strIdUser)
                    {
                        int idcanboRec = Convert.ToInt32(a);
                        string strhoten = _canboRepo.GetActiveCanbo.FirstOrDefault(p => p.intid == idcanboRec).strhoten;
                        strlistUsername += strhoten;
                        _uyquyenRepo.Them(idcanboSend, idcanboRec);
                    }
                    _logger.Info("Ủy quyền thành công cho các user: " + strlistUsername);
                    kq.id = (int)ResultViewModels.Success;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    kq.id = (int)ResultViewModels.Success;
                    kq.message = "Lỗi Không ủy quyền được!!!";
                }
            }

            return kq;
        }


        /// <summary>
        /// lay ten cua don vi da ma hoa
        /// </summary>
        /// <param name="strentext"></param>
        /// <returns></returns>
        public string GetTenDonvi(string strentext)
        {
            try
            {
                string _m = "tttH_qlvb@820704";
                string strten = CryptServices.DeCryptString_AES(_m, strentext).ToUpper();
                //strten = strentext;
                return strten;

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return "Lỗi Tên đơn vị";
            }
        }

        /// <summary>
        /// lay ten don vi chua ma hoa
        /// </summary>
        /// <returns></returns>
        public string GetTenDonvi()
        {
            string strten = QLVB.Donvi.Donvi.GetTenDonVi().ToUpper();

            //string encode = CryptServices.EncryptText(strten, "123");
            //string decode = CryptServices.DecryptText(encode, "123");
            //string _m = "tttH_qlvb@820704";
            //string encode = CryptServices.EncryptStringAES(_m, strten);
            //string decode = CryptServices.DecryptStringAES(encode, _m);

            return strten;
        }

        /// <summary>
        /// kiem tra xem co su dung module ykcd khong
        /// </summary>
        public void CheckYKCD()
        {
            try
            {
                bool isykcd = QLVB.Donvi.Donvi.IsYKCD();
                if (isykcd)
                {
                    _menuRepo.UpdateYkcd(1);
                    _quyenRepo.UpdateYKCD(1);
                }
                else
                {
                    _menuRepo.UpdateYkcd(0);
                    _quyenRepo.UpdateYKCD(0);
                }
            }
            catch
            {
                //_logger.Error(ex.Message);
            }
        }

        public IList<string> GetCanBoByUserName(string strUsername)
        {
            var results = new List<string>();

            var cb = _canboRepo.GetAllCanboByUserName(strUsername);

            if (cb == null) return null;

            results.Add(cb.strusername);
            results.Add(cb.strhoten);
            results.Add(cb.stremail);

            results.Add(string.Format("{0}",cb.intnhomquyen));

            return results;
        }


        #endregion Interface Implementation

        #region Option

        public OptionViewModel GetOption()
        {
            int iduser = _session.GetUserId();
            Dictionary<int, string> listOption = _canboRepo.GetListOption();
            OptionViewModel model = new OptionViewModel();

            model.strImageProfile = _canboRepo.GetActiveCanboByID(iduser).strImageProfile;

            List<OptionUserViewModel> listUserOption = new List<OptionUserViewModel>();
            string strRightUser = _canboRepo.GetUserOption(iduser);

            foreach (var option in listOption)
            {
                var userOption = new OptionUserViewModel();
                userOption.intVitri = option.Key;
                userOption.strmota = option.Value;
                userOption.IsValue = _canboRepo.IsOption(strRightUser, option.Key);

                listUserOption.Add(userOption);
            }
            model.ListOption = listUserOption;

            return model;
        }

        public int SaveOption(OptionViewModel model)
        {
            int iduser = _session.GetUserId();
            string strRightUser = _canboRepo.GetUserOption(iduser);

            string strNewOption = string.Empty;
            string strNewRightUser = string.Empty;

            var arrRight = strRightUser.ToList();

            for (int i = 0; i < strRightUser.Length; i++)
            {
                bool found = false;
                strNewOption = string.Empty;
                foreach (var Opt_user in model.ListOption)
                {
                    if (Opt_user.intVitri == i)
                    {   //i = (int)enumcanbo.strRight.PhanXLNhieuVB;
                        found = true;
                        strNewOption += Opt_user.IsValue ? "1" : "0";
                    }
                }
                if (found)
                {   // tim thay
                    strNewRightUser += strNewOption;
                }
                else
                {   // giu nguyen gia tri 
                    strNewRightUser += arrRight[i].ToString();
                }
            }

            int kq = (int)ResultViewModels.Error;
            try
            {
                _canboRepo.UpdateOption(iduser, strNewRightUser);
                kq = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return kq;
        }

        private IEnumerable<TuychonCanboViewModel> GetTuychonCanbo()
        {
            int iduser = _session.GetUserId();
            var model = _tuychonRepo.Tuychons
                .GroupJoin(
                    _tuychonCanboRepo.TuychonCanbos.Where(p => p.intidcanbo == iduser),
                    t => t.intid,
                    c => c.intidoption,
                    (t, c) => new { t, c }
                )
                .Select(p => new TuychonCanboViewModel
                {
                    intid = p.c.FirstOrDefault().intid,
                    intgroup = p.t.intgroup,
                    intorder = p.t.intorder,
                    strmota = p.t.strmota,
                    strgiatri = p.c.FirstOrDefault().strgiatri,
                    strgiatridefault = p.t.strgiatri,
                    strthamso = p.t.strthamso
                }).ToList();
            return model;

        }

        private IEnumerable<TuychonSelectListVM> GetTuychonMenuType()
        {
            List<TuychonSelectListVM> listopt = new List<TuychonSelectListVM>();
            TuychonSelectListVM opt = new TuychonSelectListVM();
            opt.strgiatri = "sidebar-left-mini";
            opt.strmota = "Menu mini";
            opt.isSelect = false;
            listopt.Add(opt);

            opt = new TuychonSelectListVM();
            opt.strgiatri = "none";
            opt.strmota = "Menu đầy đủ";
            opt.isSelect = false;
            listopt.Add(opt);

            return listopt;
        }

        private IEnumerable<TuychonSelectListVM> GetTuychonHomePage()
        {
            List<TuychonSelectListVM> listopt = new List<TuychonSelectListVM>();
            //TuychonSelectListVM opt = new TuychonSelectListVM();
            //opt.strgiatri = "Vanbanden";
            //opt.strmota = "Văn bản đến";
            //listopt.Add(opt);

            //opt = new TuychonSelectListVM();
            //opt.strgiatri = "Vanbandi";
            //opt.strmota = "Văn bản đi";
            //listopt.Add(opt);

            //opt = new TuychonSelectListVM();
            //opt.strgiatri = "Tinhhinhxuly";
            //opt.strmota = "Tình hình xử lý Văn bản đến";
            //listopt.Add(opt);

            int iduser = _session.GetUserId();
            var submenu = _menuManager.GetSubMenu(iduser).Where(p => p.ParentId > 0);

            foreach (var p in submenu)
            {
                TuychonSelectListVM opt = new TuychonSelectListVM();
                opt.strgiatri = p.Id.ToString();
                opt.strmota = p.strmota;
                opt.isSelect = false;
                listopt.Add(opt);
            }

            return listopt.OrderBy(p => p.strmota);
        }

        public TuychonViewModel GetTuychon()
        {
            TuychonViewModel model = new TuychonViewModel();
            model.Tuychon = GetTuychonCanbo();
            model.MenuType = GetTuychonMenuType();
            model.HomePage = GetTuychonHomePage();
            return model;
        }

        public ResultFunction SaveTuychon(Dictionary<string, string> Options)
        {
            ResultFunction kq = new ResultFunction();

            foreach (var p in Options)
            {
                if ((!string.IsNullOrEmpty(p.Value)) && (!string.IsNullOrEmpty(p.Key)))
                {
                    try
                    {
                        int idcanbo = _session.GetUserId();
                        kq.id = _tuychonCanboRepo.Save(p.Key, p.Value, idcanbo);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                    }

                }
            }
            return kq;
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

        #endregion Option


        #region Private Methods

        /// <summary>
        /// luu cac session va lay quyen cua user de dang nhap
        /// </summary>
        /// <param name="user"></param>
        private void _StartLogin(Canbo user)
        {
            try
            {
                {   // khoi tao cac gia tri session mac dinh 
                    _session.SessionStart();

                    _session.InsertObject(AppConts.SessionUserName, user.strhoten);
                    _session.InsertObject(AppConts.SessionUserId, user.intid);

                }
                // cap nhat quyen cua user trong session
                //RoleServices.GetUserRole();

                //Session["UsersOnline"] = (int)Session["UsersOnline"] + 1;

                // set ten don vi theo dll donvi
                _donviRepo.SetTen(Donvi.Donvi.GetTenDonVi(), 1);
                // set ma dinh danh 
                _configRepo.SaveConfig(ThamsoHethong.MaDinhDanh, Donvi.Donvi.GetMaDinhDanh());


                int idcanbo = _session.GetUserId();
                int RealIdcanbo = _session.GetRealUserId();

                if ((idcanbo != RealIdcanbo) && (RealIdcanbo > 0))
                {
                    string strhotenRealId = _canboRepo.GetActiveCanboByID(RealIdcanbo).strhoten;
                    //_canboRepo.GetCanbo.FirstOrDefault(p => p.intid == RealIdcanbo).strhoten;
                    _logger.Info("Đăng nhập ủy quyền thành công, cán bộ được ủy quyền: " + strhotenRealId);
                }
                else
                {
                    _logger.Info("Đăng nhập thành công");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }




        #endregion Private Methods
    }
}
