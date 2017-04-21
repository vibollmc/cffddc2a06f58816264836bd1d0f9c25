using QLVB.Common.Sessions;
using QLVB.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Core.Contract;
using QLVB.DTO.Vanbanden;
using QLVB.Common.Utilities;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using QLVB.Domain.Entities;
using DocumentFormat.OpenXml.Packaging;
using QLVB.WebUI.Common.OpenXML;
using DocumentFormat.OpenXml.Spreadsheet;
using QLVB.Common.Date;

namespace QLVB.WebUI.Controllers.Store
{
    public class TracuuVanbandenController : Controller
    {
        #region Constructor

        private ISessionServices _session;
        private ITracuuVanbandenManager _vanban;

        public TracuuVanbandenController(ISessionServices session, ITracuuVanbandenManager tracuu)
        {
            _vanban = tracuu;
            _session = session;
        }
        #endregion Contructor

        #region ViewIndex

        public ActionResult Index(bool? isBack)
        {

            if (isBack == true)
            {
                // luu session tim kiem
                //SessionService.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchVBDen);
            }
            else
            {   // khong co isBack (nhan vao menu vanbanden)
                // thi reset session ve Nosearch
                isBack = false;
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            }
            ViewBag.isBack = isBack;

            return View();
        }

        [ChildActionOnly]
        public ActionResult _Toolbar()
        {
            return PartialView();
        }


        public ActionResult _ListVanbanden
            (bool? isSearch, bool? isBack,
            string strngaydencat, int? idloaivb, int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly
            )
        {
            SearchVBViewModel model = new SearchVBViewModel();
            //===============================================
            // status
            //===============================================
            model.isSearch = isSearch == true ? true : false;
            model.isBack = isBack == true ? true : false;

            int intPage = 1;
            //if (isBack == true)
            //{   // tra ve page dang xem khi quay lai
            //    int _PageType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchPageType));
            //    if (_PageType == (int)EnumSession.PageType.VBDen)
            //    {
            //        intPage = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchPageValues));
            //    }
            //}
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
            model.idsovb = idsovb;//!= null ? (int)idsovb : 0;
            model.idloaivb = idloaivb;//!= null ? (int)idloaivb : 0;
            model.idkhoiph = idkhoiph;// != null ? (int)idkhoiph : 0;
            model.strngaydencat = strngaydencat;
            model.xuly = xuly;
            //===============================================
            // search
            //===============================================
            model.intsodenbd = intsodenbd;// != null ? (int)intsodenbd : 0;
            model.intsodenkt = intsodenkt;// != null ? (int)intsodenkt : 0;

            model.strngaydenbd = strngaydenbd;
            model.strngaydenkt = strngaydenkt;

            model.strngaykybd = strngaykybd;
            model.strngaykykt = strngaykykt;

            model.strsokyhieu = strsokyhieu;
            model.strnguoiky = strnguoiky;
            model.strnoigui = strnoigui;

            model.strtrichyeu = strtrichyeu;
            model.strnguoixuly = strnguoixuly;

            return PartialView(model);

        }


        public ActionResult Vanbanden_Read
            ([DataSourceRequest]DataSourceRequest request,
            //bool isSearch,
            string strngaydencat, int? idloaivb, int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly
            )
        {
            int currentPage = request.Page;
            string highlightResult = string.Empty;
            // luu trang dang xem vao session
            //_session.InsertObject(AppConts.SessionSearchPageType, EnumSession.PageType.VBDen);
            //_session.InsertObject(AppConts.SessionSearchPageValues, currentPage);

            //if (currentPage != 1)
            //{
            //    // luu trang dang xem vao session
            //    _session.InsertObject(AppConts.SessionPageType, EnumSession.PageType.VBDen);
            //    _session.InsertObject(AppConts.SessionPageValues, currentPage);
            //}

            int _SearchType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchType));

            IEnumerable<ListVanbandenViewModel> vbden;

