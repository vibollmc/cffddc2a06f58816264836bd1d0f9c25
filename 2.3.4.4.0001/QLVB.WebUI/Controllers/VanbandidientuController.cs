using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using QLVB.Common.Sessions;
using QLVB.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.DTO.Vanbandientu;
using QLVB.Common.Utilities;
using DocumentFormat.OpenXml.Packaging;
using QLVB.WebUI.Common.OpenXML;
using DocumentFormat.OpenXml.Spreadsheet;
using QLVB.Common.Date;
using QLVB.DTO;
using QLVB.DTO.Hethong;
using System.Net.Http;
using System.Threading.Tasks;


namespace QLVB.WebUI.Controllers
{
    public class VanbandidientuController : Controller
    {
        #region Constructor

        private IVanbandientuManager _vanban;
        private ISessionServices _session;
        private ITinhhinhXulyVanbanDiManager _tinhhinhXulyVanbanDiManager;
        public VanbandidientuController(IVanbandientuManager vanban, ISessionServices session, ITinhhinhXulyVanbanDiManager tinhhinhXulyVanbanDiManager)
        {
            _vanban = vanban;
            _session = session;
            _tinhhinhXulyVanbanDiManager = tinhhinhXulyVanbanDiManager;
        }

        #endregion Constructor

        #region Index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Category()
        {
            ViewBag.Songayhienthi = _vanban.GetSongayhienthi();
            return PartialView();
        }

        public ActionResult _ListVanbandidientu(
            bool? isSearch, bool? isBack,
            string strngaykycat, string strngaynhancat, string strDonviguicat,
            int? intsodibd, int? intsodikt, string strngaynhanbd, string strngaynhankt,
            string strngaykybd, string strngaykykt, string strngayguibd, string strngayguikt,
            string strkyhieu, string strnoigui, string strtrichyeu
            )
        {
            SearchVBDiDientuViewModel model = new SearchVBDiDientuViewModel();
            //===============================================
            // status
            //===============================================
            model.isSearch = isSearch == true ? true : false;
            model.isBack = isBack == true ? true : false;

            int intPage = 1;
            if (isBack == true)
            {   // tra ve page dang xem khi quay lai
                int _PageType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchPageType));
                if (_PageType == (int)EnumSession.PageType.VBDiDientu)
                {
                    intPage = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchPageValues));
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
            model.strDonviguicat = strDonviguicat;
            model.strngaykycat = strngaykycat;
            model.strngaynhancat = strngaynhancat;
            //===============================================
            // search
            //===============================================
            model.intsodibd = intsodibd;
            model.intsodikt = intsodikt;

            model.strngaykybd = strngaykybd;
            model.strngaykykt = strngaykykt;

            model.strngaynhanbd = strngaynhanbd;
            model.strngaynhankt = strngaynhankt;

            model.strngayguibd = strngayguibd;
            model.strngayguikt = strngayguikt;

