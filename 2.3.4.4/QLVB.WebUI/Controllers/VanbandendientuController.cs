using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Core.Contract;
using QLVB.Common.Sessions;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Vanbandientu;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using QLVB.Common.Utilities;
using DocumentFormat.OpenXml.Packaging;
using QLVB.WebUI.Common.OpenXML;
using DocumentFormat.OpenXml.Spreadsheet;
using QLVB.Common.Date;


namespace QLVB.WebUI.Controllers
{
    public class VanbandendientuController : Controller
    {
        #region Constructor

        private IVanbandientuManager _vanban;
        private ISessionServices _session;
        private IEdxmlManager _edxml;
        private ITrucLienthongTinhManager _trucLienthongTinhManager;
        public VanbandendientuController(IVanbandientuManager vanban, ISessionServices session, IEdxmlManager edxml, ITrucLienthongTinhManager trucLienthongTinhManager)
        {
            _vanban = vanban;
            _session = session;
            _edxml = edxml;
            _trucLienthongTinhManager = trucLienthongTinhManager;
        }

        #endregion Constructor

        #region ViewIndex
        public ActionResult Index(bool? isBack)
        {
            ViewBag.isBack = isBack == true ? true : false;
            ViewBag.inttinhtrangcat = 1;
            ToolbarVBDenViewModel model = _vanban.GetToolbarVBDen();

            return View(model);
        }

        public ActionResult _Category()
        {
            ViewBag.Songayhienthi = _vanban.GetSongayhienthi();
            return PartialView();
        }

