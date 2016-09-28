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
    }
}