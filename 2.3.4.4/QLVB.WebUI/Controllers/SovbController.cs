using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Core.Contract;
using QLVB.DTO;
using QLVB.DTO.Sovb;
using QLVB.Domain.Entities;

namespace QLVB.WebUI.Controllers
{
    public class SovbController : Controller
    {
        #region Constructor
        private ISovanbanManager _sovb;
        public SovbController(ISovanbanManager sovb)
        {
            _sovb = sovb;
        }

        #endregion Constructor

        #region ListSovanban

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Listsovb()
        {
            ListSovanbanViewModel model = _sovb.GetListSovb();

            return PartialView(model);
        }

        public ActionResult _AjaxSovanban(int id)
        {
            int intloai = _sovb.GetLoaiSovb(id);
            if (intloai == (int)enumSovanban.intloai.Vanbanden)
            {
                ListKhoiphathanhViewModel model = _sovb.GetListKhoiphathanh(id);
                return PartialView("_AjaxSovanbanden", model);
            }
            else
            {
                ListLoaivanbanViewModel model = _sovb.GetListLoaivanban(id);
                return PartialView("_AjaxSovanbandi", model);
            }
        }

        [HttpPost]
        public ActionResult SaveKhoiphathanh(int idsovb, FormCollection collection)
        {
            int idkhoiph = 0;
            foreach (string p in collection)
            {
                if (collection[p].Contains("true")) idkhoiph = Convert.ToInt32(p);
            }
            ResultFunction kq = _sovb.SaveKhoiphathanh(idsovb, idkhoiph);
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
        public ActionResult SaveLoaivb(int idsovb, FormCollection collection)
        {
            int idloaivb = 0;
            foreach (string p in collection)
            {
                if (collection[p].Contains("true")) idloaivb = Convert.ToInt32(p);
            }
            ResultFunction kq = _sovb.SaveLoaivanban(idsovb, idloaivb);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        public ActionResult _formEditSovanban(int id)
        {
            EditSovanbanViewModel model = new EditSovanbanViewModel();
            if (id > 0)
            {   // cap nhat
                model = _sovb.GetEditSovanban(id);
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SaveSovb(FormCollection collection)
        {
            EditSovanbanViewModel sovb = new EditSovanbanViewModel();
            foreach (string p in collection)
            {
                if (p == "intid") sovb.intid = Convert.ToInt32(collection[p]);
                if (p == "strten") sovb.strten = collection[p];
                if (p == "strkyhieu") sovb.strkyhieu = collection[p];
                if (p == "strghichu") sovb.strghichu = collection[p];
                if (p == "Loaisovanban") sovb.intloai = Convert.ToInt32(collection[p]);
                if (p == "IsDefault")
                {
                    if (collection[p].Contains("true"))
                    {
                        sovb.IsDefault = true;
                    }
                    else
                    {
                        sovb.IsDefault = false;
                    }
                }
                //if (p == "intorder")
                //{
                //    if (!string.IsNullOrEmpty(collection[p]))
                //        sovb.intorder = Convert.ToInt32(collection[p]);
                //}
            }
            ResultFunction kq = _sovb.SaveSovanban(sovb);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        public ActionResult _formDeleteSovanban(int id)
        {
            EditSovanbanViewModel model = _sovb.GetEditSovanban(id);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Delete(int id, string strten)
        {
            EditSovanbanViewModel sovb = new EditSovanbanViewModel
            {
                intid = id,
                strten = strten
            };
            ResultFunction kq = _sovb.DeleteSovanban(sovb);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }
        #endregion ListSovanban


    }
}