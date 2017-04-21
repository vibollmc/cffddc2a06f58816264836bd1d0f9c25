using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Tinhchatvb;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace QLVB.WebUI.Controllers
{
    public class TinhchatvbController : Controller
    {
        #region Constructor
        private ITinhchatvanbanManager _tinhchatvb;
        public TinhchatvbController(ITinhchatvanbanManager tinhchatvb)
        {
            _tinhchatvb = tinhchatvb;
        }

        #endregion Constructor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Listtinhchatvb(int intloai)
        {
            ViewBag.intloai = intloai;
            return PartialView();
        }

        public ActionResult Detailvb_Read([DataSourceRequest]DataSourceRequest request, int intloai)
        {
            return Json(GetListTinhchat(intloai).ToDataSourceResult(request));
        }

        private IEnumerable<Tinhchatvanban> GetListTinhchat(int intloai)
        {
            var vb = _tinhchatvb.GetTinhchatvb(intloai);
            return vb;
        }
        public ActionResult _formEdit(int id, int? idloai)
        {
            EditTinhchatViewModel model = new EditTinhchatViewModel();
            if (id == 0)
            {   // them moi
                model.intid = 0;
                model.intloai = (int)idloai;
            }
            else
            {
                model = _tinhchatvb.GetEdit(id);
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Save(FormCollection collection)
        {
            EditTinhchatViewModel vb = new EditTinhchatViewModel();
            foreach (string p in collection)
            {
                if (p == "intid") vb.intid = Convert.ToInt32(collection[p]);
                if (p == "intloai") vb.intloai = Convert.ToInt32(collection[p]);
                if (p == "strkyhieu") vb.strkyhieu = collection[p];
                if (p == "strtentinhchatvb") vb.strtentinhchatvb = collection[p];
                if (p == "strmota") vb.strmota = collection[p];
            }

            ResultFunction kq = _tinhchatvb.Save(vb);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        public ActionResult _formDelete(int id)
        {
            EditTinhchatViewModel model = _tinhchatvb.GetEdit(id);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, string strten)
        {
            EditTinhchatViewModel vb = new EditTinhchatViewModel();
            vb.intid = id;
            vb.strtentinhchatvb = strten;
            ResultFunction kq = _tinhchatvb.Delete(vb);
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