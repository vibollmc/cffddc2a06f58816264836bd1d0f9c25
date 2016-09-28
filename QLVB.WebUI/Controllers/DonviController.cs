using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using QLVB.DTO.Donvi;
using QLVB.Common.Date;

namespace QLVB.WebUI.Controllers
{
    public class DonviController : Controller
    {
        #region Constructor
        private IDonviManager _donvi;
        public DonviController(IDonviManager donvi)
        {
            _donvi = donvi;
        }

        #endregion Constructor

        #region ViewIndex
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Listdonvi()
        {
            var donvi = _donvi.GetRootDonvi();
            return PartialView(donvi);
        }

        public ActionResult _ListUser(int iddonvi)
        {
            ViewBag.iddonvi = iddonvi;
            return PartialView();
        }
        public ActionResult User_Read([DataSourceRequest]DataSourceRequest request, int iddonvi)
        {
            return Json(GetListUsers(iddonvi).ToDataSourceResult(request));
        }

        private IEnumerable<ListUserViewModel> GetListUsers(int iddonvi)
        {
            var user = _donvi.GetListUsers(iddonvi).OrderBy(p => p.strkyhieu).ThenBy(p => p.strhoten);
            return user;
        }
        #endregion ViewIndex

        #region UpdateDonvi
        public ActionResult _formEditDonvi(int id, string type)
        {
            EditDonviViewModel model = new EditDonviViewModel();
            if (type == "add")
            {
                model.intid = id;
                model.intType = 0;
            }
            if (type == "edit")
            {
                model = _donvi.GetDonvi(id);
                model.intType = 1;
            }
            return PartialView(model);
        }

        /// <summary>
        /// dung trong submit form de tu load lai trang 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveDonvi(EditDonviViewModel model)
        {
            int kq = _donvi.SaveDonvi(model);
            return RedirectToAction("Index", "Donvi");
        }

        /// <summary>
        /// dung trong jquery ajax
        /// HIEN KHONG SU DUNG
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveDonvi2(EditDonviViewModel model)
        {
            int kq = _donvi.SaveDonvi(model);
            if (kq == (int)ResultViewModels.Success)
            {
                return Json((int)ResultViewModels.Success);
            }
            else
            {
                return Json((int)ResultViewModels.Error);
            }
        }

        public ActionResult _formDeleteDonvi(int id)
        {
            EditDonviViewModel dv = new EditDonviViewModel();
            dv = _donvi.GetDonvi(id);
            DeleteDonviViewModel model = new DeleteDonviViewModel
            {
                intid = dv.intid,
                strtendonvi = dv.strtendonvi
            };
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult DeleteDonvi(DeleteDonviViewModel model)
        {
            int kq = _donvi.DeleteDonvi(model);
            return RedirectToAction("Index", "Donvi");
        }

        #endregion UpdateDonvi

        #region UpdateUser
        public ActionResult _formEditUser(int iduser, int iddonvi)
        {
            EditUserViewModel model = _donvi.GetUser(iduser, iddonvi);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SaveUser(FormCollection collection)
        {
            string strgiatri = string.Empty;
            EditUserViewModel user = new EditUserViewModel();
            foreach (string p in collection)
            {
                strgiatri = collection[p];
                if (!string.IsNullOrEmpty(strgiatri))
                {
                    if (p == "intid") { user.intid = Convert.ToInt32(strgiatri); }
                    if (p == "iddonvi") { user.iddonvi = Convert.ToInt32(strgiatri); }

                    if (p == "strmacanbo") { user.strmacanbo = strgiatri; }
                    if (p == "strhoten") { user.strhoten = strgiatri; }
                    if (p == "strkyhieu") { user.strkyhieu = strgiatri; }
                    if (p == "strdienthoai") { user.strdienthoai = strgiatri; }
                    if (p == "stremail") { user.stremail = strgiatri; }

                    if (p == "strngaysinh") { user.strngaysinh = DateServices.FormatDateEn(strgiatri); }
                    if (p == "enumgioitinh")
                    {
                        int intgioitinh = Convert.ToInt32(strgiatri);
                        if (intgioitinh == (int)enumcanbo.intgioitinh.Nam)
                        {
                            user.enumgioitinh = enumcanbo.intgioitinh.Nam;
                        }
                        else
                        {
                            user.enumgioitinh = enumcanbo.intgioitinh.Nu;
                        }
                    }
                    if (p == "intchucvu") { user.intchucvu = Convert.ToInt32(strgiatri); }
                    if (p == "intnhomquyen") { user.intnhomquyen = Convert.ToInt32(strgiatri); }
                    if (p == "intkivb")
                    {
                        if (strgiatri.Contains("true")) { user.intkivb = true; }
                        else { user.intkivb = false; }
                    }
                    if (p == "IsNguoiXL")
                    {
                        if (strgiatri.Contains("true")) { user.IsNguoiXL = true; }
                        else { user.IsNguoiXL = false; }
                    }
                    if (p == "IsDefaultXLBD")
                    {
                        if (strgiatri.Contains("true")) { user.IsDefaultXLBD = true; }
                        else { user.IsDefaultXLBD = false; }
                    }
                    if (p == "strusername") { user.strusername = strgiatri; }
                    if (p == "strpassword") { user.strpassword = strgiatri; }
                    if (p == "ConfirmPassword") { user.ConfirmPassword = strgiatri; }

                }
            }
            ResultFunction kq = _donvi.SaveUser(user);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        [HttpPost]
        public ActionResult CheckUsername(string strusername)
        {
            bool flag = _donvi.CheckUsername(strusername);
            if (flag == true)
            {   // chua co user name trong database
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }

        public ActionResult _formDeleteUser(int id)
        {
            var cb = _donvi.GetUser(id, 0);
            DeleteUserViewModel model = new DeleteUserViewModel
            {
                intid = id,
                strhoten = cb.strhoten
            };
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            ResultFunction kq = _donvi.DeleteUser(id);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        public ActionResult _formMoveUser(int idsource, int iddest)
        {
            MoveUserViewModel model = _donvi.GetMoveUser(idsource, iddest);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult SaveMoveUser(string strlistiduser, int iddonvi)
        {
            ResultFunction kq = _donvi.UpdateMoveUser(strlistiduser, iddonvi);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        #endregion UpdateUser
    }
}