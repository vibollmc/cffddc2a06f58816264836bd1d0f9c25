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
using QLVB.DTO.Capcoquan;

namespace QLVB.WebUI.Controllers
{
    public class CapcoquanController : Controller
    {
        #region Constructor

        private ICapcoquanManager _coquan;
        public CapcoquanController(ICapcoquanManager coquan)
        {
            _coquan = coquan;
        }
        #endregion Constructor

        #region ViewIndex

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Khoiph_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetKhoiph().ToDataSourceResult(request));
        }

        private IEnumerable<Khoiphathanh> GetKhoiph()
        {
            return _coquan.GetListKhoiph();
        }

        public ActionResult _ListTochuc(int idkhoiph)
        {
            ViewBag.idkhoiph = idkhoiph;
            return PartialView();
        }

        public ActionResult Tochuc_Read([DataSourceRequest]DataSourceRequest request, int idkhoiph)
        {
            return Json(GetTochuc(idkhoiph).ToDataSourceResult(request));
        }

        private IEnumerable<Tochucdoitac> GetTochuc(int idkhoiph)
        {
            return _coquan.GetListTochuc(idkhoiph);
        }

        #endregion ViewIndex

        #region UpdateKhoiph
        public ActionResult _formEditKhoiph(int id)
        {
            EditKhoiphViewModel model = new EditKhoiphViewModel();
            if (id == 0)
            {   //them moi
                model.intid = 0;
            }
            else
            {   // cap nhat
                model = _coquan.GetEditKhoiph(id);
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SaveKhoiph(FormCollection collection)
        {
            EditKhoiphViewModel khoiph = new EditKhoiphViewModel();
            foreach (string p in collection)
            {
                if (p == "intid") khoiph.intid = Convert.ToInt32(collection[p]);
                if (p == "strtenkhoi") khoiph.strtenkhoi = collection[p];
            }

            ResultFunction kq = _coquan.SaveKhoiph(khoiph);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json((int)ResultViewModels.Success);
            }
            else
            {
                return Json((int)ResultViewModels.Error);
            }
        }

        public ActionResult _formDeleteKhoiph(int id)
        {
            EditKhoiphViewModel model = _coquan.GetEditKhoiph(id);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteKhoiph(int id, string strten)
        {
            EditKhoiphViewModel khoiph = new EditKhoiphViewModel
            {
                intid = id,
                strtenkhoi = strten
            };
            ResultFunction kq = _coquan.DeleteKhoiph(khoiph);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json((int)ResultViewModels.Success);
            }
            else
            {
                return Json((int)ResultViewModels.Error);
            }
        }

        #endregion UpdateKhoiph

        #region UpdateTochuc

        public ActionResult _formEditTochuc(int id, int idkhoiph)
        {
            EditTochucViewModel model = new EditTochucViewModel();
            if (id == 0)
            {
                // them moi
                model.intid = id;
                model.intidkhoiph = idkhoiph;
            }
            else
            {
                // cap nhat
                model = _coquan.GetEditTochuc(id);
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SaveTochuc(FormCollection collection)
        {
            EditTochucViewModel tochuc = new EditTochucViewModel();
            string strgiatri = "";
            foreach (string p in collection)
            {
                strgiatri = collection[p];
                if (!string.IsNullOrWhiteSpace(strgiatri))
                {
                    if (p == "intid") tochuc.intid = Convert.ToInt32(strgiatri);
                    if (p == "intidkhoiph") tochuc.intidkhoiph = Convert.ToInt32(strgiatri);
                    if (p == "strtentochucdoitac") tochuc.strtentochucdoitac = strgiatri;
                    if (p == "strmatochucdoitac") tochuc.strmatochucdoitac = strgiatri;
                    if (p == "strmadinhdanh") tochuc.strmadinhdanh = strgiatri;
                    if (p == "strphone") tochuc.strphone = strgiatri;
                    if (p == "strdiachi") tochuc.strdiachi = strgiatri;
                    if (p == "stremail") tochuc.stremail = strgiatri;
                    if (p == "IsHoibao") { if (strgiatri.Contains("true")) tochuc.IsHoibao = true; }
                    if (p == "Isvbdt") { if (strgiatri.Contains("true")) tochuc.Isvbdt = true; }
                    if (p == "stremailvbdt") tochuc.stremailvbdt = strgiatri;
                    if (p == "strmatructinh") tochuc.strmatructinh = strgiatri;
                }
            }

            ResultFunction kq = _coquan.SaveTochuc(tochuc);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json((int)ResultViewModels.Success);
            }
            else
            {
                return Json((int)ResultViewModels.Error);
            }
        }

        public ActionResult _formDeleteTochuc(int id)
        {
            EditTochucViewModel model = _coquan.GetEditTochuc(id);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteTochuc(int id, string strten)
        {
            EditTochucViewModel tochuc = new EditTochucViewModel
            {
                intid = id,
                strtentochucdoitac = strten
            };
            ResultFunction kq = _coquan.DeleteTochuc(tochuc);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json((int)ResultViewModels.Success);
            }
            else
            {
                return Json((int)ResultViewModels.Error);
            }
        }
        #endregion UpdateTochuc

        #region UpdateDonviFromXML

        public ActionResult CapnhatDanhmucDonvi()
        {
            CapnhatDanhmucDonviViewModel model = new CapnhatDanhmucDonviViewModel();
            model = _coquan.GetDanhmucDonvi();

            return View(model);
        }
        [HttpPost]
        public ActionResult CapnhatDanhmucDonvi(string listid)
        {
            var kq = _coquan.UpdateDanhmucDonvi(listid);

            CapnhatDanhmucDonviViewModel model = new CapnhatDanhmucDonviViewModel();
            model = _coquan.GetDanhmucDonvi();

            return View(model);
        }

        public ActionResult _GetKhoiph()
        {
            //return Json(GetKhoiph());
            var khoiph = GetKhoiph();
            return PartialView(khoiph);
        }
        [HttpPost]
        public ActionResult _AddNewDonvi(string listemail, int idkhoiph)
        {
            var kq = _coquan.AddNewDanhmucDonvi(listemail, idkhoiph);
            return Json("1");
        }

        #endregion UpdateDonviFromXML

    }
}