        public ActionResult _ListVanbandendientu(
            bool? isSearch, bool? isBack,
            string strngaykycat, string strngaynhancat, int? inttinhtrangcat,
            int? intsodenbd, int? intsodenkt, string strngaynhanbd, string strngaynhankt,
            string strngaykybd, string strngaykykt, string strngayguibd, string strngayguikt,
            string strkyhieu, string strnoigui, string strtrichyeu,
            string truclienthong, string strmadinhdanh
            )
        {
            SearchVBDenDientuViewModel model = new SearchVBDenDientuViewModel();
            //===============================================
            // status
            //===============================================
            model.isSearch = isSearch == true ? true : false;
            model.isBack = isBack == true ? true : false;

            int intPage = 1;
            if (isBack == true)
            {   // tra ve page dang xem khi quay lai
                int _PageType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchPageList));
                if (_PageType == (int)EnumSession.PageType.VBDenDientu)
                {
                    intPage = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchPageListValues));
                }
            }
            model.intPage = intPage;

            if (isSearch == true)
            {
                // khi nhan nut tim kiem thi reset ve Nosearch 
                // de tim theo dieu kien moi
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            }
            //===============================================
            // category
            //===============================================          
            model.inttinhtrangcat = inttinhtrangcat;
            model.strngaykycat = strngaykycat;
            model.strngaynhancat = strngaynhancat;
            //===============================================
            // search
            //===============================================
            model.intsodenbd = intsodenbd;// != null ? (int)intsodenbd : 0;
            model.intsodenkt = intsodenkt;// != null ? (int)intsodenkt : 0;

            model.strngaykybd = strngaykybd;
            model.strngaykykt = strngaykykt;

            model.strngaynhanbd = strngaynhanbd;
            model.strngaynhankt = strngaynhankt;

            model.strngayguibd = strngayguibd;
            model.strngayguikt = strngayguikt;

            model.strkyhieu = strkyhieu;
            model.strnoigui = strnoigui;
            model.strtrichyeu = strtrichyeu;

            model.truclienthong = truclienthong;
            model.strmadinhdanh = strmadinhdanh;

            return PartialView(model);
        }

        public ActionResult Vanbanden_Read
            ([DataSourceRequest]DataSourceRequest request,
            //bool isSearch,            
            string strngaykycat, string strngaynhancat, int? inttinhtrangcat,
            int? intsodenbd, int? intsodenkt, string strngaynhanbd, string strngaynhankt,
            string strngaykybd, string strngaykykt, string strngayguibd, string strngayguikt,
            string strkyhieu, string strnoigui, string strtrichyeu,
            string truclienthong, string strmadinhdanh
            )
        {
            int currentPage = request.Page;

            // luu trang dang xem vao session
            _session.InsertObject(AppConts.SessionSearchPageList, EnumSession.PageType.VBDenDientu);
            _session.InsertObject(AppConts.SessionSearchPageListValues, currentPage);

            IEnumerable<ListVanbandendientuViewModel> vanban;

            int _SearchList = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchList));
            if (_SearchList == (int)EnumSession.SearchType.SearchVBDenDientu)
            {
                // lay cac gia tri search trong session
                string strSearchValues = _session.GetObject(AppConts.SessionSearchListValues).ToString();

                string _strngaykycat = _session.GetStringSearchValues("strngaykycat", strSearchValues);
                string _strngaynhancat = _session.GetStringSearchValues("strngaynhancat", strSearchValues);
                int _inttinhtrangcat = _session.GetIntSearchValues("inttinhtrangcat", strSearchValues);

                int _intsodenbd = _session.GetIntSearchValues("intsodenbd", strSearchValues);
                int _intsodenkt = _session.GetIntSearchValues("intsodenkt", strSearchValues);

                string _strngaynhanbd = _session.GetStringSearchValues("strngaynhanbd", strSearchValues);
                string _strngaynhankt = _session.GetStringSearchValues("strngaynhankt", strSearchValues);

                string _strngaykybd = _session.GetStringSearchValues("strngaykybd", strSearchValues);
                string _strngaykykt = _session.GetStringSearchValues("strngaykykt", strSearchValues);

                string _strngayguibd = _session.GetStringSearchValues("strngayguibd", strSearchValues);
                string _strngayguikt = _session.GetStringSearchValues("strngayguikt", strSearchValues);

                string _strkyhieu = _session.GetStringSearchValues("strkyhieu", strSearchValues);
                string _strtrichyeu = _session.GetStringSearchValues("strtrichyeu", strSearchValues);
                string _strnoigui = _session.GetStringSearchValues("strnoigui", strSearchValues);

                string _truclienthong = _session.GetStringSearchValues("truclienthong", strSearchValues);
                string _strmadinhdanh = _session.GetStringSearchValues("strmadinhdanh", strSearchValues);

                vanban = _vanban.GetListVanbandendientu
                    (_strngaykycat, _strngaynhancat, _inttinhtrangcat,
                    _intsodenbd, _intsodenkt, _strngaynhanbd, _strngaynhankt,
                    _strngaykybd, _strngaykykt, _strngayguibd, _strngayguikt,
                    _strkyhieu, _strnoigui, _strtrichyeu,
                    _truclienthong, _strmadinhdanh
                    );
            }
            else
            {
                // khong co luu tim kiem
                vanban = _vanban.GetListVanbandendientu
                    (strngaykycat, strngaynhancat, inttinhtrangcat,
                    intsodenbd, intsodenkt, strngaynhanbd, strngaynhankt,
                    strngaykybd, strngaykykt, strngayguibd, strngayguikt,
                    strkyhieu, strnoigui, strtrichyeu,
                    truclienthong, strmadinhdanh
                    );
            }

            return Json(vanban
                    .OrderByDescending(p => p.dtengayky)
                    .ThenByDescending(p => p.intso)
                    .ToDataSourceResult(request)
                    );
        }

        public ActionResult _ViewDetailVBDTDen()
        {
            return PartialView();
        }

        public ActionResult _XemChitietVanbanden(int id)
        {
            var model = _vanban.GetViewDetail(id);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult _NhanEmail()
        {
            ResultFunction kq = _vanban.NhanEmail();
            return Json(kq);
        }

        [HttpPost]
        public ActionResult _AutoReceiveMail()
        {
            ResultFunction kq = _vanban.AutoReceiveMail();
            return Json(kq);

        }

        public ActionResult _SearchVBDenDientu()
        {
            return PartialView();
        }

        public ActionResult _formDeleteVanban(int id)
        {
            DeleteVBViewModel model = _vanban.GetDeleteVanban(id);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult DeleteVanban(int id)
        {
            ResultFunction kq = _vanban.DeleteVanban(id);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json((int)kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }
        #endregion ViewIndex

        #region Export

        public FileResult Export([DataSourceRequest]DataSourceRequest request)
        {
            string strSearchValues = _session.GetObject(AppConts.SessionSearchListValues).ToString();

            string _strngaykycat = _session.GetStringSearchValues("strngaykycat", strSearchValues);
            string _strngaynhancat = _session.GetStringSearchValues("strngaynhancat", strSearchValues);
            int _inttinhtrangcat = _session.GetIntSearchValues("inttinhtrangcat", strSearchValues);

            int _intsodenbd = _session.GetIntSearchValues("intsodenbd", strSearchValues);
            int _intsodenkt = _session.GetIntSearchValues("intsodenkt", strSearchValues);

            string _strngaynhanbd = _session.GetStringSearchValues("strngaynhanbd", strSearchValues);
            string _strngaynhankt = _session.GetStringSearchValues("strngaynhankt", strSearchValues);

            string _strngaykybd = _session.GetStringSearchValues("strngaykybd", strSearchValues);
            string _strngaykykt = _session.GetStringSearchValues("strngaykykt", strSearchValues);

            string _strngayguibd = _session.GetStringSearchValues("strngayguibd", strSearchValues);
            string _strngayguikt = _session.GetStringSearchValues("strngayguikt", strSearchValues);

            string _strkyhieu = _session.GetStringSearchValues("strkyhieu", strSearchValues);
            string _strtrichyeu = _session.GetStringSearchValues("strtrichyeu", strSearchValues);
            string _strnoigui = _session.GetStringSearchValues("strnoigui", strSearchValues);

            string _truclienthong = _session.GetStringSearchValues("truclienthong", strSearchValues);
            string _strmadinhdanh = _session.GetStringSearchValues("strmadinhdanh", strSearchValues);

            var vanban = new List<ListVanbandendientuViewModel>(_vanban.GetListVanbandendientu
                (_strngaykycat, _strngaynhancat, _inttinhtrangcat,
                _intsodenbd, _intsodenkt, _strngaynhanbd, _strngaynhankt,
                _strngaykybd, _strngaykykt, _strngayguibd, _strngayguikt,
                _strkyhieu, _strnoigui, _strtrichyeu,
                _truclienthong, _strmadinhdanh
                ));

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                /* Create the worksheet. */

                SpreadsheetDocument spreadsheet = Excel.CreateWorkbook(stream);
                Excel.AddBasicStyles(spreadsheet);
                Excel.AddAdditionalStyles(spreadsheet);
                Excel.AddWorksheet(spreadsheet, "Văn bản đến điện tử");
                Worksheet worksheet = spreadsheet.WorkbookPart.WorksheetParts.First().Worksheet;

                //create columns and set their widths

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 1, "Ngày ký", false, false);
                Excel.SetColumnWidth(worksheet, 1, 15);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 2, "Số", false, false);
                Excel.SetColumnWidth(worksheet, 2, 10);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 3, "Ký hiệu", false, false);
                Excel.SetColumnWidth(worksheet, 3, 10);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 4, "Trích yếu", false, false);
                Excel.SetColumnWidth(worksheet, 4, 55);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 5, "Đơn vị gửi", false, false);
                Excel.SetColumnWidth(worksheet, 5, 40);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 6, "Ngày gửi", false, false);
                Excel.SetColumnWidth(worksheet, 6, 15);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 7, "Ngày nhận", false, false);
                Excel.SetColumnWidth(worksheet, 7, 15);


                /* Add the data to the worksheet. */

                // For each row of data...
                for (int idx = 0; idx < vanban.Count; idx++)
                {
                    // Set the field values in the spreadsheet for the current row.
                    string strngayky = DateServices.FormatDateVN(vanban[idx].dtengayky);
                    Excel.SetCellValue(spreadsheet, worksheet, 1, (uint)idx + 2, strngayky, false, false);

                    Excel.SetCellValue(spreadsheet, worksheet, 2, (uint)idx + 2, vanban[idx].intso.ToString(), false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 3, (uint)idx + 2, vanban[idx].strkyhieu, false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 4, (uint)idx + 2, vanban[idx].strtrichyeu, false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 5, (uint)idx + 2, vanban[idx].strnoiguivb, false, false);

                    string strngaygui = DateServices.FormatDateTimeVN(vanban[idx].dtengayguivb);
                    Excel.SetCellValue(spreadsheet, worksheet, 6, (uint)idx + 2, strngaygui, false, false);

                    string strngaynhan = DateServices.FormatDateTimeVN(vanban[idx].dtengaynhanvb);
                    Excel.SetCellValue(spreadsheet, worksheet, 7, (uint)idx + 2, strngaynhan, false, false);
                }

                /* Save the worksheet and store it in Session using the spreadsheet title. */

                worksheet.Save();
                spreadsheet.Close();

                return File(stream.ToArray(),   //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                "ExportVanbandendientu.xlsx");
            }
        }


        #endregion Export

        #region Tonghop

        public ActionResult Tonghop()
        {
            return View();
        }

        public ActionResult _TonghopVBDT(string strngaykybd, string strngaykykt,
            string danhmuc, int? intSoNgaygui, int LoaiTonghop)
        {
            var tonghop = _vanban.TonghopVBDen(strngaykybd, strngaykykt, danhmuc, intSoNgaygui, LoaiTonghop);

            return PartialView(tonghop);
        }

        [HttpPost]
        public ActionResult TonghopDonvi(string stremail, string danhmuc,
            string strngaykybd, string strngaykykt, int? intSoNgaygui, int LoaiTonghop)
        {
            ViewBag.stremail = stremail;
            ViewBag.strDonvi = _vanban.GetTenDonviDi(stremail, danhmuc);

            ViewBag.strngaykybd = strngaykybd;
            ViewBag.strngaykykt = strngaykykt;
            //ViewBag.intSoNgaygui = (intSoNgaygui >= 0) ? intSoNgaygui : 0;
            ViewBag.intSoNgaygui = intSoNgaygui;
            ViewBag.LoaiTonghop = LoaiTonghop;

            return View();
        }

        public ActionResult TonghopVBDonvi_Read(
           [DataSourceRequest]DataSourceRequest request,
          string stremail, string strngaykybd, string strngaykykt, int? intSoNgaygui, int LoaiTonghop
          )
        {
            var vbdi = _vanban.GetListVBDenDientuDonvi(stremail, strngaykybd, strngaykykt, intSoNgaygui, LoaiTonghop);

            DataSourceResult result = vbdi.OrderByDescending(p => p.dtengayky)
                                            .ThenByDescending(p => p.intso)
                                            .ToDataSourceResult(request);
            return Json(result);

        }
        [HttpPost]
        public ActionResult TonghopVBDonvi_Export(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        #endregion Tonghop

        #region Edxml

        [HttpPost]
        public ActionResult _NhanEdxml()
        {
            ResultFunction kq = _edxml.Receiver();
            //string file = "F:\\123.edxml";
            //int idmail = _edxml.ReadEdxml(file);
            return Json(kq);
        }

        #endregion Edxml
    
        #region Xml

        [HttpPost]
        public ActionResult _NhanXml()
        {
            ResultFunction kq = _edxml.Receiver();
            //string file = "F:\\123.edxml";
            //int idmail = _edxml.ReadEdxml(file);
            return Json(kq);
        }

        #endregion Xml


        #region Truc Lien Thong Tinh

        [HttpPost]
        public ActionResult _NhanVanbanTrucTinh()
        {
            return Json(_trucLienthongTinhManager.NhanVanBan());
        }

        #endregion Truc Lien Thong Tinh
    }
}