using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Domain.Abstract;
using QLVB.DTO;
using QLVB.Core.Contract;

namespace QLVB.WebUI.Controllers
{
    public class TestController : Controller
    {
        private ICanboRepository _canboRepo;
        private IMailManager _mail;
        private IMailFormatManager _format;
        private IEdxmlManager _edxml;

        public TestController(ICanboRepository canboRepo, IMailManager mail, IMailFormatManager format, IEdxmlManager edxml)
        {
            _canboRepo = canboRepo;
            _mail = mail;
            _format = format;
            _edxml = edxml;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Mail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult _DecodeMail(string strnoidung)
        {

            return Json(1);
        }
        public ActionResult _AutoReceiveMail()
        {
            //IMailManager mail;
            //mail = QLVB.Core.Implementation.AutoReceiveMailSingleton.
            //mail = new QLVB.Core.Implementation.AutoReceiveMailSingleton();

            //QLVB.Core.Implementation.AutoReceiveMailSingleton auto = QLVB.Core.Implementation.AutoReceiveMailSingleton.Instance;
            //auto.NhanVBDT(1, 1);

            return Json(1);


        }

        public ActionResult _ReadEdxml(string fileEdxml)
        {
            //_edxml.ReadEdxml(fileEdxml);
            return Json(1);
        }
    }
}