            if (_SearchType == (int)EnumSession.SearchType.SearchTracuuVBDen)
            {
                // lay cac gia tri search trong session
                string strSearchValues = _session.GetObject(AppConts.SessionSearchTypeValues).ToString();

                string _strngaydencat = _session.GetStringSearchValues("strngaydencat", strSearchValues);
                int _idkhoiph = _session.GetIntSearchValues("idkhoiph", strSearchValues);
                int _idloaivb = _session.GetIntSearchValues("idloaivb", strSearchValues);
                int _idsovb = _session.GetIntSearchValues("idsovb", strSearchValues);
                string _xuly = _session.GetStringSearchValues("xuly", strSearchValues);

                int _intsodenbd = _session.GetIntSearchValues("intsodenbd", strSearchValues);
                int _intsodenkt = _session.GetIntSearchValues("intsodenkt", strSearchValues);

                string _strngaydenbd = _session.GetStringSearchValues("strngaydenbd", strSearchValues);
                string _strngaydenkt = _session.GetStringSearchValues("strngaydenkt", strSearchValues);

                string _strngaykybd = _session.GetStringSearchValues("strngaykybd", strSearchValues);
                string _strngaykykt = _session.GetStringSearchValues("strngaykykt", strSearchValues);

                string _strsokyhieu = _session.GetStringSearchValues("strsokyhieu", strSearchValues);
                string _strtrichyeu = _session.GetStringSearchValues("strtrichyeu", strSearchValues);
                string _strnguoixuly = _session.GetStringSearchValues("strnguoixuly", strSearchValues);
                string _strnguoiky = _session.GetStringSearchValues("strnguoiky", strSearchValues);
                string _strnoigui = _session.GetStringSearchValues("strnoigui", strSearchValues);

                highlightResult = _strtrichyeu;

                vbden = _vanban.GetListVanbanden
                    (_strngaydencat, _idloaivb,
                    _idkhoiph, _idsovb, _xuly,
                    _intsodenbd, _intsodenkt, _strngaydenbd, _strngaydenkt,
                    _strngaykybd, _strngaykykt, _strsokyhieu, _strnguoiky,
                    _strnoigui, _strtrichyeu, _strnguoixuly
                    );
            }
            else
            {
                highlightResult = strtrichyeu;
                // khong co luu tim kiem
                vbden = _vanban.GetListVanbanden
                    (strngaydencat, idloaivb,
                    idkhoiph, idsovb, xuly,
                    intsodenbd, intsodenkt, strngaydenbd, strngaydenkt,
                    strngaykybd, strngaykykt, strsokyhieu, strnguoiky,
                    strnoigui, strtrichyeu, strnguoixuly
                    );
            }

            DataSourceResult result = vbden.OrderByDescending(p => p.dtengayden)
                                            .ThenByDescending(p => p.intsoden)
                                            .ToDataSourceResult(request);



