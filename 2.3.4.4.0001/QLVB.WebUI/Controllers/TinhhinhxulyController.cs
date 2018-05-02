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
using QLVB.DTO.Tinhhinhxuly;
using QLVB.Common.Date;
using DocumentFormat.OpenXml.Packaging;
using QLVB.WebUI.Common.OpenXML;
using DocumentFormat.OpenXml.Spreadsheet;
using QLVB.Common.Sessions;
using QLVB.Common.Utilities;

namespace QLVB.WebUI.Controllers
{
    public class TinhhinhxulyController : Controller
    {
        #region Constructor

        private ITinhhinhxulyManager _xuly;
        private ISessionServices _session;
        public TinhhinhxulyController(ITinhhinhxulyManager xuly, ISessionServices session)
        {
            _xuly = xuly;
            _session = session;
        }

        #endregion Constructor

        public ActionResult Index()
        {
            return RedirectToAction("Vanbanden", "Tinhhinhxuly");
           
        }

        #region Vanbandi

        public ActionResult Vanbandi(bool? isBack, string strngaybd, string strngaykt, LoaiNgay? loaingay) 
        {
            if (isBack == true)
            {
                var _SearchType = Convert.ToInt32(_session.GetObject(AppConts.SessionTonghopXuly));
                if (_SearchType == (int)EnumSession.TonghopXuly.Vanbandi)
                {
                    var strSearchValues = _session.GetObject(AppConts.SessionTonghopValues).ToString();

                    var _strngaybd = _session.GetStringSearchValues("strngaybd", strSearchValues);
                    var _strngaykt = _session.GetStringSearchValues("strngaykt", strSearchValues);
                    var _loaingay = _session.GetIntSearchValues("loaingay", strSearchValues);

                    try
                    {
                        strngaybd = _strngaybd;
                        strngaykt = _strngaykt;
                        loaingay = (LoaiNgay)_loaingay;
                    }
                    catch
                    {
                    }
                }
            }

            var model = new VanbandiViewModel
            {
                Loaingay = loaingay ?? LoaiNgay.NgayKy,

                Ngaybd = string.IsNullOrWhiteSpace(strngaybd)
                    ? DateTime.Now
                    : (DateServices.FormatDateEn(strngaybd) ?? DateTime.Now),

                Ngaykt = string.IsNullOrWhiteSpace(strngaykt)
                    ? DateTime.Now
                    : (DateServices.FormatDateEn(strngaykt) ?? DateTime.Now)
            };



            return View(model);
        }

        public ActionResult _TonghopVBDi(string strngaybd, string strngaykt, LoaiNgay loaingay)
        {

            var model = _xuly.TonghopVbDi(strngaybd, strngaykt, loaingay);
            return PartialView(model);
        }

