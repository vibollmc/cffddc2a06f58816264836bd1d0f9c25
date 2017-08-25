using QLVB.Common.Crypt;
using QLVB.Common.Logging;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Donvi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLVB.Core.Implementation
{
    public class DonviManager : IDonviManager
    {
        #region Constructor

        private ILogger _logger;
        private IDonvitructhuocRepository _donviRepo;
        private ICanboRepository _canboRepo;

        //private ICanboDonviRepository _canbodonviRepo;
        private IChucdanhRepository _chucdanhRepo;

        private INhomQuyenRepository _nhomquyenRepo;

        public DonviManager(IDonvitructhuocRepository donviRepo,
                                ICanboRepository canboRepo,
            //ICanboDonviRepository canbodonviRepo,
                                IChucdanhRepository chucdanhRepo,
                                INhomQuyenRepository nhomquyenRepo,
                                ILogger logger)
        {
            _donviRepo = donviRepo;
            _canboRepo = canboRepo;
            //_canbodonviRepo = canbodonviRepo;
            _chucdanhRepo = chucdanhRepo;
            _nhomquyenRepo = nhomquyenRepo;
            _logger = logger;
        }

        #endregion Constructor

        #region Interface Implementation

        public IList<Donvitructhuoc> GetRootDonvi()
        {
            var cacdonvi = _donviRepo.Donvitructhuocs
                        .Where(p => p.inttrangthai == (int)enumDonvitructhuoc.inttrangthai.IsActive)
                        .OrderBy(p => p.strtendonvi)
                        .ToList();
            var donvicon = cacdonvi.Where(p => p.ParentId == null).OrderBy(p => p.strtendonvi).ToList();

            return donvicon;
        }

        public IEnumerable<ListUserViewModel> GetListUsers(int iddonvi)
        {
            var users = _canboRepo.GetActiveCanbo.Where(p => p.intdonvi == iddonvi)
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
                    .Select(p => new ListUserViewModel
                    {
                        intid = p.u2.u1.intid,
                        strhoten = p.u2.u1.strhoten,
                        strkyhieu = p.u2.u1.strkyhieu,
                        strmacanbo = p.u2.u1.strmacanbo,
                        strusername = p.u2.u1.strusername,
                        strchucdanh = p.u2.strtenchucdanh,
                        strtennhomquyen = p.strtennhom
                    })
                    .OrderBy(p => p.strkyhieu)
                    .ThenBy(p => p.strhoten);
            return users;
        }

        public EditDonviViewModel GetDonvi(int id)
        {
            var dv = _donviRepo.Donvitructhuocs.Where(p => p.Id == id)
                .Select(p => new EditDonviViewModel
                {
                    intid = p.Id,
                    strtendonvi = p.strtendonvi
                }).First();
            return dv;
        }

        public int SaveDonvi(EditDonviViewModel model)
        {
            try
            {
                if (model.intType == 0)
                {   // add new
                    // them node con vao id don vi dang chon
                    _donviRepo.AddTen(model.strtendonvi, model.intid);
                    _logger.Info("Thêm mới đơn vị trực thuộc: " + model.strtendonvi);
                }
                else
                {   // cap nhat
                    // khong cho phep cap nhat iddonvi=1
                    if (model.intid != 1)
                    {
                        _donviRepo.SetTen(model.strtendonvi, model.intid);
                        _logger.Info("Cập nhật đơn vị trực thuộc: " + model.strtendonvi + ", id: " + model.intid);
                    }
                    else
                    {
                        _logger.Info("Không thể cập nhật tên đơn vị gốc: " + model.strtendonvi);
                    }
                }
                return (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return (int)ResultViewModels.Error;
            }
        }

        public int DeleteDonvi(DeleteDonviViewModel model)
        {
            try
            {
                // kiem tra dieu kien xem co xoa don vi duoc khong
                // co user va don vi con truc thuoc khong???
                var donvicon = _donviRepo.Donvitructhuocs
                                .Where(p => p.ParentId == model.intid);
                if (donvicon.Count() == 0)
                {
                    var user = _canboRepo.GetActiveCanbo.Where(p => p.intdonvi == model.intid);
                    if (user.Count() == 0)
                    {
                        _donviRepo.DeleteDonvi(model.intid);
                        _logger.Info("Xóa đơn vị trực thuộc: " + model.strtendonvi);
                        return (int)ResultViewModels.Success;
                    }
                    else
                    {
                        _logger.Info("Không thể xóa đơn vị: " + model.strtendonvi);
                        return (int)ResultViewModels.ErrorForeignKey;
                    }
                }
                else
                {
                    _logger.Info("Không thể xóa đơn vị: " + model.strtendonvi);
                    return (int)ResultViewModels.ErrorForeignKey;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return (int)ResultViewModels.Error;
            }
        }

        public EditUserViewModel GetUser(int iduser, int iddonvi)
        {
            // khoi tao user moi
            EditUserViewModel user = new EditUserViewModel();
            user.intid = iduser;
            user.iddonvi = iddonvi;
            user.Listchucdanh = _chucdanhRepo.Chucdanhs.OrderBy(p => p.strtenchucdanh);
            user.Listnhomquyen = _nhomquyenRepo.GetActiveNhomQuyens.OrderBy(p => p.strtennhom);
            user.enumgioitinh = enumcanbo.intgioitinh.Nam;

            if (iduser != 0)
            {   // neu cap nhat thi load thong tin user
                var cb = _canboRepo.GetActiveCanboByID(iduser);
                user.iddonvi = cb.intdonvi;
                user.intchucvu = cb.intchucvu;
                user.intkivb = cb.intkivb == (int)enumcanbo.intkivb.Co ? true : false;
                user.intnhomquyen = cb.intnhomquyen;

                //user.IsNguoiXL = cb.intnguoixuly == (int)enumcanbo.intnguoixuly.IsActive ? true : false;
                if (cb.intnguoixuly == (int)enumcanbo.intnguoixuly.IsDefault)
                {
                    user.IsNguoiXL = true;
                    user.IsDefaultXLBD = true;
                }
                else
                {
                    user.IsNguoiXL = cb.intnguoixuly == (int)enumcanbo.intnguoixuly.IsActive ? true : false;
                    user.IsDefaultXLBD = false;
                }

                user.strdienthoai = cb.strdienthoai;
                user.stremail = cb.stremail;
                user.strhoten = cb.strhoten;
                user.strkyhieu = cb.strkyhieu;
                user.strmacanbo = cb.strmacanbo;
                user.strngaysinh = cb.strngaysinh;
                user.strpassword = cb.strpassword;
                user.strusername = cb.strusername;
                user.enumgioitinh = cb.intgioitinh == (int)enumcanbo.intgioitinh.Nam ? enumcanbo.intgioitinh.Nam : enumcanbo.intgioitinh.Nu;
                user.strImageProfile = cb.strImageProfile;

            }
            return user;
        }

        public ResultFunction SaveUser(EditUserViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                Canbo cb = _GetUserFromModel(model);
                if (model.intid == 0)
                {
                    // them moi can bo
                    // kiem tra co username nay chua
                    var checkuser = CheckUsername(model.strusername);

                    if ((checkuser.id == 0) // chua co trong database
                        || (checkuser.id == 2)) // chua co trong database va da co trong ldap
                    {
                        model.intid = _canboRepo.Them(cb);
                        _logger.Info("Thêm mới cán bộ: " + model.strusername + ", id: " + model.intid.ToString());
                        kq.id = (int)ResultViewModels.Success;
                    }
                    else
                    {
                        if (checkuser.id == 1) // da co trong database
                        {
                            kq.id = (int)ResultViewModels.Error;
                            kq.message = "Đã có tên đăng nhập: " + model.strusername + " trong hệ thống. Vui lòng chọn tên khác";
                        }
                        if (checkuser.id == 3)  // chua co trong datase va chua co trong ldap
                        {
                            kq.id = (int)ResultViewModels.Error;
                            kq.message = "Chưa có tên đăng nhập: " + model.strusername + " trong LDAP Server. Vui lòng chọn tên khác";
                        }
                    }
                }
                else
                {
                    // cap nhat can bo
                    _canboRepo.Sua(model.intid, cb);
                    if (!string.IsNullOrEmpty(cb.strpassword))
                    {
                        _canboRepo.ResetPassword(model.intid, cb.strpassword);
                    }
                    _logger.Info("Cập nhật cán bộ: " + cb.strhoten + ", id: " + model.intid.ToString());
                    kq.id = (int)ResultViewModels.Success;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
                kq.message = "Lỗi: " + ex.Message;
            }
            return kq;
        }

        /// <summary>
        /// Kiem tra username da co trong database va Ldap chua
        /// </summary>
        /// <param name="strusername"></param>
        /// <returns> kq.id:
        /// 0: chưa có trong database
        /// 1: đã có trong database
        /// 2:(ldap): chưa có trong database và đã có trong ldap
        /// 3:(ldap): chưa có trong database và chưa có trong ldap
        /// </returns>
        public ResultFunction CheckUsername(string strusername)
        {
            ResultFunction kq = new ResultFunction();

            var cb = _canboRepo.GetAllCanbo.FirstOrDefault(p => p.strusername == strusername);
            if (cb != null)
            {   // da co user name nay trong database
                kq.id = 1;
                kq.message = "Tên đăng nhập đã được sử dụng";
            }
            else
            {   // chua co trong database

                // kiem tra tren ldap
                bool isLdap = QLVB.Common.Utilities.AppSettings.IsLDAP;
                if (isLdap)
                {
                    var ldapUser = QLVB.Common.LDAP.LDAPServices.CheckUserNameLDAP(strusername);
                    if (ldapUser)
                    {   // da co trong ldap
                        kq.id = 2;
                        kq.message = string.Empty; //"Tên đăng nhập đã có trong LDAP";
                    }
                    else
                    {   // chua co trong ldap
                        kq.id = 3;
                        kq.message = "Tên đăng nhập chưa có trong LDAP";
                    }
                }
                else
                {
                    kq.id = 0;
                    kq.message = string.Empty; //"Tên đăng nhập chưa được sử dụng";
                }
            }
            return kq;
        }

        public ResultFunction DeleteUser(int iduser)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                string strhoten = _canboRepo.Xoa(iduser);
                _logger.Info("Xóa user: " + strhoten + ", id: " + iduser.ToString());
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
                kq.message = "Lỗi không xóa được !!!";
            }
            return kq;
        }

        public MoveUserViewModel GetMoveUser(int idsource, int iddest)
        {
            MoveUserViewModel model = new MoveUserViewModel();

            // lay danh sach cac phong ban truc thuoc
            model.cacdonvi = _donviRepo.Donvitructhuocs
                //.Where(p => p.Id != idsource)
                        .OrderBy(p => p.intlevel)
                        .ThenBy(p => p.ParentId)
                        .ThenBy(p => p.strtendonvi);
            // id cua don vi sap chuyen toi
            model.iddest = iddest;

            // danh sach can bo cua don vi sap chuyen toi
            model.canbodest = _canboRepo.GetActiveCanbo
                        .Where(p => p.intdonvi == iddest)
                        .OrderBy(p => p.strkyhieu)
                        .ThenBy(p => p.strhoten);

            model.maxLevelDonvi = _donviRepo.Donvitructhuocs
                        .Where(p => p.inttrangthai == (int)enumDonvitructhuoc.inttrangthai.IsActive)
                        .Max(p => p.intlevel);

            // ten don vi dang chuyen can bo
            model.strtendonvi = _donviRepo.Donvitructhuocs
                            .FirstOrDefault(p => p.Id == idsource).strtendonvi;

            // danh sach can bo thuoc don vi dang xet
            model.canbosource = _canboRepo.GetActiveCanbo
                        .Where(p => p.intdonvi == idsource)
                        .OrderBy(p => p.strkyhieu)
                        .ThenBy(p => p.strhoten);

            return model;
        }

        public ResultFunction UpdateMoveUser(string strlistiduser, int iddonvi)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                string[] split = strlistiduser.Split(new Char[] { ';' });
                foreach (var s in split)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        int iduser = Convert.ToInt32(s);
                        _canboRepo.UpdateDonvi(iduser, iddonvi);
                    }
                }
                _logger.Info("Cập nhật đơn vị trực thuộc");
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
                kq.message = "Lỗi không cập nhật được !!!";
            }
            return kq;
        }

        #endregion Interface Implementation

        #region Private Methods

        /// <summary>
        /// lay thong tin chung tu view model luu vao bang canbo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Canbo _GetUserFromModel(EditUserViewModel model)
        {
            Canbo cb = new Canbo();
            cb.intchucvu = model.intchucvu;
            cb.intdonvi = model.iddonvi;
            cb.intgioitinh = model.enumgioitinh == enumcanbo.intgioitinh.Nam ? (int)enumcanbo.intgioitinh.Nam : (int)enumcanbo.intgioitinh.Nu;
            //cb.intid / bo truong intid
            cb.intkivb = model.intkivb == true ? (int)enumcanbo.intkivb.Co : (int)enumcanbo.intkivb.Khong;

            cb.intnguoixuly = model.IsNguoiXL == true ? (int)enumcanbo.intnguoixuly.IsActive : (int)enumcanbo.intnguoixuly.NotActive;
            if (cb.intnguoixuly == (int)enumcanbo.intnguoixuly.IsActive)
            {
                if (model.IsDefaultXLBD == true)
                {
                    cb.intnguoixuly = (int)enumcanbo.intnguoixuly.IsDefault;
                }
            }

            cb.intnhomquyen = model.intnhomquyen;
            //cb.inttrangthai  / default la active
            cb.strdienthoai = model.strdienthoai;
            cb.stremail = model.stremail;
            cb.strhoten = model.strhoten;
            cb.strkyhieu = model.strkyhieu;
            cb.strmacanbo = model.strmacanbo;
            cb.strngaysinh = model.strngaysinh;

            cb.strusername = model.strusername;

            if (!string.IsNullOrEmpty(model.strpassword))
            {
                if (model.strpassword == model.ConfirmPassword)
                {
                    cb.strpassword = CryptServices.HashMD5(model.strpassword);
                }
            }

            return cb;
        }

        #endregion Private Methods
    }
}