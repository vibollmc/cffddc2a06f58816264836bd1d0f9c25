using QLVB.Common.Sessions;
using QLVB.Common.Utilities;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLVB.Core.Implementation
{
    public class RoleManager : IRoleManager
    {
        #region Constructor

        private ISessionServices _session;
        private ICanboRepository _canboRepo;
        private IQuyenRepository _quyenRepo;
        private IQuyenNhomQuyenRepository _quyennhomRepo;
        private IHosovanbanRepository _hsvanbanRepo;
        private IHosocongviecRepository _hscongviecRepo;
        private IVanbandenRepository _vbdenRepo;
        private IVanbandiRepository _vbdiRepo;
        private IAttachVanbanRepository _filevanbanRepo;
        private IDoituongxulyRepository _dtxulyRepo;
        private IVanbandenCanboRepository _vbdencanboRepo;
        private IVanbandiCanboRepository _vbdicanboRepo;

        public RoleManager(ICanboRepository canboRepo, IQuyenRepository quyenRepo, IQuyenNhomQuyenRepository quyennhomRepo,
            IHosovanbanRepository hosovbRepo, IHosocongviecRepository hosocvRepo,
            IVanbandenRepository vbdenRepo, IVanbandiRepository vbdiRepo,
            IAttachVanbanRepository filevanbanRepo, IDoituongxulyRepository dtxulyRepo,
            IVanbandenCanboRepository vbdencanboRepo, IVanbandiCanboRepository vbdicanboRepo,
            ISessionServices session)
        {
            _canboRepo = canboRepo;
            _quyenRepo = quyenRepo;
            _quyennhomRepo = quyennhomRepo;
            _hsvanbanRepo = hosovbRepo;
            _hscongviecRepo = hosocvRepo;
            _vbdenRepo = vbdenRepo;
            _vbdiRepo = vbdiRepo;
            _filevanbanRepo = filevanbanRepo;
            _dtxulyRepo = dtxulyRepo;
            _vbdencanboRepo = vbdencanboRepo;
            _vbdicanboRepo = vbdicanboRepo;
            _session = session;
        }

        #endregion Constructor

        #region GetRole

        public object[] GetUserRole()
        {
            Int32 userId = _session.GetUserId();
            var userRoles = GetRole(userId);
            return userRoles;
        }

        private object[] GetRole(int iduser)
        {
            int? idnhomquyen = _canboRepo.GetActiveCanbo
                                .FirstOrDefault(p => p.intid == iduser)
                                .intnhomquyen;

            var roles = _quyenRepo.Quyens
                        .Join(
                            _quyennhomRepo.QuyenNhomQuyens.Where(p => p.intidnhomquyen == idnhomquyen),
                            quyen => quyen.intid,
                            nhom => nhom.intidquyen,
                            (quyen, nhom) => quyen
                        ).Select(p => p.strquyen);

            List<object> userRoles = new List<object>();
            string Roles = "";
            foreach (var p in roles)
            {
                if (!userRoles.Contains(p))
                {
                    userRoles.Add(p);
                    Roles = Roles + p + ";";
                }
            }
            _session.InsertObject(AppConts.SessionUserRoles, Roles);

            return userRoles.ToArray();
        }

        public bool IsRole(string strRole)
        {
            bool flag = false;
            string userRoles = GetUserRolesInSession();
            string[] role = userRoles.Split(new Char[] { ';' });
            foreach (var p in role)
            {
                if (p == strRole) flag = true;
            }
            return flag;
        }

        /// <summary>
        /// Gets the user role in session.
        /// </summary>
        /// <returns>role in string</returns>
        private string GetUserRolesInSession()
        {
            var roles = _session.GetObject(AppConts.SessionUserRoles);
            return roles != null ? roles.ToString() : string.Empty;
            //return _session.GetObject(AppConts.SessionUserRoles).ToString();
        }

        #endregion GetRole

        #region Vanban

        /// <summary>
        /// kiem tra xem user co quyen xem van ban den khong
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        public bool IsViewVanbanden(int idvanban, int idcanbo)
        {
            // user duoc quyen xem van ban do
            // hoac user co quyen xem tat ca cac van ban
            bool flag = false;
            bool isViewAllvb = IsRole(RoleVanbanden.Xemtatcavb);
            if (isViewAllvb == false)
            {
                var vanban = _hsvanbanRepo.Hosovanbans //context.Hosovanbans.AsNoTracking()
                        .Where(p => p.intidvanban == idvanban)
                        .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                        .Join(
                    //context.Doituongxulys.AsNoTracking()
                            _dtxulyRepo.Doituongxulys
                                .Where(p => p.intidcanbo == idcanbo),
                            hs => hs.intidhosocongviec,
                            dt => dt.intidhosocongviec,
                            (hs, dt) => new { hs.intidvanban }
                        )
                        ;

                if (vanban.Count() != 0)
                {   // user co quyen xu ly
                    flag = true;
                }
                else
                {   // user khong co quyen xu ly
                    // kiem tra quyen xem vb va vb public
                    var vbpublic = _vbdenRepo.Vanbandens.Where(p => p.intid == idvanban)
                        .GroupJoin(
                            _vbdencanboRepo.VanbandenCanbos.Where(p => p.intidcanbo == idcanbo),
                            v => v.intid,
                            c => c.intidvanban,
                            (v, c) => new { v, c }
                        )
                        .Select(p => p.v.intid);
                    if (vbpublic.Count() != 0) { flag = true; }
                }
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// kiem tra xem user co quyen download file dinh kem cua van ban den khong
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="idvanban"></param>
        /// <param name="idcanbo"></param>
        /// <returns></returns>
        public bool IsDownloadFileVanbanden(int idfile, int idcanbo, int idvanban)
        {
            bool flag = false;
            // giong voi isviewvanbanden
            flag = IsViewVanbanden(idvanban, idcanbo);
            return flag;
        }

        /// <summary>
        /// kiem tra xem user co quyen xem van ban di khong
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        public bool IsViewVanbandi(int idvanban, int idcanbo)
        {
            // user duoc quyen xem van ban do
            // hoac user co quyen xem tat ca cac van ban
            bool flag = false;
            bool isViewAllvb = IsRole(RoleVanbandi.Xemtatcavb);
            if (isViewAllvb == false)
            {
                var vanban = _vbdiRepo.Vanbandis.Where(p => p.intid == idvanban)
                    .GroupJoin(
                        _vbdicanboRepo.VanbandiCanbos.Where(p => p.intidcanbo == idcanbo),
                        v => v.intid,
                        c => c.intidvanban,
                        (v, c) => new { v, c }
                    )
                    .Select(p => p.v.intid)
                    ;

                if (vanban.Count() != 0)
                {   // user co quyen xem vb
                    flag = true;
                }
                else
                {
                    // user khong co quyen xu ly
                    // kiem tra vb public
                    var vbpublic = _vbdiRepo.Vanbandis.Where(p => p.intid == idvanban)
                        .Where(p => p.intpublic == (int)enumVanbanden.intpublic.Public)
                        .Select(p => p.intid);
                    if (vbpublic.Count() != 0) { flag = true; }
                }
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// kiem tra xem user co quyen download file dinh kem cua van ban di khong
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="idvanban"></param>
        /// <param name="idcanbo"></param>
        /// <returns></returns>
        public bool IsDownloadFileVanbandi(int idfile, int idcanbo, int idvanban)
        {
            bool flag = false;
            flag = IsViewVanbandi(idvanban, idcanbo);
            return flag;
        }

        #endregion Vanban

        #region Hosocongviec

        /// <summary>
        /// kiem tra xem user co quyen xem thong tin ho so khong
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="idcanbo"></param>
        /// <returns></returns>
        public bool IsViewHosocongviec(int idhosocongviec, int idcanbo)
        {
            bool flag = false;
            bool isViewAllvb = IsRole(RoleVanbanden.Xemtatcavb);
            if (isViewAllvb == false)
            {
                var hoso = _dtxulyRepo.Doituongxulys
                            .Where(p => p.intidhosocongviec == idhosocongviec)
                            .Where(p => p.intidcanbo == idcanbo);

                if (hoso.Count() != 0)
                {
                    flag = true;
                }
                else
                {
                    // user khong co quyen xu ly
                    // kiem tra quyen xem vb va vb public
                    int idvanban = _hsvanbanRepo.Hosovanbans.Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                            .Where(p => p.intidhosocongviec == idhosocongviec)
                            .FirstOrDefault().intidvanban;

                    var vbxem = _vbdencanboRepo.VanbandenCanbos
                           .Where(p => p.intidcanbo == idcanbo);

                    var vbpublic = _vbdenRepo.Vanbandens.Where(p => p.intid == idvanban)
                            .Where(p => p.intpublic == (int)enumVanbanden.intpublic.Public
                                    || vbxem.Any(c => c.intidvanban == p.intid)
                            );
                    if (vbpublic.Count() != 0) { flag = true; }
                }
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// kiem tra xem user co quyen xu ly ho so khong
        /// user: ldgv, ldpt, xlc, phxl va DA CHUYEN XU LY
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="idcanbo"></param>
        /// <returns>true: neu co quyen xu ly</returns>
        public bool IsXulyHosocongviec(int idhosocongviec, int idcanbo)
        {
            // user phai co ten trong danh sach doi tuong xu ly
            // user: ldgv, ldpt, xlc, phxl  va chuyen xu ly
            // giong nhu trong IDoituongxulyRepository.getAllcanboxulys

            bool flag = false;
            var dt = _dtxulyRepo.Doituongxulys
                        .Where(p => p.intidhosocongviec == idhosocongviec)
                        .Where(p => p.intidcanbo == idcanbo)
                        .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec
                                || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach
                                || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh
                                || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Phoihopxuly
                                || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Chuyenxuly)
                        .Select(p => p.intid);

            if (dt.Count() != 0)
            {
                flag = true;
            }

            return flag;
        }

        /// <summary>
        /// kiem tra user co dang xu ly khong,
        /// user: ldgv, ldpt, xlc va phxl
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="idcanbo"></param>
        /// <returns>true: neu co quyen xu ly</returns>
        public bool IsDangXulyHosocongviec(int idhosocongviec, int idcanbo)
        {
            // user phai co ten trong danh sach doi tuong xu ly
            // user: ldgv, ldpt, xlc, phxl
            // giong nhu trong IDoituongxulyRepository.getCanboDangxulys

            bool flag = false;
            var dt = _dtxulyRepo.GetCanboDangXulys
                       .Where(p => p.intidhosocongviec == idhosocongviec)
                       .Where(p => p.intidcanbo == idcanbo)
                       .Where(p => p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaogiaoviec
                               || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Lanhdaophutrach
                               || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Xulychinh
                               || p.intvaitro == (int)enumDoituongxuly.intvaitro_doituongxuly.Phoihopxuly)
                       .Select(p => p.intid);

            if (dt.Count() != 0)
            {
                flag = true;
            }

            return flag;
        }

        /// <summary>
        /// kiem tra ho so da duoc xu ly chua(hoan thanh chua )
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns>true: da hoan thanh</returns>
        public bool CheckHoanthanhHoso(int idhosocongviec)
        {
            bool flag = false;
            var inttrangthai = _hscongviecRepo.Hosocongviecs
                                .FirstOrDefault(p => p.intid == idhosocongviec)
                                .inttrangthai;
            if (inttrangthai == (int)enumHosocongviec.inttrangthai.Dahoanthanh)
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// kiem tra xem ho so nay co cho phep user dong khong
        /// </summary>
        /// <param name="idcongviec"></param>
        /// <returns></returns>
        public bool CheckQuyenDongHoso(int idcongviec)
        {
            try
            {
                bool flag = true;
                var intdonghoso = _hscongviecRepo.Hosocongviecs
                                    .FirstOrDefault(p => p.intid == idcongviec)
                                    .intdonghoso;
                if (intdonghoso == (int)enumHosocongviec.intdonghoso.Khong)
                {
                    flag = false;
                }
                return flag;
            }
            catch
            {
                return true;
            }

        }

        /// <summary>
        /// kiem tra xem van ban den nay da duoc phan xu ly chua
        /// tranh phan 2 lan 1 van ban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns>
        /// true: van ban nay chua phan xu ly
        /// false: van ban nay da phan xu ly roi, khong phan moi nua
        /// </returns>
        public bool CheckPhanHosocongviec(int idvanban, int intloai)
        {
            bool flag = true;
            try
            {
                var hsvb = _hsvanbanRepo.Hosovanbans
                        .Where(p => p.intidvanban == idvanban)
                        .Where(p => p.intloai == intloai);
                if (hsvb.Count() != 0)
                {
                    flag = false;
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// kiem tra xem user co quyen download file dinh kem trong ho so cong viec
        /// </summary>
        /// <param name="idfile"></param>
        /// <param name="idcanbo"></param>
        /// <param name="idhoso"></param>
        /// <returns></returns>
        public bool IsDownloadFileHosocongviec(int idfile, int idcanbo, int idhoso)
        {
            bool flag = false;
            // giong voi IsViewHosocongviec
            flag = IsViewHosocongviec(idhoso, idcanbo);
            return flag;
        }

        #endregion Hosocongviec
    }
}