        public ActionResult ListVanbandi(LoaiXuLyVbDi loaiXuLyVbDi, string donvi, string strngaybd,
            string strngaykt, LoaiNgay loaingay, bool? isBack)
        {
            if (isBack == true)
            {
                var searchType = Convert.ToInt32(_session.GetObject(AppConts.SessionTonghopXuly));
                if (searchType == (int)EnumSession.TonghopXuly.Vanbandi)
                {
                    var strSearchValues = _session.GetObject(AppConts.SessionTonghopValues).ToString();

                    var _strngaybd = _session.GetStringSearchValues("strngaybd", strSearchValues);
                    var _strngaykt = _session.GetStringSearchValues("strngaykt", strSearchValues);
                    var _donvi = _session.GetStringSearchValues("donvi", strSearchValues);
                    var _loaixuly = _session.GetIntSearchValues("loaixuly", strSearchValues);
                    var _loaingay = _session.GetIntSearchValues("loaingay", strSearchValues);

                    try
                    {
                        donvi = _donvi;
                        strngaybd = _strngaybd;
                        strngaykt = _strngaykt;
                        loaiXuLyVbDi = (LoaiXuLyVbDi) _loaixuly;
                        loaingay = (LoaiNgay) _loaingay;
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                var strSearchValues = string.Empty;
                strSearchValues += "strngaybd=" + strngaybd + ";strngaykt=" + strngaykt + ";";
                strSearchValues += "donvi=" + donvi + ";";
                strSearchValues += "loaingay=" + (int)loaingay + ";";
                strSearchValues += "loaixuly=" + (int)loaiXuLyVbDi + ";";

                // luu cac gia tri vao session
                _session.InsertObject(AppConts.SessionTonghopXuly, EnumSession.TonghopXuly.Vanbandi);
                _session.InsertObject(AppConts.SessionTonghopValues, strSearchValues);
            }

            //===============================================
            var model = new ListTonghopVbDiViewModel();
            ViewBag.intPage = 1;
            var strloaivanban = string.Empty;
            switch (loaiXuLyVbDi)
            {
                case LoaiXuLyVbDi.Dangxuly:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐANG XỬ LÝ";
                    break;
                case LoaiXuLyVbDi.Datiepnhan:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐÃ TIẾP NHẬN";
                    break;
                case LoaiXuLyVbDi.Hoanthanh:
                    strloaivanban = "DANH SÁCH VĂN BẢN HOÀN THÀNH";
                    break;
                case LoaiXuLyVbDi.Quahan:
                    strloaivanban = "DANH SÁCH VĂN BẢN QUÁ HẠN";
                    break;
                default:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐÃ GỬI";
                    break;
            }
            model.LoaiNgay = loaingay;
            model.LoaiVanBan = strloaivanban;
            model.Donvi = donvi;
            model.Ngaybd = strngaybd;
            model.Ngaykt = strngaykt;
            model.LoaiXuly = loaiXuLyVbDi;

            return View(model);
        }

        public ActionResult Vanbandi_Read(
            [DataSourceRequest]DataSourceRequest request,
            LoaiXuLyVbDi loaiXuLyVbDi, string donvi, string strngaybd, string strngaykt, LoaiNgay loaingay)
        {
            var currentPage = request.Page;

            // luu trang dang xem vao session
            _session.InsertObject(AppConts.SessionSearchPageType, EnumSession.PageType.TinhhinhVBDi);
            _session.InsertObject(AppConts.SessionSearchPageValues, currentPage);


            return Json(_xuly.GetListVanbandi(loaiXuLyVbDi, donvi, strngaybd, strngaykt, loaingay)
                .OrderByDescending(p => p.strnoinhan)
                .ThenByDescending(p => p.dtengayky)
                .ThenByDescending(p => p.intso)
                .ToDataSourceResult(request));
        }

        public FileResult ExportVanbanDi([DataSourceRequest]DataSourceRequest request,
            LoaiXuLyVbDi loaiXuLyVbDi, string donvi, string strngaybd, string strngaykt, LoaiNgay loaingay)
        {

            //Get the data representing the current grid state - page, sort and filter
            //var products = new List<Product>(db.Products.ToDataSourceResult(request).Data as IEnumerable<Product>);

            var vanban = _xuly.GetListVanbandi(loaiXuLyVbDi, donvi, strngaybd, strngaykt, loaingay)
                .OrderByDescending(p => p.strnoinhan)
                .ThenByDescending(p => p.dtengayky)
                .ThenByDescending(p => p.intso)
                .ToList();



            using (var stream = new System.IO.MemoryStream())
            {
                /* Create the worksheet. */

                var spreadsheet = Excel.CreateWorkbook(stream);
                Excel.AddBasicStyles(spreadsheet);
                Excel.AddAdditionalStyles(spreadsheet);
                Excel.AddWorksheet(spreadsheet, "Văn bản đi");
                var worksheet = spreadsheet.WorkbookPart.WorksheetParts.First().Worksheet;

                //create columns and set their widths

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 1, "Ngày ký", false, false);
                Excel.SetColumnWidth(worksheet, 1, 15);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 2, "Số", false, false);
                Excel.SetColumnWidth(worksheet, 2, 10);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 3, "Ký hiệu", false, false);
                Excel.SetColumnWidth(worksheet, 3, 15);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 4, "Trích yếu", false, false);
                Excel.SetColumnWidth(worksheet, 4, 50);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 5, "Nơi nhận", false, false);
                Excel.SetColumnWidth(worksheet, 5, 50);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 6, "Hạn trả lời", false, false);
                Excel.SetColumnWidth(worksheet, 6, 15);


                /* Add the data to the worksheet. */

                // For each row of data...
                for (int idx = 0; idx < vanban.Count; idx++)
                {
                    // Set the field values in the spreadsheet for the current row.
                    var strngayky = DateServices.FormatDateVN(vanban[idx].dtengayky);
                    Excel.SetCellValue(spreadsheet, worksheet, 1, (uint)idx + 2, strngayky, false, false);

                    Excel.SetCellValue(spreadsheet, worksheet, 2, (uint)idx + 2, vanban[idx].intso.ToString(), false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 3, (uint)idx + 2, vanban[idx].strkyhieu, false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 4, (uint)idx + 2, vanban[idx].strtrichyeu, false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 5, (uint)idx + 2, vanban[idx].strnoinhan, false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 6, (uint)idx + 2, string.Format("{0:dd/MM/yyyy}",vanban[idx].dtehanxuly), false, false);
                }

                /* Save the worksheet and store it in Session using the spreadsheet title. */

                worksheet.Save();
                spreadsheet.Close();

                return File(stream.ToArray(),   //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                "ExportVanbandi.xlsx");
            }
        }

        #endregion

        #region Vanbanden
        public ActionResult Vanbanden(bool? isBack,
            int? iddonvi, string strngaybd, string strngaykt, int? idloaingay, int? idsovb)
        {
            ListdonviViewModel model = new ListdonviViewModel();
            if (isBack == true)
            {
                int _SearchType = Convert.ToInt32(_session.GetObject(AppConts.SessionTonghopXuly));
                if (_SearchType == (int)EnumSession.TonghopXuly.Vanbanden)
                {
                    string strSearchValues = _session.GetObject(AppConts.SessionTonghopValues).ToString();

                    string _strngaybd = _session.GetStringSearchValues("strngaybd", strSearchValues);
                    string _strngaykt = _session.GetStringSearchValues("strngaykt", strSearchValues);
                    int _iddonvi = _session.GetIntSearchValues("iddonvi", strSearchValues);
                    int _idloaingay = _session.GetIntSearchValues("idloaingay", strSearchValues);
                    int _idsovb = _session.GetIntSearchValues("idsovb", strSearchValues);

                    try
                    {
                        iddonvi = _iddonvi;
                        strngaybd = _strngaybd;
                        strngaykt = _strngaykt;
                        idloaingay = _idloaingay;
                        idsovb = _idsovb;
                    }
                    catch
                    {
                    }
                }
            }

            model = _xuly.GetListDonvi(iddonvi, strngaybd, strngaykt, idloaingay, idsovb);

            return View(model);
        }

        public ActionResult _TonghopVBDen(int iddonvi, string strngaybd, string strngaykt, int idloaingay, int idsovb)
        {
           
            var model = _xuly.TonghopVBDen(iddonvi, strngaybd, strngaykt, idloaingay, idsovb);
            return PartialView(model);
        }
        //=====================================================
        // List van ban den
        //=====================================================
        public ActionResult ListVanbanDen(int? intloai, int? iddonvi, int? idcanbo, 
            string strngaybd, string strngaykt,
            int? idloaingay, int? idsovb, 
            bool? isBack)
        {
            if (isBack==true)
            {
                int _SearchType = Convert.ToInt32(_session.GetObject(AppConts.SessionTonghopXuly));
                if (_SearchType == (int)EnumSession.TonghopXuly.Vanbanden)
                {
                    string strSearchValues = _session.GetObject(AppConts.SessionTonghopValues).ToString();

                    string _strngaybd = _session.GetStringSearchValues("strngaybd", strSearchValues);
                    string _strngaykt = _session.GetStringSearchValues("strngaykt", strSearchValues);
                    int _iddonvi = _session.GetIntSearchValues("iddonvi", strSearchValues);
                    int _idloaingay = _session.GetIntSearchValues("idloaingay", strSearchValues);
                    int _idsovb = _session.GetIntSearchValues("idsovb", strSearchValues);
                    int _intloai = _session.GetIntSearchValues("intloai", strSearchValues);
                    int _idcanbo = _session.GetIntSearchValues("idcanbo", strSearchValues);

                    try
                    {
                        iddonvi = _iddonvi;
                        strngaybd = _strngaybd;
                        strngaykt = _strngaykt;
                        idloaingay = _idloaingay;
                        idsovb = _idsovb;
                        intloai = _intloai;
                        idcanbo = _idcanbo;
                    }
                    catch
                    {
                    }
                }
            }
            else
            {             
                string strSearchValues = string.Empty;
                strSearchValues += "strngaybd=" + strngaybd + ";strngaykt=" + strngaykt + ";";
                strSearchValues += "iddonvi=" + iddonvi.ToString() + ";";
                strSearchValues += "idloaingay=" + idloaingay.ToString() + ";";
                strSearchValues += "idsovb=" + idsovb.ToString() + ";";
                strSearchValues += "intloai=" + intloai.ToString() + ";";
                strSearchValues += "idcanbo=" + idcanbo.ToString() + ";";

                // luu cac gia tri vao session
                _session.InsertObject(AppConts.SessionTonghopXuly, EnumSession.TonghopXuly.Vanbanden);
                _session.InsertObject(AppConts.SessionTonghopValues, strSearchValues);
            }
          
            //===============================================
            ListTonghopVBDenViewModel model = new ListTonghopVBDenViewModel();
            ViewBag.intPage = 1;
            string strloaivanban = string.Empty;
            switch (intloai)
            {
                case (int)enumtinhtrangxuly.intloai.LuuHS:
                    strloaivanban = "DANH SÁCH VĂN BẢN LƯU HỒ SƠ";
                    break;
                case (int)enumtinhtrangxuly.intloai.DaXL:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐÃ HOÀN THÀNH XỬ LÝ";
                    break;
                case (int)enumtinhtrangxuly.intloai.DangXL:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐANG XỬ LÝ";
                    break;
                case (int)enumtinhtrangxuly.intloai.QuahanXL:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐÃ QUÁ HẠN XỬ LÝ";
                    break;
                case (int)enumtinhtrangxuly.intloai.Trinhky:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐANG TRÌNH KÝ";
                    break;
                //================tong cong =============
                case (int)enumtinhtrangxuly.intloai.TongLuuHS:
                    strloaivanban = "DANH SÁCH VĂN BẢN LƯU HỒ SƠ";
                    break;
                case (int)enumtinhtrangxuly.intloai.TongDaXL:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐÃ HOÀN THÀNH XỬ LÝ";
                    break;
                case (int)enumtinhtrangxuly.intloai.TongDangXL:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐANG XỬ LÝ";
                    break;
                case (int)enumtinhtrangxuly.intloai.TongQuahanXL:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐÃ QUÁ HẠN XỬ LÝ";
                    break;
                case (int)enumtinhtrangxuly.intloai.TongTrinhKy:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐANG TRÌNH KÝ";
                    break;
            }
            model.intloai = (int)intloai;
            model.strloaivanban = strloaivanban;
            model.idcanbo = (int)idcanbo;
            model.iddonvi = (int)iddonvi;
            model.strngaybd = strngaybd;
            model.strngaykt = strngaykt;
            model.idloaingay = idloaingay;
            model.idsovb = idsovb;

            return View(model);
        }

        public ActionResult Vanbanden_Read(
            [DataSourceRequest]DataSourceRequest request,
            int intloai, int idcanbo, int iddonvi,
            string strngaybd, string strngaykt, int idloaingay,
            int idsovb
            )
        {
            int currentPage = request.Page;

            // luu trang dang xem vao session
            _session.InsertObject(AppConts.SessionSearchPageType, EnumSession.PageType.TinhhinhVBDen);
            _session.InsertObject(AppConts.SessionSearchPageValues, currentPage);


            return Json(_xuly.GetListVanbanden
                    (intloai, idcanbo, iddonvi,
                     strngaybd, strngaykt, idloaingay, idsovb
                    )
                    .OrderByDescending(p => p.strnoinhan)
                    .ThenByDescending(p => p.dtengayden)
                    .ThenByDescending(p => p.intsoden)
                    .ToDataSourceResult(request));
        }

        public FileResult ExportVBDen([DataSourceRequest]DataSourceRequest request,
            int intloai, int idcanbo, int iddonvi, string strngaybd, string strngaykt, int idloaingay, int idsovb)
        {

            //Get the data representing the current grid state - page, sort and filter
            //var products = new List<Product>(db.Products.ToDataSourceResult(request).Data as IEnumerable<Product>);

            var vanban = new List<QLVB.DTO.Vanbanden.ListVanbandenViewModel>();

            vanban = _xuly.GetListVanbanden
                    (intloai, idcanbo, iddonvi,
                     strngaybd, strngaykt, idloaingay, idsovb
                    )
                    .OrderByDescending(p => p.strnoinhan)
                    .ThenByDescending(p => p.dtengayden)
                    .ThenByDescending(p => p.intsoden)
                    .ToList();



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

        public ActionResult _PieChartXLVBDen(string xuly)
        {
            List<ChartCommonViewModel> model = new List<ChartCommonViewModel>();

            dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(xuly);

            int intVitri = 0;
            int intTong = 0;
            string strBaocao = string.Empty;

            foreach (var j in json)
            {
                // chi lay tu vi tri thu 2 : 
                // dem, ten, luuhs, daxl,dangxuly,trinhky,quahan,tongcong
                var t = j.ToString();

                switch (intVitri)
                {
                    case 0: // stt
                        break;
                    case 1: // ten
                        strBaocao = t;
                        break;
                    case 2: // luu hs
                        ChartCommonViewModel chart = new ChartCommonViewModel();
                        chart.strGiatri = t;
                        chart.strMota = "Lưu Hồ sơ";
                        chart.strColor = "#838383";
                        model.Add(chart);
                        break;
                    case 3: // daxl
                        chart = new ChartCommonViewModel();
                        chart.strGiatri = t;
                        chart.strMota = "Đã Xử lý";
                        chart.strColor = "#414141";
                        model.Add(chart);
                        break;
                    case 4: // dang xl
                        chart = new ChartCommonViewModel();
                        chart.strGiatri = t;
                        chart.strMota = "Đang Xử lý";
                        chart.strColor = "#3f51b5";
                        model.Add(chart);
                        break;
                    case 5: // trinh ky
                        chart = new ChartCommonViewModel();
                        chart.strGiatri = t;
                        chart.strMota = "Đang Trình ký";
                        chart.strColor = "#03a9f4";
                        model.Add(chart);
                        break;
                    case 6: // qua han
                        chart = new ChartCommonViewModel();
                        chart.strGiatri = t;
                        chart.strMota = "Quá hạn xử lý";
                        chart.strColor = "#FF0000";
                        model.Add(chart);
                        break;
                    case 7: // tong
                        intTong = Convert.ToInt32(t);
                        break;
                }
                intVitri++;
            }

            if (intTong > 0)
            {
                foreach (var m in model)
                {
                    m.strMota = m.strMota.ToUpper();
                    int intGiatri = Convert.ToInt32(m.strGiatri);
                    if (intGiatri > 0)
                    {
                        double percent = (double)(intGiatri * 100) / intTong;
                        double percentage = Math.Round(percent, 1);
                        m.strGiatri = percentage.ToString();
                    }
                }
            }
            else
            {
                model = null;
            }


            ViewBag.strBaocao = strBaocao;

            return PartialView(model);
        }


        #endregion Vanbanden
 
        #region Quytrinh

        public ActionResult Quytrinh(
            bool? isBack, int? idloaiquytrinh, int? idquytrinh,
            int? iddonvi, string strngaybd, string strngaykt)
        {
            //ListdonviViewModel model = new ListdonviViewModel();
            ListQuytrinhViewModel model = new ListQuytrinhViewModel();
            if (isBack == true)
            {
                int _SearchType = Convert.ToInt32(_session.GetObject(AppConts.SessionTonghopXuly));
                if (_SearchType == (int)EnumSession.TonghopXuly.Quytrinh)
                {
                    string strSearchValues = _session.GetObject(AppConts.SessionTonghopValues).ToString();

                    string _strngaybd = _session.GetStringSearchValues("strngaybd", strSearchValues);
                    string _strngaykt = _session.GetStringSearchValues("strngaykt", strSearchValues);
                    int _iddonvi = _session.GetIntSearchValues("iddonvi", strSearchValues);
                    int _idquytrinh = _session.GetIntSearchValues("idquytrinh", strSearchValues);
                    int _idloaiquytrinh = _session.GetIntSearchValues("idloaiquytrinh", strSearchValues);

                    try
                    {
                        idquytrinh = _idquytrinh;
                        iddonvi = _iddonvi;
                        idloaiquytrinh = _idloaiquytrinh;
                        strngaybd = _strngaybd;
                        strngaykt = _strngaykt;
                    }
                    catch
                    {
                    }
                }
            }

            model = _xuly.GetListQuytrinh(idloaiquytrinh, idquytrinh, strngaybd, strngaykt);

            return View(model);
        }

        public JsonResult GetQuytrinh(int? idloai)
        {
            if (idloai != null)
            {
                var quytrinh = _xuly.GetQuytrinh((int)idloai);
                return Json(quytrinh, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public ActionResult _TonghopQuytrinh(
            int? idloaiquytrinh, int? idquytrinh, int? iddonvi,
            string strngaybd, string strngaykt)
        {

            _session.InsertObject("strngaybd", strngaybd);
            _session.InsertObject("strngaykt", strngaykt);
            _session.InsertObject("iddonvi", iddonvi);
            _session.InsertObject("idquytrinh", idquytrinh);
            _session.InsertObject("idloaiquytrinh", idloaiquytrinh);

            var model = _xuly.TonghopQuytrinh(idloaiquytrinh, idquytrinh, iddonvi, strngaybd, strngaykt);
            return PartialView(model);
        }
        public ActionResult ListQuytrinh(int intloai, int idquytrinh, int idloaiquytrinh, string strngaybd, string strngaykt)
        {
            ListTonghopVBDenViewModel model = new ListTonghopVBDenViewModel();
            ViewBag.intPage = 1;
            string strloaivanban = string.Empty;
            switch (intloai)
            {
                case (int)enumtinhtrangquytrinh.intloai.DaXL_Dunghan:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐÃ XỬ LÝ ĐÚNG HẠN";
                    break;
                case (int)enumtinhtrangquytrinh.intloai.DaXL_Trehan:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐÃ XỬ LÝ TRỄ HẠN";
                    break;
                case (int)enumtinhtrangquytrinh.intloai.DangXL:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐANG XỬ LÝ";
                    break;
                case (int)enumtinhtrangquytrinh.intloai.QuahanXL:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐÃ QUÁ HẠN XỬ LÝ";
                    break;
            }
            model.intloai = intloai;
            model.strloaivanban = strloaivanban;
            model.idloaiquytrinh = idloaiquytrinh;
            model.idquytrinh = idquytrinh;
            model.strngaybd = strngaybd;
            model.strngaykt = strngaykt;

            return View(model);
        }

        public ActionResult _ChitietQuytrinh(string NodeId, int intloai, int idquytrinh, int? idloaiquytrinh, string strngaybd, string strngaykt)
        {
            ListTonghopVBDenViewModel model = new ListTonghopVBDenViewModel();
            ViewBag.intPage = 1;
            string strloaivanban = string.Empty;
            switch (intloai)
            {
                case (int)enumtinhtrangquytrinh.intloai.DaXL_Dunghan:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐÃ XỬ LÝ ĐÚNG HẠN";
                    break;
                case (int)enumtinhtrangquytrinh.intloai.DaXL_Trehan:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐÃ XỬ LÝ TRỄ HẠN";
                    break;
                case (int)enumtinhtrangquytrinh.intloai.DangXL:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐANG XỬ LÝ";
                    break;
                case (int)enumtinhtrangquytrinh.intloai.QuahanXL:
                    strloaivanban = "DANH SÁCH VĂN BẢN ĐÃ QUÁ HẠN XỬ LÝ";
                    break;
            }
            model.intloai = intloai;
            model.strloaivanban = strloaivanban;
            model.idloaiquytrinh = idloaiquytrinh;
            model.idquytrinh = idquytrinh;
            model.strngaybd = strngaybd;
            model.strngaykt = strngaykt;

            model.NodeId = NodeId;

            return PartialView(model);
        }


        public ActionResult QuytrinhVBDen_Read(
            [DataSourceRequest]DataSourceRequest request,
            int? idquytrinh, string NodeId,
            int intloai, int? idloaiquytrinh,
            string strngaybd, string strngaykt
            )
        {
            int currentPage = request.Page;

            // luu trang dang xem vao session
            _session.InsertObject(AppConts.SessionSearchPageType, EnumSession.PageType.TinhhinhQuytrinh);
            _session.InsertObject(AppConts.SessionSearchPageValues, currentPage);


            return Json(_xuly.GetListQuytrinhVBDen
                    (idquytrinh, intloai, idloaiquytrinh,
                     strngaybd, strngaykt, NodeId
                    )
                    .OrderByDescending(p => p.dtengayden)
                    .ThenByDescending(p => p.intsoden)
                    .ToDataSourceResult(request));
        }

        public ActionResult _XemQuytrinh(int idquytrinh, int intloai, string strngaybd, string strngaykt)
        {
            ViewBag.jsFlowchart = _xuly.XemTonghopQuytrinhFlowchart(idquytrinh, intloai, strngaybd, strngaykt);
            return PartialView();
        }

        public FileResult ExportVBDenQuytrinh(
             [DataSourceRequest]DataSourceRequest request,
            int? idquytrinh, string NodeId,
            int intloai, int? idloaiquytrinh,
            string strngaybd, string strngaykt
            )
        {

            //Get the data representing the current grid state - page, sort and filter
            //var products = new List<Product>(db.Products.ToDataSourceResult(request).Data as IEnumerable<Product>);

            var vanban = new List<QLVB.DTO.Vanbanden.ListVanbandenViewModel>();

            vanban = _xuly.GetListQuytrinhVBDen
                    (idquytrinh, intloai, idloaiquytrinh,
                     strngaybd, strngaykt, NodeId
                    )
                    .OrderByDescending(p => p.dtengayden)
                    .ThenByDescending(p => p.intsoden).ToList();



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

        public ActionResult _PieChartXLQT(string xuly)
        {
            List<ChartCommonViewModel> model = new List<ChartCommonViewModel>();

            dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(xuly);

            int intVitri = 0;
            int intTong = 0;
            string strBaocao = string.Empty;

            foreach (var j in json)
            {
                // chi lay tu vi tri thu 2 : 
                // dem, ten, luuhs, daxl,dangxuly,trinhky,quahan,tongcong
                var t = j.ToString();

                switch (intVitri)
                {
                    case 0: // stt
                        break;
                    case 1: // ten
                        strBaocao = t;
                        break;
                    case 2: // daxl dung han
                        ChartCommonViewModel chart = new ChartCommonViewModel();
                        chart.strGiatri = t;
                        chart.strMota = "Đã Xử lý đúng hạn";
                        chart.strColor = "#838383";
                        model.Add(chart);
                        break;
                    case 3: // daxl tre han
                        chart = new ChartCommonViewModel();
                        chart.strGiatri = t;
                        chart.strMota = "Đã Xử lý trễ hạn";
                        chart.strColor = "#414141";
                        model.Add(chart);
                        break;
                    case 4: // dang xl
                        chart = new ChartCommonViewModel();
                        chart.strGiatri = t;
                        chart.strMota = "Đang Xử lý";
                        chart.strColor = "#3f51b5";
                        model.Add(chart);
                        break;
                    case 5: // qua han
                        chart = new ChartCommonViewModel();
                        chart.strGiatri = t;
                        chart.strMota = "Quá hạn xử lý";
                        chart.strColor = "#FF0000";
                        model.Add(chart);
                        break;
                    case 6: // tong
                        intTong = Convert.ToInt32(t);
                        break;
                }
                intVitri++;
            }

            if (intTong > 0)
            {
                foreach (var m in model)
                {
                    m.strMota = m.strMota.ToUpper();
                    int intGiatri = Convert.ToInt32(m.strGiatri);
                    if (intGiatri > 0)
                    {
                        double percent = (double)(intGiatri * 100) / intTong;
                        double percentage = Math.Round(percent, 1);
                        m.strGiatri = percentage.ToString();
                    }
                }
            }
            else
            {
                model = null;
            }


            ViewBag.strBaocao = strBaocao;

            return PartialView(model);
        }
        #endregion Quytrinh



    }
}