using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Core.Contract;
using QLVB.DTO;
using QLVB.DTO.Luutru;

namespace QLVB.WebUI.Controllers
{
    public class LuutruController : Controller
    {
        #region Constructor
        private ILuutruManager _luutru;
        public LuutruController(ILuutruManager luutru)
        {
            _luutru = luutru;
        }

        #endregion Constructor

        public ActionResult Index()
        {
            var donvi = _luutru.GetRootDonvi();
            return View(donvi);
        }

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
                model = _luutru.GetDonvi(id);
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
            int kq = _luutru.SaveDonvi(model);
            return RedirectToAction("Index", "Luutru");
        }

        public ActionResult _formDeleteDonvi(int id)
        {
            EditDonviViewModel dv = _luutru.GetDonvi(id);
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
            int kq = _luutru.DeleteDonvi(model);
            return RedirectToAction("Index", "Luutru");
        }

        [HttpPost]
        public ActionResult CapnhatVanban(int idvanban, int intloai)
        {
            QLVB.DTO.TracuuLuutru.CapnhatLuutruViewModel model = _luutru.GetFormCapnhatLuutru(idvanban, intloai);
            return View(model);
        }

        [HttpPost]
        public ActionResult _SaveLuutru(FormCollection collection)
        {
            string strgiatri = "";
            QLVB.Domain.Entities.LuutruVanban lt = new Domain.Entities.LuutruVanban();
            foreach (string p in collection)
            {
                strgiatri = collection[p];
                if (p == "intid") { lt.intid = Convert.ToInt32(strgiatri); }
                if (p == "idvanban") { lt.intidvanban = Convert.ToInt32(strgiatri); }
                if (p == "intloai") { lt.intloaivanban = Convert.ToInt32(strgiatri); }
                if (p == "inthopso") { lt.inthopso = Convert.ToInt32(strgiatri); }
                if (p == "intdonvibaoquan") { lt.intdonvibaoquan = Convert.ToInt32(strgiatri); }
                if (p == "strthoihanbaoquan") { lt.strthoihanbaoquan = strgiatri; }
                if (p == "strnoidung") { lt.strnoidung = strgiatri; }
            }
            int kq = _luutru.SaveLuutruVanban(lt);

            return Json(kq);

        }
    }
}