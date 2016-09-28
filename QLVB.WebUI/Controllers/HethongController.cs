using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Hethong;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace QLVB.WebUI.Controllers
{
    public class HethongController : Controller
    {
        #region Constructor
        private IHethongManager _hethong;
        public HethongController(IHethongManager hethong)
        {
            _hethong = hethong;
        }

        #endregion Constructor

        public ActionResult Config()
        {
            var config = _hethong.GetAllConfig();
            return View(config);
        }

        /// <summary>
        /// cap nhat cau hinh he thong
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveConfig(FormCollection collection)
        {
            string strgiatri = "";
            Dictionary<int, string> fCollection = new Dictionary<int, string>();
            foreach (string p in collection)
            {
                strgiatri = collection[p];
                fCollection.Add(Convert.ToInt32(p), collection[p]);
            }
            int kq = _hethong.SaveAllConfig(fCollection);
            if (kq == 1)
            {
                return Json(ResultViewModels.Success);
            }
            else
            {
                return Json(ResultViewModels.Error);
            }
        }

        public ActionResult RoleGroup()
        {

            return View();
        }

        /// <summary>
        /// kendo grid loading
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Group_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetGroups().ToDataSourceResult(request));
            //return Json(1);
        }

        private IEnumerable<RoleGroupViewModel> GetGroups()
        {
            var group = _hethong.GetAllNhomQuyen().Select(g => new RoleGroupViewModel
            {
                intid = g.intid,
                strten = g.strtennhom
            });
            return group;
        }

        /// <summary>
        /// liet ke cac quyen trong nhom quyen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult _AjaxQuyen(int id)
        {
            ListRoleGroupViewModel quyennhom = _hethong.GetQuyenNhom(id);
            quyennhom.idgroup = id;
            return PartialView(quyennhom);
        }

        /// <summary>
        /// save quyen 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveQuyen(FormCollection collection)
        {
            //string strgiatri = "";
            //Dictionary<int, bool> fCollection = new Dictionary<int, bool>();
            List<int> fCollection = new List<int>();
            int idgroup = 0;
            foreach (string p in collection)
            {
                if (p == "idgroup")
                {
                    idgroup = Convert.ToInt32(collection[p]);
                }
                else
                {
                    //strgiatri = collection[p];
                    if (collection[p].Contains("true"))
                    {
                        fCollection.Add(Convert.ToInt32(p));
                    }
                }
            }
            int kq = _hethong.SaveQuyenNhom(idgroup, fCollection);
            //--------- cập nhật lại quyền của user trong session 


            if (kq == 1)
            {
                return Json(ResultViewModels.Success);
            }
            else
            {
                return Json(ResultViewModels.Error);
            }
        }

        /// <summary>
        /// liet ke cac user trong nhom quyen
        /// </summary>
        /// <param name="idgroup"></param>
        /// <returns></returns>
        public ActionResult _AjaxUser(int idgroup)
        {
            //ListUserViewModel listUser = _hethong.GetUserGroup(idgroup);
            ViewBag.idgroup = idgroup;
            return PartialView();
        }

        public ActionResult User_Read([DataSourceRequest]DataSourceRequest request, int idgroup)
        {
            return Json(GetUsers(idgroup).ToDataSourceResult(request));
            //return Json(1);
        }
        private IEnumerable<UserViewModel> GetUsers(int idgroup)
        {
            var user = _hethong.GetUserInGroup(idgroup).OrderBy(p => p.strkyhieu).ThenBy(p => p.strhoten);
            return user;
        }

        /// <summary>
        /// hien thi nhom quyen
        /// </summary>
        /// <param name="idgroup"></param>
        /// <returns></returns>
        public ActionResult _AjaxEditGroup(int idgroup)
        {
            var group = new EditGroupViewModel { intid = idgroup, strtennhomquyen = string.Empty };
            if (idgroup != 0)
            {   // idgroup != 0 : edit
                // idgroup == 0 : addnew
                group = _hethong.GetGroup(idgroup);
            }
            return PartialView(group);
        }
        /// <summary>
        /// them moi/cap nhat nhom quyen
        /// http://stackoverflow.com/questions/16122949/submit-form-with-jquery-ajax
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveGroup(EditGroupViewModel model)
        {
            int kq = _hethong.SaveGroup(model);
            if (kq == 1)
            {
                return Json(ResultViewModels.Success);
            }
            else
            {
                return Json(ResultViewModels.Error);
            }
        }

        /// <summary>
        /// orverride for fluent security
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveGroup()
        {
            return new EmptyResult();
        }

        public ActionResult _AjaxDeleteGroup(int idgroup)
        {
            EditGroupViewModel group = _hethong.GetGroup(idgroup);
            return PartialView(group);
        }

        [HttpPost]
        public JsonResult DeleteGroup(int idgroup)
        {
            int kq = _hethong.DeleteGroup(idgroup);
            if (kq == 1)
            {
                return Json(ResultViewModels.Success);
            }
            else
            {
                return Json(ResultViewModels.Error);
            }
        }

        public ActionResult _MoveNhomquyenUser(int idgroup)
        {
            MoveNhomquyenUserViewModel model = _hethong.GetNhomquyenUser(idgroup);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveNhomquyenUser(string strlistiduser, int idgroup)
        {
            ResultFunction kq = _hethong.SaveNhomquyenUser(strlistiduser, idgroup);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(ResultViewModels.Success);
            }
            else
            {
                return Json(ResultViewModels.Error);
            }
        }

        public ActionResult Backup()
        {
            BackupViewModel model = _hethong.GetFormBackup();
            return View(model);
        }
        [HttpPost]
        public JsonResult _SaveBackup(BackupViewModel model)
        {
            ResultFunction kq = _hethong.BackupDatabase(model);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }
    }
}