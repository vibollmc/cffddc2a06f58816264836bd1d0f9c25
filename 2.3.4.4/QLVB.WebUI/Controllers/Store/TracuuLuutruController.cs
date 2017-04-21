using Store.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.DTO.TracuuLuutru;
using QLVB.DTO;

namespace QLVB.WebUI.Controllers.Store
{
    public class TracuuLuutruController : Controller
    {
        #region Constructor
        private ITracuuLuutruManager _luutru;
        public TracuuLuutruController(ITracuuLuutruManager luutru)
        {
            _luutru = luutru;
        }

        #endregion Constructor

        [HttpPost]
        public ActionResult CapnhatVanban(int idvanban, int intloai)
        {
            CapnhatLuutruViewModel model = _luutru.GetFormCapnhatLuutru(idvanban, intloai);
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