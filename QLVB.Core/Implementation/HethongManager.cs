using QLVB.Common.Logging;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Hethong;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace QLVB.Core.Implementation
{
    public class HethongManager : IHethongManager
    {
        #region Constructor

        private ILogger _logger;
        private IConfigRepository _configRepo;
        private INhomQuyenRepository _grouproleRepo;
        private IQuyenRepository _quyenRepo;
        private IQuyenNhomQuyenRepository _quyengroupRepo;
        private IMenuRepository _menuRepo;
        private ICanboRepository _canboRepo;
        private IChucdanhRepository _chucdanhRepo;
        private IDonvitructhuocRepository _donviRepo;
        private ISqlQuery _sqlQuery;

        public HethongManager(IConfigRepository configRepo,
                            INhomQuyenRepository grouproleRepo,
                            IQuyenRepository quyenRepo,
                            IQuyenNhomQuyenRepository quyengroupRepo,
                            IMenuRepository menuRepo,
                            ICanboRepository canboRepo,
                            ILogger logger,
                            IChucdanhRepository chucdanhRepo,
                            IDonvitructhuocRepository donviRepo,
                            ISqlQuery sqlQuery
                           )
        {
            _configRepo = configRepo;
            _grouproleRepo = grouproleRepo;
            _quyenRepo = quyenRepo;
            _quyengroupRepo = quyengroupRepo;
            _menuRepo = menuRepo;
            _canboRepo = canboRepo;
            _logger = logger;
            _chucdanhRepo = chucdanhRepo;
            _donviRepo = donviRepo;
            _sqlQuery = sqlQuery;
        }

        #endregion Constructor

        #region Interface Implementation

        public IEnumerable<Config> GetAllConfig()
        {
            var _config = _configRepo.Configs
                    .OrderBy(p => p.intgroup)
                    .ThenBy(p => p.intorder);
            return _config;
        }

        public int SaveAllConfig(Dictionary<string, string> config)
        {
            int kq = 0;
            foreach (var p in config)
            {
                if (!string.IsNullOrEmpty(p.Value))
                {
                    _configRepo.SaveConfig(p.Key, p.Value);
                    kq = 1;
                }
            }
            if (kq == 1)
            {
                _logger.Info("Cập nhật cấu hình hệ thống");
            }
            return kq;
        }

        public int SaveAllConfig(Dictionary<int, string> config)
        {
            int kq = 0;
            foreach (var p in config)
            {
                if (!string.IsNullOrEmpty(p.Value))
                {
                    _configRepo.SaveConfig(p.Key, p.Value);
                    kq = 1;
                }
            }
            if (kq == 1)
            {
                _logger.Info("Cập nhật cấu hình hệ thống");
            }
            return kq;
        }

        public IEnumerable<NhomQuyen> GetAllNhomQuyen()
        {
            var _nhomquyen = _grouproleRepo.GetActiveNhomQuyens.Where(p => p.inttrangthai == 1).OrderBy(p => p.strtennhom);
            return _nhomquyen;
        }

        public ListRoleGroupViewModel GetQuyenNhom(int idgroup)
        {
            ListRoleGroupViewModel quyennhom = new ListRoleGroupViewModel
            {
                QuyenVM = _quyenRepo.Quyens.OrderBy(q => q.intorder),

                RoleGroupCheckVM = (from c in _quyengroupRepo.QuyenNhomQuyens
                                    where c.intidnhomquyen == idgroup
                                    select (new RoleGroupCheckViewModel
                                    {
                                        intidquyen = c.intidquyen
                                    })
                                    ),

                MenuVM = _menuRepo.Menus
                            .Where(m => m.ParentId != null)
                            .Where(m => m.inttrangthai == (int)enummenu.inttrangthai.IsActive)
                            .OrderByDescending(m => m.ParentId)
                            .ThenBy(m => m.intorder),

                idgroup = idgroup
            };
            return quyennhom;
        }

        public int SaveQuyenNhom(int idgroup, List<int> role)
        {
            try
            {
                // -- delete all from nhomquyen
                _quyengroupRepo._DeleteRoleGroup(idgroup);
                //  --- them tung quyen duoc check vao
                _quyengroupRepo._AddRoleGroup(role, idgroup);

                //foreach (int p in role)
                //{
                //    var addquyen = new QuyenNhomQuyen
                //    {
                //        intidquyen = p,
                //        intidnhomquyen = idgroup
                //    };
                //    _quyengroupRepo._AddRoleGroup(addquyen);
                //}
                // luu cac cap nhat vao he thong
                //_quyengroupRepo.SaveChanges();

                var strtennhom = _grouproleRepo.GetActiveNhomQuyens.SingleOrDefault(p => p.intid == idgroup).strtennhom;
                _logger.Info("Cập nhật quyền của nhóm quyền: " + strtennhom);

                return 1;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return 0;
            }
        }

        public ListUserViewModel GetUserGroup(int idgroup)
        {
            ListUserViewModel listUser = new ListUserViewModel
            {
                idgroup = idgroup,
                user = _canboRepo.GetActiveCanbo
                        .Where(p => p.intnhomquyen == idgroup)
                        .OrderBy(p => p.strkyhieu)
                        .ThenBy(p => p.strhoten)
                        .Select(p => new UserViewModel
                        {
                            intid = p.intid,
                            strhoten = p.strhoten,
                            strkyhieu = p.strkyhieu,
                            strchucvu = _chucdanhRepo.Chucdanhs.FirstOrDefault(m => m.intid == p.intchucvu).strtenchucdanh
                            //strphongban = _donviRepo.Donvitructhuocs.FirstOrDefault(m=>m.Id ==p.int)
                        })
            };
            return listUser;
        }

        public IEnumerable<UserViewModel> GetUserInGroup(int idgroup)
        {
            var user = _canboRepo.GetActiveCanbo
                        .Where(p => p.intnhomquyen == idgroup)
                        .OrderBy(p => p.strkyhieu)
                        .ThenBy(p => p.strhoten)
                        .Select(p => new UserViewModel
                        {
                            intid = p.intid,
                            strhoten = p.strhoten,
                            strkyhieu = p.strkyhieu,
                            strchucvu = _chucdanhRepo.Chucdanhs.FirstOrDefault(m => m.intid == p.intchucvu).strtenchucdanh
                            //strphongban = _donviRepo.Donvitructhuocs.FirstOrDefault(m=>m.Id ==p.int)
                        });
            return user;
        }

        public EditGroupViewModel GetGroup(int intid)
        {
            var group = _grouproleRepo.GetActiveNhomQuyens.Where(p => p.intid == intid)
                        .Select(p => new EditGroupViewModel
                        {
                            intid = p.intid,
                            strtennhomquyen = p.strtennhom
                        }).FirstOrDefault();
            return group;
        }

        public int SaveGroup(EditGroupViewModel model)
        {
            try
            {
                if (model.intid == 0)
                {   // them moi
                    _grouproleRepo.SaveGroup(model.strtennhomquyen);
                    _logger.Info("Thêm mới nhóm quyền: " + model.strtennhomquyen);
                }
                else
                {   // cap nhat
                    _grouproleRepo.EditGroup(model.intid, model.strtennhomquyen);
                    _logger.Info("Cập nhật nhóm quyền id: " + model.intid);
                }
                return 1;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return 0;
            }
        }

        public int DeleteGroup(int intid)
        {
            try
            {
                _grouproleRepo.DeleteGroup(intid);
                _logger.Info("Xóa nhóm quyền id: " + intid.ToString());
                return 1;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// lay tat ca cac user thuoc nhom quyen dang xet
        /// </summary>
        /// <param name="idnhomquyen"></param>
        /// <returns></returns>
        public MoveNhomquyenUserViewModel GetNhomquyenUser(int idnhomquyen)
        {
            MoveNhomquyenUserViewModel user = new MoveNhomquyenUserViewModel();
            user.userSource = _canboRepo.GetActiveCanbo.Where(p => p.intnhomquyen == idnhomquyen)
                            .Select(p => new UserViewModel
                            {
                                intid = p.intid,
                                strhoten = p.strhoten,
                                strkyhieu = p.strkyhieu,
                                idnhomquyen = p.intnhomquyen
                            })
                            .OrderBy(p => p.strkyhieu)
                            .ThenBy(p => p.strhoten);

            user.idnhomquyen = idnhomquyen;

            user.userDest = _canboRepo.GetActiveCanbo.Where(p => p.intnhomquyen != idnhomquyen)
                            .Select(p => new UserViewModel
                            {
                                intid = p.intid,
                                strhoten = p.strhoten,
                                strkyhieu = p.strkyhieu,
                                idnhomquyen = p.intnhomquyen
                            })
                            .OrderBy(p => p.strkyhieu)
                            .ThenBy(p => p.strhoten);

            return user;
        }

        /// <summary>
        /// cap nhat nhom quyen cua cac user
        /// </summary>
        /// <param name="listiduser"></param>
        /// <param name="idnhomquyen"></param>
        /// <returns></returns>
        public ResultFunction SaveNhomquyenUser(string strlistiduser, int idnhomquyen)
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
                        _canboRepo.UpdateNhomquyen(iduser, idnhomquyen);
                    }
                }
                _logger.Info("Cập nhật nhóm quyền của các user");
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

        //==================================================
        // backup
        //==================================================

        public BackupViewModel GetFormBackup()
        {
            BackupViewModel model = new BackupViewModel();

            model.strPath = _GetPathBackupDatabase();
            model.strDatabase = _GetDatabaseName();
            model.IsCheckPath = System.IO.Directory.Exists(model.strPath);
            model.strFileName = model.strDatabase + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";


            return model;
        }

        private string _GetDatabaseName()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["QLVBDatabase"];
            string strconnect = ConfigurationManager.ConnectionStrings["QLVBDatabase"].ConnectionString;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(strconnect);
            string @Database = builder.InitialCatalog;
            return @Database;
        }

        private string _GetPathBackupDatabase()
        {
            string filePath = QLVB.Common.Utilities.AppSettings.BackupDatabase;
            filePath = HttpContext.Current.Server.MapPath(filePath);
            return filePath;
        }

        public ResultFunction BackupDatabase(BackupViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                //ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["QLVBDatabase"];
                //string strconnect = ConfigurationManager.ConnectionStrings["QLVBDatabase"].ConnectionString;
                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(strconnect);
                //string @Database = builder.InitialCatalog;

                //string @path = QLVB.Common.Utilities.AppSettings.BackupDatabase;
                //string query = string.Empty;
                //string filename = @Database + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
                //@path = HttpContext.Current.Server.MapPath(@path);


                model.strDatabase = _GetDatabaseName();
                model.strPath = _GetPathBackupDatabase();

                if (System.IO.Directory.Exists(model.strPath))
                {
                    string filepath = model.strPath + "\\" + model.strFileName;

                    string query = "backup database [" + model.strDatabase + "] to disk ='" + filepath + "'; ";

                    int flag = _sqlQuery.RunSqlCommand(query);

                    kq.id = (int)ResultViewModels.Success;
                }
                else
                {
                    kq.id = (int)ResultViewModels.Error;
                    kq.message = "Không tìm thấy đường dẫn sao lưu";
                    _logger.Info("Lỗi không sao lưu dữ liệu. Không tìm thấy đường dẫn sao lưu");
                }

            }
            catch (Exception ex)
            {
                _logger.Info("Lỗi không sao lưu dữ liệu. " + ex.Message);
                kq.id = (int)ResultViewModels.Error;
                kq.message = ex.Message;
            }
            return kq;

        }


        #endregion Interface Implementation
    }
}