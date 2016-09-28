using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Domain.Entities;
using QLVB.Core.Contract;
using QLVB.DTO.Linhvuc;
using QLVB.DTO;



namespace QLVB.WebUI.Controllers
{
    public class LinhvucController : Controller
    {
        #region Constructor

        private ILinhvucManager _linhvuc;
        public LinhvucController(ILinhvucManager linhvuc)
        {
            _linhvuc = linhvuc;
        }
        #endregion Constructor

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Linhvuc_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetLinhvuc().ToDataSourceResult(request));
        }

        private IEnumerable<ListLinhvucViewModel> GetLinhvuc()
        {
            var lv = _linhvuc.GetListLinhvuc();
            return lv;
        }

        public ActionResult _formEditLinhvuc(int id)
        {
            EditLinhvucViewModel model = _linhvuc.GetEditLinhvuc(id);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Save(EditLinhvucViewModel model)
        {
            ResultFunction kq = _linhvuc.Save(model);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json((int)ResultViewModels.Success);
            }
            else
            {
                return Json((int)ResultViewModels.Error);
            }
        }

        public ActionResult _formDeleteLinhvuc(int id)
        {
            EditLinhvucViewModel lv = _linhvuc.GetEditLinhvuc(id);
            DeleteLinhvucViewModel model = new DeleteLinhvucViewModel
            {
                intid = lv.intid,
                strtenlinhvuc = lv.strtenlinhvuc
            };
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            ResultFunction kq = _linhvuc.Delete(id);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json((int)ResultViewModels.Success);
            }
            else
            {
                return Json((int)ResultViewModels.Error);
            }
        }

    }
}