            model.strkyhieu = strkyhieu;
            model.strnoigui = strnoigui;
            model.strtrichyeu = strtrichyeu;
            return PartialView(model);
        }


        public ActionResult Vanbandidientu_Read([DataSourceRequest]DataSourceRequest request,
            string strngaykycat, string strngaynhancat, string strDonviguicat,
            int? intsodibd, int? intsodikt, string strngaynhanbd, string strngaynhankt,
            string strngaykybd, string strngaykykt, string strngayguibd, string strngayguikt,
            string strkyhieu, string strnoigui, string strtrichyeu
            )
        {
            IEnumerable<ListVanbandidientuViewModel> vanban;

            int _SearchList = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchType));
            if (_SearchList == (int)EnumSession.SearchType.SearchVBDiDientu)
            {
                // lay cac gia tri search trong session
                string strSearchValues = _session.GetObject(AppConts.SessionSearchListValues).ToString();

                string _strngaykycat = _session.GetStringSearchValues("strngaykycat", strSearchValues);
                string _strngaynhancat = _session.GetStringSearchValues("strngaynhancat", strSearchValues);
                string _strDonviguicat = _session.GetStringSearchValues("strDonviguicat", strSearchValues);

                int _intsodibd = _session.GetIntSearchValues("intsodibd", strSearchValues);
                int _intsodikt = _session.GetIntSearchValues("intsodikt", strSearchValues);

                string _strngaynhanbd = _session.GetStringSearchValues("strngaynhanbd", strSearchValues);
                string _strngaynhankt = _session.GetStringSearchValues("strngaynhankt", strSearchValues);

                string _strngaykybd = _session.GetStringSearchValues("strngaykybd", strSearchValues);
                string _strngaykykt = _session.GetStringSearchValues("strngaykykt", strSearchValues);

                string _strngayguibd = _session.GetStringSearchValues("strngayguibd", strSearchValues);
                string _strngayguikt = _session.GetStringSearchValues("strngayguikt", strSearchValues);

                string _strkyhieu = _session.GetStringSearchValues("strkyhieu", strSearchValues);
                string _strtrichyeu = _session.GetStringSearchValues("strtrichyeu", strSearchValues);
                string _strnoigui = _session.GetStringSearchValues("strnoigui", strSearchValues);

                vanban = _vanban.GetListVanbandidientu
                (
                    _strngaykycat, _strngaynhancat, _strDonviguicat,
                    _intsodibd, _intsodikt, _strngaynhanbd, _strngaynhankt,
                    _strngaykybd, _strngaykykt, _strngayguibd, _strngayguikt,
                    _strkyhieu, _strnoigui, _strtrichyeu
                );
            }
            else
            {
                // khong co luu tim kiem
                vanban = _vanban.GetListVanbandidientu
                (
                    strngaykycat, strngaynhancat, strDonviguicat,
                    intsodibd, intsodikt, strngaynhanbd, strngaynhankt,
                    strngaykybd, strngaykykt, strngayguibd, strngayguikt,
                    strkyhieu, strnoigui, strtrichyeu
                );
            }
            return Json(vanban
                .OrderByDescending(p => p.dtengayky)
                .ThenByDescending(p => p.intso)
                .ToDataSourceResult(request));
        }
        public ActionResult _Listdonvigui(int? idvanban)
        {
            ViewBag.idvanban = idvanban;
            return PartialView();
        }

        public ActionResult Donvigui_Read([DataSourceRequest]DataSourceRequest request, int idvanban)
        {
            return Json(_vanban.GetListDonvigui(idvanban)
                .OrderBy(p => p.strtendonvi)
                .ThenBy(p => p.dtengaygui)
                .ToDataSourceResult(request)
                );
        }

        public ActionResult _TinhHinhXuLy(int? idguivb)
        {
            ViewBag.IdGuiVanBan = idguivb;
            return PartialView();
        }

        public ActionResult TinhHinhXuLy_Read([DataSourceRequest]DataSourceRequest request, int id)
        {
            return Json(_tinhhinhXulyVanbanDiManager.GetTinhhinhXulyVanbanDi(id)
                .OrderBy(p => p.strngayxuly)
                .ToDataSourceResult(request)
                );
        }

        public ActionResult _SearchVBDiDientu()
        {
            return PartialView();
        }

        #endregion Index

        #region Export

        public FileResult Export([DataSourceRequest]DataSourceRequest request)
        {
            // lay cac gia tri search trong session
            string strSearchValues = _session.GetObject(AppConts.SessionSearchListValues).ToString();

            string _strngaykycat = _session.GetStringSearchValues("strngaykycat", strSearchValues);
            string _strngaynhancat = _session.GetStringSearchValues("strngaynhancat", strSearchValues);
            string _strDonviguicat = _session.GetStringSearchValues("strDonviguicat", strSearchValues);

            int _intsodibd = _session.GetIntSearchValues("intsodibd", strSearchValues);
            int _intsodikt = _session.GetIntSearchValues("intsodikt", strSearchValues);

            string _strngaynhanbd = _session.GetStringSearchValues("strngaynhanbd", strSearchValues);
            string _strngaynhankt = _session.GetStringSearchValues("strngaynhankt", strSearchValues);

            string _strngaykybd = _session.GetStringSearchValues("strngaykybd", strSearchValues);
            string _strngaykykt = _session.GetStringSearchValues("strngaykykt", strSearchValues);

            string _strngayguibd = _session.GetStringSearchValues("strngayguibd", strSearchValues);
            string _strngayguikt = _session.GetStringSearchValues("strngayguikt", strSearchValues);

            string _strkyhieu = _session.GetStringSearchValues("strkyhieu", strSearchValues);
            string _strtrichyeu = _session.GetStringSearchValues("strtrichyeu", strSearchValues);
            string _strnoigui = _session.GetStringSearchValues("strnoigui", strSearchValues);

            var vanban = new List<ListVanbandidientuViewModel>(_vanban.GetListVanbandidientu
            (
                _strngaykycat, _strngaynhancat, _strDonviguicat,
                _intsodibd, _intsodikt, _strngaynhanbd, _strngaynhankt,
                _strngaykybd, _strngaykykt, _strngayguibd, _strngayguikt,
                _strkyhieu, _strnoigui, _strtrichyeu
            ));

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                /* Create the worksheet. */

                SpreadsheetDocument spreadsheet = Excel.CreateWorkbook(stream);
                Excel.AddBasicStyles(spreadsheet);
                Excel.AddAdditionalStyles(spreadsheet);
                Excel.AddWorksheet(spreadsheet, "Văn bản đi điện tử");
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


                /* Add the data to the worksheet. */

                // For each row of data...
                for (int idx = 0; idx < vanban.Count; idx++)
                {
                    // Set the field values in the spreadsheet for the current row.
                    string strngayky = DateServices.FormatDateVN(vanban[idx].dtengayky);
                    Excel.SetCellValue(spreadsheet, worksheet, 1, (uint)idx + 2, strngayky, false, false);

                    Excel.SetCellValue(spreadsheet, worksheet, 2, (uint)idx + 2, vanban[idx].intso.ToString() + vanban[idx].strsophu, false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 3, (uint)idx + 2, vanban[idx].strkyhieu, false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 4, (uint)idx + 2, vanban[idx].strtrichyeu, false, false);

                }

                /* Save the worksheet and store it in Session using the spreadsheet title. */

                worksheet.Save();
                spreadsheet.Close();

                return File(stream.ToArray(),   //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                "ExportVanbandidientu.xlsx");
            }
        }

        #endregion Export

        #region AutoSendMail

        [HttpPost]
        public ActionResult _AutoSendMail()
        {
            ResultFunction kq = _vanban.AutoSendMail();
            return Json(kq);

        }

        #endregion AutoSendMail

        #region Tonghop

        public ActionResult Tonghop()
        {
            ViewBag.isSendSolieuVB = _vanban.CheckSendSolieuVB();
            return View();
        }

        /// <summary>
        /// thong ke van ban di da gui dien tu
        /// </summary>
        /// <param name="strngaykybd"></param>
        /// <param name="strngaykykt"></param>
        /// <param name="intSoNgaygui"></param>
        /// <returns></returns>
        public ActionResult _TonghopVBDT(string strngaykybd, string strngaykykt,
            int? intSoNgaygui, string LoaiTonghop)
        {
            var tonghop = _vanban.TonghopVBDi(strngaykybd, strngaykykt, intSoNgaygui, LoaiTonghop);
            switch (LoaiTonghop)
            {
                case "donvi":
                    return PartialView("_TonghopVBDT", tonghop);
                case "chuyenvien":
                    return PartialView("_TonghopVBDT_chuyenvien", tonghop);
                default:
                    return PartialView("_TonghopVBDT", tonghop);
            }
            
        }

        [HttpPost]
        public ActionResult TonghopDonvi(int? iddonvi,
            string strngaykybd, string strngaykykt, int? intSoNgaygui)
        {
            ViewBag.iddonvi = iddonvi;
            ViewBag.strDonvi = _vanban.GetTenDonviDi((int)iddonvi);

            ViewBag.strngaykybd = strngaykybd;
            ViewBag.strngaykykt = strngaykykt;
            ViewBag.intSoNgaygui = (intSoNgaygui >= 0) ? intSoNgaygui : 0;

            return View();
        }

        /// <summary>
        /// thong ke van ban di da gui/chua gui dien tu
        /// </summary>
        /// <param name="strngaykybd"></param>
        /// <param name="strngaykykt"></param>
        /// <returns></returns>
        public ActionResult _TonghopVanbandi(string strngaykybd, string strngaykykt)
        {
            var tonghop = _vanban.TonghopVBDi_Dientu(strngaykybd, strngaykykt);

            return PartialView(tonghop);
        }

        [HttpPost]
        public ActionResult TonghopLoaiVanbandi(int? idloai,
            string strngaykybd, string strngaykykt)
        {
            ViewBag.idloai = idloai;
            ViewBag.strngaykybd = strngaykybd;
            ViewBag.strngaykykt = strngaykykt;
            string loaivb = string.Empty;
            switch (idloai)
            {
                case (int)QLVB.Domain.Entities.enumVanbandi.intguivbdt.Chuagui:
                    loaivb = "Tổng hợp văn bản đi chưa gửi điện tử";
                    break;
                case (int)QLVB.Domain.Entities.enumVanbandi.intguivbdt.Dagui:
                    loaivb = "Tổng hợp văn bản đi đã gửi điện tử";
                    break;
                default:
                    loaivb = "Tổng hợp văn bản đi";
                    break;
            }
            ViewBag.strloaivb = loaivb;
            return View();
        }

        public ActionResult _TonghopSolieuVanban(string strngaybd, string strngaykt)
        {
            var tonghop = _vanban.TonghopSolieuVB(strngaybd, strngaykt);

            return PartialView(tonghop);
        }
        #endregion Tonghop

        #region GuiTonghopVBveUBT

        [HttpPost]
        public async Task<JsonResult> _SendVBToUBT()
        {
            SettingSendTonghopVBViewModel SendSettings = _vanban.GetSettingSendTonghopVB();

            if (SendSettings.IsSendTonghopVb == true)
            {
                Dictionary<string, DateTime> ngaytonghop = _vanban.GetNgaybatdau_ketthuc();
                DateTime ngaybd = DateTime.Now;
                DateTime ngaykt = DateTime.Now;

                foreach (var n in ngaytonghop)
                {
                    //Console.WriteLine("{0}, {1}", pair.Key, pair.Value);
                    if (n.Key == "ngaybd")
                    {
                        ngaybd = n.Value;
                    }
                    if (n.Key == "ngaykt")
                    {
                        ngaykt = n.Value;
                    }
                }

                if (ngaybd.Date < DateTime.Now.Date)
                {
                    TonghopVanbanViewModel tonghop = _vanban.GetTonghopVanban(ngaybd, ngaykt);

                    HttpClient client = new HttpClient();

                    string address = "http://" + SendSettings.IPAddress;

                    client.BaseAddress = new Uri(address);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    ResultFunction kq = new ResultFunction();
                    var thongbao = await _CreateVBtAsync(tonghop, SendSettings);
                    if (thongbao == 1)
                    {
                        kq.message = "Gửi dữ liệu thành công";
                        _vanban.UpdateNgayguiTonghopVB(tonghop);                     
                    }
                    else
                    {
                        kq.message = "Lỗi!!! Không gửi được";                       
                    }

                    kq.id = 1;

                    return Json(kq);
                }
                else
                {
                    string thongbao = "Ngày hôm nay đã gửi dữ liệu rồi. Bạn có muốn gửi lại không?";
                    ResultFunction kq = new ResultFunction();
                    kq.message = thongbao;
                    kq.id = 0;

                    return Json(kq);
                }
            }else
            {
                return null;
            }
            
        }

        [HttpPost]
        public async Task<JsonResult> _ForceSendVBToUBT()
        {
            SettingSendTonghopVBViewModel SendSettings = _vanban.GetSettingSendTonghopVB();

            if (SendSettings.IsSendTonghopVb == true)
            {
                DateTime ngaybd = DateTime.Now.AddDays(-1);
                DateTime ngaykt = DateTime.Now.AddDays(-1);


                TonghopVanbanViewModel tonghop = _vanban.GetTonghopVanban(ngaybd, ngaykt);

                HttpClient client = new HttpClient();

                string address = "http://" + SendSettings.IPAddress;

                client.BaseAddress = new Uri(address);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                ResultFunction kq = new ResultFunction();
                var thongbao = await _CreateVBtAsync(tonghop, SendSettings);
                if (thongbao == 1)
                {
                    kq.message = "Gửi dữ liệu thành công";
                    _vanban.UpdateNgayguiTonghopVB(tonghop);
                }
                else
                {
                    kq.message = "Lỗi!!! Không gửi được";
                }

                kq.id = 1;

                return Json(kq);
                
            }
            else
            {
                return null;
            }

        }

        private static async Task<int> _CreateVBtAsync(TonghopVanbanViewModel vb, SettingSendTonghopVBViewModel setting)
        {
            
            HttpClient client = new HttpClient();

            string address = "http://" + setting.IPAddress;
            var endpoint = address + "/api/vbdt";

            HttpResponseMessage response = await client.PostAsJsonAsync(endpoint, vb);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                return 1; // "Gửi dữ liệu thành công";
            }else
            {
                return 0;// "Lỗi!!! Không gửi được";
            }            
        }


        #endregion GuiTonghopVBveUBT

       
    }
}