            return Json(result);
        }

        [OutputCache(Duration = 1000, VaryByParam = "none")]
        public ActionResult _SearchVBDen()
        {
            SearchVBViewModel model = _vanban.GetViewSearch();
            return PartialView(model);
        }



        #endregion ViewIndex

        #region ViewDetail

        public ActionResult _ViewDetailVBDen()
        {
            return PartialView();
        }

        public ActionResult _XemChitietVanban(int id)
        {
            DetailVBDenViewModel model = new DetailVBDenViewModel();
            if (id != 0)
            {
                model = _vanban.GetViewDetail(id);
            }
            return PartialView(model);
        }

        #endregion ViewDetail

        #region ExportGridToExcel

        public FileResult Export([DataSourceRequest]DataSourceRequest request)
        {

            //Get the data representing the current grid state - page, sort and filter
            //var products = new List<Product>(db.Products.ToDataSourceResult(request).Data as IEnumerable<Product>);

            var vanban = new List<ListVanbandenViewModel>();


            // lay cac gia tri search trong session
            string strSearchValues = _session.GetObject(AppConts.SessionSearchTypeValues).ToString();

            string _strngaydencat = _session.GetStringSearchValues("strngaydencat", strSearchValues);
            int _idkhoiph = _session.GetIntSearchValues("idkhoiph", strSearchValues);
            int _idloaivb = _session.GetIntSearchValues("idloaivb", strSearchValues);
            int _idsovb = _session.GetIntSearchValues("idsovb", strSearchValues);

            int _intsodenbd = _session.GetIntSearchValues("intsodenbd", strSearchValues);
            int _intsodenkt = _session.GetIntSearchValues("intsodenkt", strSearchValues);

            string _strngaydenbd = _session.GetStringSearchValues("strngaydenbd", strSearchValues);
            string _strngaydenkt = _session.GetStringSearchValues("strngaydenkt", strSearchValues);

            string _strngaykybd = _session.GetStringSearchValues("strngaykybd", strSearchValues);
            string _strngaykykt = _session.GetStringSearchValues("strngaykykt", strSearchValues);

            string _strsokyhieu = _session.GetStringSearchValues("strsokyhieu", strSearchValues);
            string _strtrichyeu = _session.GetStringSearchValues("strtrichyeu", strSearchValues);
            string _strnguoixuly = _session.GetStringSearchValues("strnguoixuly", strSearchValues);
            string _strnguoiky = _session.GetStringSearchValues("strnguoiky", strSearchValues);
            string _strnoigui = _session.GetStringSearchValues("strnoigui", strSearchValues);

            vanban = new List<ListVanbandenViewModel>(_vanban.GetListVanbanden
                    (_strngaydencat, _idloaivb,
                    _idkhoiph, _idsovb, "",
                    _intsodenbd, _intsodenkt, _strngaydenbd, _strngaydenkt,
                    _strngaykybd, _strngaykykt, _strsokyhieu, _strnguoiky,
                    _strnoigui, _strtrichyeu, _strnguoixuly
                    ));



            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                /* Create the worksheet. */

                SpreadsheetDocument spreadsheet = Excel.CreateWorkbook(stream);
                Excel.AddBasicStyles(spreadsheet);
                Excel.AddAdditionalStyles(spreadsheet);
                Excel.AddWorksheet(spreadsheet, "Văn bản đến");
                Worksheet worksheet = spreadsheet.WorkbookPart.WorksheetParts.First().Worksheet;

                //create columns and set their widths

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 1, "Ngày đến", false, false);
                Excel.SetColumnWidth(worksheet, 1, 15);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 2, "Số đến", false, false);
                Excel.SetColumnWidth(worksheet, 2, 10);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 3, "Nơi gửi", false, false);
                Excel.SetColumnWidth(worksheet, 3, 40);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 4, "Số ký hiệu", false, false);
                Excel.SetColumnWidth(worksheet, 4, 15);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 5, "Trích yếu", false, false);
                Excel.SetColumnWidth(worksheet, 5, 50);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 6, "Xử lý chính", false, false);
                Excel.SetColumnWidth(worksheet, 6, 30);


                /* Add the data to the worksheet. */

                // For each row of data...
                for (int idx = 0; idx < vanban.Count; idx++)
                {
                    // Set the field values in the spreadsheet for the current row.
                    string strngayden = DateServices.FormatDateVN(vanban[idx].dtengayden);
                    Excel.SetCellValue(spreadsheet, worksheet, 1, (uint)idx + 2, strngayden, false, false);

                    Excel.SetCellValue(spreadsheet, worksheet, 2, (uint)idx + 2, vanban[idx].intsoden.ToString(), false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 3, (uint)idx + 2, vanban[idx].strnoiphathanh, false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 4, (uint)idx + 2, vanban[idx].strkyhieu, false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 5, (uint)idx + 2, vanban[idx].strtrichyeu, false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 6, (uint)idx + 2, vanban[idx].strnoinhan, false, false);
                }

                /* Save the worksheet and store it in Session using the spreadsheet title. */

                worksheet.Save();
                spreadsheet.Close();

                return File(stream.ToArray(),   //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                "ExportVanbanden.xlsx");
            }
        }

        #endregion ExportGridToExcel

        public ActionResult Tracuu()
        {
            return View();
        }

    }
}