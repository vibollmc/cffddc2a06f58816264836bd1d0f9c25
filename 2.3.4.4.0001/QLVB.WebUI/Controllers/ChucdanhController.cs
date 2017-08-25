using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Domain.Entities;
using QLVB.Core.Contract;
using QLVB.DTO.Chucdanh;
using QLVB.DTO;

namespace QLVB.WebUI.Controllers
{
    public class ChucdanhController : Controller
    {
        #region Constructor
        private IChucdanhManager _chucdanh;
        public ChucdanhController(IChucdanhManager chucdanh)
        {
            _chucdanh = chucdanh;
        }

        #endregion Constructor

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chucdanh_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetChucdanhs().ToDataSourceResult(request));
        }

        private IEnumerable<ListChucdanhViewModel> GetChucdanhs()
        {
            var chucdanh = _chucdanh.GetListChucdanh();
            return chucdanh;
        }

        public ActionResult _AjaxEditChucdanh(int id)
        {
            var cd = new EditChucdanhViewModel
            {
                intid = id,
                strmachucdanh = string.Empty,
                strtenchucdanh = string.Empty,
                strghichu = string.Empty,
                Loaichucdanh = enumchucdanh.intloai.Canbo
            };
            if (id != 0)
            {   // cap nhat
                Chucdanh chucdanh = _chucdanh.GetChucdanh(id);
                cd.intid = chucdanh.intid;
                cd.intloai = (int)chucdanh.intloai;
                cd.strghichu = chucdanh.strghichu;
                cd.strmachucdanh = chucdanh.strmachucdanh;
                cd.strtenchucdanh = chucdanh.strtenchucdanh;
                cd.Loaichucdanh = chucdanh.intloai == (int)enumchucdanh.intloai.Lanhdao ? enumchucdanh.intloai.Lanhdao : enumchucdanh.intloai.Canbo;
            }
            return PartialView(cd);
        }

        [HttpPost]
        public ActionResult Save(EditChucdanhViewModel model)
        {
            int kq = _chucdanh.Save(model);
            if (kq == (int)ResultViewModels.Success)
            {
                return Json((int)ResultViewModels.Success);
            }
            else
            {
                return Json((int)ResultViewModels.Error);
            }
        }

        public ActionResult _AjaxDeleteChucdanh(int id)
        {
            var cd = _chucdanh.GetChucdanh(id);
            DeleteChucdanhViewModel model = new DeleteChucdanhViewModel
            {
                intid = cd.intid,
                strtenchucdanh = cd.strtenchucdanh
            };
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Delete(DeleteChucdanhViewModel model)
        {
            int kq = _chucdanh.Delete(model);
            if (kq == (int)ResultViewModels.Success)
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