using QLVB.Common.Sessions;
using QLVB.Common.Utilities;
using QLVB.DTO.Hoso;
using Store.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLVB.WebUI.Controllers.Store
{
    public class TracuuHosoController : Controller
    {
        #region Constructor

        private ITracuuHosoManager _hoso;
        private ISessionServices _session;
        public TracuuHosoController(ITracuuHosoManager hoso, ISessionServices session)
        {
            _hoso = hoso;
            _session = session;
        }

        #endregion Constructor

        #region ViewDetailHosocongviec

        public ActionResult _ViewDetailHoso()
        {
            return PartialView();
        }

        public ActionResult _XemchitietHoso(int id)
        {
            DetailHosoViewModel model = _hoso.GetDetailHoso(id);
            if (model.idhosocongviec == 0)
            {
                return new ViewResult { ViewName = AppConts.ErrAccessDenied };
            }
            else
            {
                return PartialView(model);
            }
        }

        #endregion ViewDetailHosocongviec
    }
}