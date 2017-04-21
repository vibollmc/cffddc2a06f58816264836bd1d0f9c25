using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Core.Contract;
using QLVB.DTO.Vanbandi;
using QLVB.DTO;
using QLVB.DTO.Vanbandientu;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using QLVB.Common.Sessions;
using QLVB.Common.Utilities;
using QLVB.Domain.Entities;
using QLVB.Common.Date;
using DocumentFormat.OpenXml.Packaging;
using QLVB.WebUI.Common.OpenXML;
using DocumentFormat.OpenXml.Spreadsheet;
using QLVB.DTO.Edxml;
using QLVB.DTO.Truclienthongtinh;

namespace QLVB.WebUI.Controllers
{
    public class VanbandiController : Controller
    {
        #region Constructor
        private IVanbandiManager _vanban;
        private ISessionServices _session;
        private IMailManager _mail;
        private IEdxmlManager _edxml;
        private ITrucLienthongTinhManager _truclienthong;
        public VanbandiController(IVanbandiManager vanban, ISessionServices session, IMailManager mail, 
                IEdxmlManager edxml, ITrucLienthongTinhManager truclienthong)
        {
            _vanban = vanban;
            _session = session;
            _mail = mail;
            _edxml = edxml;
            _truclienthong = truclienthong;
        }

        #endregion Constructor

        #region ViewIndex
        public ActionResult Index(bool? isBack)
        {
            ToolbarViewModel tbar = _vanban.GetToolbar();
            ViewBag.IsDuyetvb = tbar.Duyetvb == "none" ? false : true;

            if (isBack == true)
            {
                // luu session tim kiem
                //SessionService.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchVBDi);
            }
            else
            {   // khong co isBack (nhan vao menu vanbandi)
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
            ToolbarViewModel model = _vanban.GetToolbar();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult _Category()
        {
            CategoryViewModel model = _vanban.GetCategory();
            return PartialView(model);
        }

        public ActionResult _ListVanbandi(
            bool? isSearch, bool? isBack,
            string strngaykycat, int? idloaivb, int? idsovb, string strvbphathanh, string hoibao,
            int? intsobd, int? intsokt, string strngaykybd, string strngaykykt,
            string strkyhieu, string strnguoiky, string strnguoisoan, string strnguoiduyet,
            string strnoinhan, string strtrichyeu, string strhantraloi, string strdonvisoan,
            int? idkhan, int? idmat
            )
        {
            SearchVBViewModel model = new SearchVBViewModel();
            //===============================================
            // status
            //===============================================
            model.isSearch = isSearch == true ? true : false;
            model.isBack = isBack == true ? true : false;

            int intPage = 1;
            if (isBack == true)
            {   // tra ve page dang xem khi quay lai
                int _PageType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchPageType));
                if (_PageType == (int)EnumSession.PageType.VBDi)
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
            model.idsovb = idsovb;//!= null ? (int)idsovb : 0;
            model.idloaivb = idloaivb;//!= null ? (int)idloaivb : 0;            
            model.strngaykycat = strngaykycat;
            model.strvbphathanh = strvbphathanh;
            model.hoibao = hoibao;
            //===============================================
            // search
            //===============================================
            model.intsobd = intsobd;
            model.intsokt = intsokt;

            model.strngaykybd = strngaykybd;
            model.strngaykykt = strngaykykt;

            model.strkyhieu = strkyhieu;
            model.strnguoiky = strnguoiky;
            model.strnguoiduyet = strnguoiduyet;
            model.strnguoisoan = strnguoisoan;

            model.strnoinhan = strnoinhan;
            model.strtrichyeu = strtrichyeu;

            model.strhantraloi = strhantraloi;
            model.strdonvisoan = strdonvisoan;

            model.idkhan = idkhan;
            model.idmat = idmat;

            return PartialView(model);

        }

        public ActionResult Vanbandi_Read(
            [DataSourceRequest]DataSourceRequest request,
            string strngaykycat, int? idloaivb, int? idsovb, string strvbphathanh, string hoibao,
            int? intsobd, int? intsokt, string strngaykybd, string strngaykykt,
            string strkyhieu, string strnguoiky, string strnguoisoan, string strnguoiduyet,
            string strnoinhan, string strtrichyeu, string strhantraloi, string strdonvisoan,
             int? idkhan, int? idmat
            )
        {
            int currentPage = request.Page;
            string highlightResult = string.Empty;
            // luu trang dang xem vao session
            _session.InsertObject(AppConts.SessionSearchPageType, EnumSession.PageType.VBDi);
            _session.InsertObject(AppConts.SessionSearchPageValues, currentPage);

            IEnumerable<ListVanbandiViewModel> vbdi;

            int _SearchType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchType));
            if (_SearchType == (int)EnumSession.SearchType.SearchVBDi)
            {
                // lay cac gia tri search trong session
                string strSearchValues = _session.GetObject(AppConts.SessionSearchTypeValues).ToString();

                int _idloaivb = _session.GetIntSearchValues("idloaivb", strSearchValues);
                int _idsovb = _session.GetIntSearchValues("idsovb", strSearchValues);
                string _strngaykycat = _session.GetStringSearchValues("strngaykycat", strSearchValues);
                string _strvbphathanh = _session.GetStringSearchValues("strvbphathanh", strSearchValues);
                string _hoibao = _session.GetStringSearchValues("hoibao", strSearchValues);

                int _intsobd = _session.GetIntSearchValues("intsobd", strSearchValues);
                int _intsokt = _session.GetIntSearchValues("intsokt", strSearchValues);
                string _strngaykybd = _session.GetStringSearchValues("strngaykybd", strSearchValues);
                string _strngaykykt = _session.GetStringSearchValues("strngaykykt", strSearchValues);
                string _strkyhieu = _session.GetStringSearchValues("strkyhieu", strSearchValues);
                string _strnguoiky = _session.GetStringSearchValues("strnguoiky", strSearchValues);
                string _strnguoisoan = _session.GetStringSearchValues("strnguoisoan", strSearchValues);
                string _strnguoiduyet = _session.GetStringSearchValues("strnguoiduyet", strSearchValues);

                string _strtrichyeu = _session.GetStringSearchValues("strtrichyeu", strSearchValues);
                string _strnoinhan = _session.GetStringSearchValues("strnoinhan", strSearchValues);
                string _strhantraloi = _session.GetStringSearchValues("strhantraloi", strSearchValues);
                string _strdonvisoan = _session.GetStringSearchValues("strdonvisoan", strSearchValues);

                int _idkhan = _session.GetIntSearchValues("idkhan", strSearchValues);
                int _idmat = _session.GetIntSearchValues("idmat", strSearchValues);

                highlightResult = _strtrichyeu;
                vbdi = _GetListvanban(
                        _strngaykycat, _idloaivb, _idsovb, _strvbphathanh, _hoibao,
                        _intsobd, _intsokt, _strngaykybd, _strngaykykt,
                        _strkyhieu, _strnguoiky, _strnguoisoan, _strnguoiduyet,
                        _strnoinhan, _strtrichyeu, _strhantraloi, _strdonvisoan,
                        _idkhan, _idmat
                    );
                //return Json(
                //_GetListvanban(
                //    _strngaykycat, _idloaivb, _idsovb,
                //    _intsobd, _intsokt, _strngaykybd, _strngaykykt,
                //    _strkyhieu, _strnguoiky, _strnguoisoan, _strnguoiduyet,
                //    _strnoinhan, _strtrichyeu, _strhantraloi, _strdonvisoan
                //)
                //.OrderByDescending(p => p.dtengayky)
                //.ThenByDescending(p => p.intso)
                //.ToDataSourceResult(request));
            }
            else
            {
                highlightResult = strtrichyeu;
                // khong co luu tim kiem
                vbdi = _GetListvanban(
                        strngaykycat, idloaivb, idsovb, strvbphathanh, hoibao,
                        intsobd, intsokt, strngaykybd, strngaykykt,
                        strkyhieu, strnguoiky, strnguoisoan, strnguoiduyet,
                        strnoinhan, strtrichyeu, strhantraloi, strdonvisoan,
                        idkhan, idmat
                    );
                //return Json(
                //_GetListvanban(
                //    strngaykycat, idloaivb, idsovb,
                //    intsobd, intsokt, strngaykybd, strngaykykt,
                //    strkyhieu, strnguoiky, strnguoisoan, strnguoiduyet,
                //    strnoinhan, strtrichyeu, strhantraloi, strdonvisoan
                //)
                //.OrderByDescending(p => p.dtengayky)
                //.ThenByDescending(p => p.intso)
                //.ToDataSourceResult(request));
            }
            DataSourceResult result = vbdi.OrderByDescending(p => p.dtengayky)
                                            .ThenByDescending(p => p.intso)
                                            .ToDataSourceResult(request);
            //if (!string.IsNullOrEmpty(highlightResult))
            //{
            //    foreach (ListVanbandiViewModel item in result.Data)
            //    {
            //        // add highlight result
            //        Dictionary<bool, string> keyHighlight = ValidateData.SearchExactly(highlightResult);
            //        if (keyHighlight.ContainsKey(true))
            //        {   // co tim kiem chinh xac "abc"   
            //            highlightResult = keyHighlight[true];
            //        }
            //        string[] keywords = highlightResult.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            //        foreach (string key in keywords)
            //        {
            //            //item.strtrichyeu = item.strtrichyeu.Replace(key, "<span class='HighLight'>" + key + "</span>");
            //            item.strtrichyeu = System.Text.RegularExpressions.Regex.Replace(item.strtrichyeu,
            //                                key, "<span class='HighLight'>" + key + "</span>",
            //                                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //        }
            //    }
            //}

            return Json(result);
        }

        private IEnumerable<ListVanbandiViewModel> _GetListvanban(
            string strngaykycat, int? idloaivb, int? idsovb, string strvbphathanh, string hoibao,
            int? intsobd, int? intsokt, string strngaykybd, string strngaykykt,
            string strkyhieu, string strnguoiky, string strnguoisoan, string strnguoiduyet,
            string strnoinhan, string strtrichyeu, string strhantraloi, string strdonvisoan,
             int? idkhan, int? idmat
            )
        {
            return _vanban.GetListVanbandi(
                    strngaykycat, idloaivb, idsovb, strvbphathanh, hoibao,
                    intsobd, intsokt, strngaykybd, strngaykykt,
                    strkyhieu, strnguoiky, strnguoisoan, strnguoiduyet,
                    strnoinhan, strtrichyeu, strhantraloi, strdonvisoan,
                    idkhan, idmat
                );
        }

        public ActionResult _SearchVBDi()
        {
            SearchVBViewModel model = _vanban.GetViewSearch();
            return PartialView(model);
        }

        #endregion ViewIndex

        #region ViewDetail

        public ActionResult _ViewDetailVBDi()
        {
            return PartialView();
        }

        public ActionResult _XemChitietVanban(int id)
        {
            DetailVBDiViewModel model = new DetailVBDiViewModel();
            if (id != 0)
            {
                model = _vanban.GetViewDetail(id);
            }
            return PartialView(model);
        }

        #endregion ViewDetail

        #region Duyetvanban

        [HttpPost]
        public ActionResult _AjaxDuyetVanban(int id)
        {
            ResultFunction kq = _vanban.DuyetVanban(id, (int)enumVanbandi.inttrangthai.Daduyet);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json((int)kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        [HttpPost]
        public ActionResult _AjaxHuyDuyetVanban(int id)
        {
            ResultFunction kq = _vanban.DuyetVanban(id, (int)enumVanbandi.inttrangthai.Chuaduyet);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json((int)kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        #endregion Duyetvanban

        #region UpdateVanban

        public ActionResult Themvanban(int? id)
        {
            ThemvanbanViewModel model = _vanban.GetLoaitruong(0, 0, id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Themvanban(int? idloaivb, int? intAddNext,
            FormCollection collection, ThemvanbanViewModel vb)
        {
            ThemvanbanViewModel vanban = new ThemvanbanViewModel();
            vanban.Vanbandi = vb.Vanbandi;
            string strgiatri = "";
            foreach (string p in collection)
            {   // lay cac gia tri tu form dien vao control 
                strgiatri = collection[p];
                if (!string.IsNullOrEmpty(strgiatri))
                {
                    if (p == "strngayky") { vanban.Vanbandi.strngayky = (DateTime)DateServices.FormatDateEn(strgiatri); }
                    if (p == "strhanxuly") { vanban.Vanbandi.strhanxuly = DateServices.FormatDateEn(collection[p]); }

                    if (p == "strkyhieu") { vanban.Vanbandi.strkyhieu = collection[p]; }
                    if (p == "strnoinhan") { vanban.Vanbandi.strnoinhan = collection[p]; }
                    if (p == "strnguoiky") { vanban.Vanbandi.strnguoiky = collection[p]; }
                    if (p == "strnguoisoan") { vanban.Vanbandi.strnguoisoan = collection[p]; }
                    if (p == "strnguoiduyet") { vanban.Vanbandi.strnguoiduyet = collection[p]; }

                    if (p == "idsovanban") { vanban.Vanbandi.intidsovanban = Convert.ToInt32(strgiatri); }
                    if (p == "idloaivanban") { vanban.Vanbandi.intidphanloaivanbandi = Convert.ToInt32(strgiatri); }
                    if (p == "idnguoiduyet") { vanban.Vanbandi.intidnguoiduyet = Convert.ToInt32(strgiatri); }

                    if (p == "IsQPPL")
                    {
                        if (strgiatri.ToLower().Contains("true"))
                        {
                            vanban.IsQPPL = true;
                            vanban.Vanbandi.intquyphamphapluat = (int)enumVanbanden.intquyphamphapluat.Co;
                        }
                        else
                        {
                            vanban.IsQPPL = false;
                            vanban.Vanbandi.intquyphamphapluat = (int)enumVanbanden.intquyphamphapluat.Khong;
                        }
                    }
                }
            }

            if (intAddNext == 1)
            {   // them tiep: giu nguyen cac truong chi tang so van ban di
                vanban = _vanban.GetLoaitruong(idloaivb, vanban.Vanbandi.intidsovanban, 0);
                vanban.IsSave = false;
                vanban.Vanbandi = vb.Vanbandi;
                vanban.Vanbandi.intid = 0;
                return View(vanban);
            }

            // thay doi loai van ban, giu lai cac gia tri tren form
            if (idloaivb != 0)
            {
                vanban = _vanban.GetLoaitruong(idloaivb, vanban.Vanbandi.intidsovanban, 0);
                vanban.IsSave = false;
                vanban.Vanbandi = vb.Vanbandi;
            }
            else
            {   // submit form
                // ghi nhan van ban

                // kiem tra dieu kien ghi nhan
                if (vb.Vanbandi.intid == 0)
                {   // them moi                    
                    int intidvanban = _vanban.Savevanban(vb);

                    vanban = _vanban.GetLoaitruong(0, 0, intidvanban);
                    vanban.IsSave = (intidvanban != 0) ? true : false;
                }
                else
                {   // cap nhat
                    bool kq = _vanban.Editvanban(vb.Vanbandi.intid, vb.Vanbandi);

                    vanban = _vanban.GetLoaitruong(0, 0, vb.Vanbandi.intid);
                    vanban.IsSave = kq;
                }
            }
            return View(vanban);
        }

        [HttpPost]
        public ActionResult _AjaxGetSovb(int idsovb)
        {
            AjaxSovanban sovb = _vanban.GetSovanban(idsovb);
            return Json(sovb);

        }

        public ActionResult _AjaxGetTenDonvi(string q)
        {
            if (!string.IsNullOrEmpty(q))
            {
                var donvi = _vanban.GetTenDonvi(q);
                return Json(donvi, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var donvi = _vanban.GetTenDonvi();
                return Json(donvi, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult _AjaxGetNoinhan(int? idvanban, string strnoinhantiep)
        {
            var donvi = _vanban.GetNoinhan(idvanban, strnoinhantiep);
            if (donvi == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(donvi, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult _formDeleteVanban(int id)
        {
            QLVB.DTO.Vanbandi.DeleteVBViewModel model = _vanban.GetDeleteVanban(id);
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

        public ActionResult _CheckLienketVanban(int id)
        {
            var idvanbanden = _vanban.CheckLienketVanban(id);
            return Json(idvanbanden, JsonRequestBehavior.AllowGet);            
        }

        public ActionResult _CheckTraloiVanban(string strTraloivb, int idvanbandi)
        {
            var model = _vanban.CheckTraloiVanban(strTraloivb, idvanbandi);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion UpdateVanban

        #region Capquyenxem

        public ActionResult _Capquyenxem()
        {
            return PartialView();
        }

        public ActionResult _ListUserCapquyenxem(int idvanban)
        {
            ListUserCapquyenxemViewModel model = _vanban.GetListUserCapquyenxem(idvanban);
            return PartialView(model);
        }

        /// <summary>
        /// submit list cac can bo duoc cap quyen xem
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _SaveCapquyenxem(int idvanban, FormCollection collection)
        {
            string strgiatri = "";
            //int idvanban = 0;
            List<int> listidcanbo = new List<int>();
            foreach (string p in collection)
            {
                strgiatri = collection[p].ToLower();
                if (strgiatri.Contains("true"))
                {
                    listidcanbo.Add(Convert.ToInt32(p));
                }
            }

            ResultFunction kq = _vanban.SaveCapquyenxem(idvanban, listidcanbo);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json((int)kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        /// <summary>
        /// cap quyen xem van ban la public hoac private
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="intpublic">trang thai public/private hien co cua van ban dang xet</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _CapquyenxemPublic(int idvanban, int intpublic)
        {


            ResultFunction kq = _vanban.CapquyenxemPublic(idvanban, intpublic);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json((int)kq.id);
            }
            else
            {
                return Json(kq.message);
            }

        }

        #endregion Capquyenxem

        #region UploadFile

        public ActionResult _UploadVBDi(int idvanban)
        {
            UploadVBDiViewModel model = _vanban.GetListFile(idvanban);

            return PartialView(model);
        }

        #endregion UploadFile

        #region ExportGridToExcel

        public FileResult Export([DataSourceRequest]DataSourceRequest request)
        {

            //Get the data representing the current grid state - page, sort and filter
            //var products = new List<Product>(db.Products.ToDataSourceResult(request).Data as IEnumerable<Product>);

            var vanban = new List<ListVanbandiViewModel>();

            // lay cac gia tri search trong session
            string strSearchValues = _session.GetObject(AppConts.SessionSearchTypeValues).ToString();

            int _idloaivb = _session.GetIntSearchValues("idloaivb", strSearchValues);
            int _idsovb = _session.GetIntSearchValues("idsovb", strSearchValues);
            string _strngaykycat = _session.GetStringSearchValues("strngaykycat", strSearchValues);
            string _strvbphathanh = _session.GetStringSearchValues("strvbphathanh", strSearchValues);
            string _hoibao = _session.GetStringSearchValues("hoibao", strSearchValues);

            int _intsobd = _session.GetIntSearchValues("intsobd", strSearchValues);
            int _intsokt = _session.GetIntSearchValues("intsokt", strSearchValues);
            string _strngaykybd = _session.GetStringSearchValues("strngaykybd", strSearchValues);
            string _strngaykykt = _session.GetStringSearchValues("strngaykykt", strSearchValues);
            string _strkyhieu = _session.GetStringSearchValues("strkyhieu", strSearchValues);
            string _strnguoiky = _session.GetStringSearchValues("strnguoiky", strSearchValues);
            string _strnguoisoan = _session.GetStringSearchValues("strnguoisoan", strSearchValues);
            string _strnguoiduyet = _session.GetStringSearchValues("strnguoiduyet", strSearchValues);

            string _strtrichyeu = _session.GetStringSearchValues("strtrichyeu", strSearchValues);
            string _strnoinhan = _session.GetStringSearchValues("strnoinhan", strSearchValues);
            string _strhantraloi = _session.GetStringSearchValues("strhantraloi", strSearchValues);
            string _strdonvisoan = _session.GetStringSearchValues("strdonvisoan", strSearchValues);


            int _idkhan = _session.GetIntSearchValues("idkhan", strSearchValues);
            int _idmat = _session.GetIntSearchValues("idmat", strSearchValues);

            vanban = new List<ListVanbandiViewModel>(_GetListvanban
                (
                _strngaykycat, _idloaivb, _idsovb, _strvbphathanh, _hoibao,
                _intsobd, _intsokt, _strngaykybd, _strngaykykt,
                _strkyhieu, _strnguoiky, _strnguoisoan, _strnguoiduyet,
                _strnoinhan, _strtrichyeu, _strhantraloi, _strdonvisoan,
                _idkhan, _idmat
                )
                .OrderByDescending(p => p.dtengayky)
                .ThenByDescending(p => p.intso));

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                /* Create the worksheet. */

                SpreadsheetDocument spreadsheet = Excel.CreateWorkbook(stream);
                Excel.AddBasicStyles(spreadsheet);
                Excel.AddAdditionalStyles(spreadsheet);
                Excel.AddWorksheet(spreadsheet, "Văn bản đi");
                Worksheet worksheet = spreadsheet.WorkbookPart.WorksheetParts.First().Worksheet;

                //create columns and set their widths

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 1, "Ngày ký", false, false);
                Excel.SetColumnWidth(worksheet, 1, 15);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 2, "Số phát hành", false, false);
                Excel.SetColumnWidth(worksheet, 2, 15);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 3, "Trích yếu", false, false);
                Excel.SetColumnWidth(worksheet, 3, 50);

                Excel.SetColumnHeadingValue(spreadsheet, worksheet, 4, "Nơi nhận", false, false);
                Excel.SetColumnWidth(worksheet, 4, 30);


                /* Add the data to the worksheet. */

                // For each row of data...
                for (int idx = 0; idx < vanban.Count; idx++)
                {
                    // Set the field values in the spreadsheet for the current row.
                    string strngayky = DateServices.FormatDateVN(vanban[idx].dtengayky);
                    Excel.SetCellValue(spreadsheet, worksheet, 1, (uint)idx + 2, strngayky, false, false);

                    string sodi = vanban[idx].intso.ToString() + vanban[idx].strsophu + "/" + vanban[idx].strkyhieu;
                    Excel.SetCellValue(spreadsheet, worksheet, 2, (uint)idx + 2, sodi, false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 3, (uint)idx + 2, vanban[idx].strtrichyeu, false, false);
                    Excel.SetCellValue(spreadsheet, worksheet, 4, (uint)idx + 2, vanban[idx].strnoinhan, false, false);
                }

                /* Save the worksheet and store it in Session using the spreadsheet title. */

                worksheet.Save();
                spreadsheet.Close();

                return File(stream.ToArray(),   //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                "ExportVanbandi.xlsx");
            }
        }

        #endregion ExportGridToExcel

        #region Email

        public ActionResult Email(int id)
        {
            ListEmailDonviViewModel model = _vanban.GetListEmailDonvi(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult _SendDonvi(int idvanban, FormCollection collection)
        {
            string strgiatri = "";
            //int idvanban = 0;
            string result = string.Empty;
            List<int> listiddonvi = new List<int>();
            bool isAutoSend = false;
            foreach (string p in collection)
            {
                strgiatri = collection[p].ToLower();
                if (strgiatri.Contains("true"))
                {
                    if ((p != "parentcheckbox") && (p != "autosend"))
                    {
                        listiddonvi.Add(Convert.ToInt32(p));
                    }
                    else
                    {
                        if (p == "autosend") { isAutoSend = true; }
                    }
                }
            }

            if (listiddonvi.Count() != 0)
            {
                var listSendDonvi = _vanban.GetListSendDonvi(listiddonvi).ToList();

                int intAutoSend = (isAutoSend) ? 1 : 0;
                ResultFunction kq = _mail.GuiVBDT(idvanban, listSendDonvi, (int)enumGuiVanban.intloaivanban.Vanbandi, intAutoSend);
                return Json(kq);
                //return Json(listSendDonvi);
            }
            else
            {
                return Json("Không tìm thấy đơn vị gửi văn bản điện tử");
            }
        }


        [HttpPost]
        public ActionResult _SaveEmailKhac(int idvanbanEmailKhac, string donviEmailKhac,
            string strtieudeEmailKhac, string strnoidungEmailKhac)
        {
            var kq = _mail.SendEmailKhac((int)enumGuiVanban.intloaivanban.Vanbandi,
                    idvanbanEmailKhac, donviEmailKhac, strtieudeEmailKhac, strnoidungEmailKhac);
            return Json(kq);
        }

        public ActionResult EdXml(int id)
        {
            return View();
        }

        public ActionResult _GetDonviEdxml(int id)
        {
            DonviEdxmlViewModel model = _edxml.getalldonvi(id);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult _SendEdxmlChinhphu(int idvanbanEdxml, FormCollection collection)
        {
            string strgiatri = "";
            DonviEdxmlViewModel donvi = new DonviEdxmlViewModel();
            donvi.idvanban = idvanbanEdxml;
            List<AgencyViewModel> listdonvi = new List<AgencyViewModel>();

            foreach (string p in collection)
            {
                strgiatri = collection[p].ToLower();
                if (strgiatri.Contains("true"))
                {
                    if ((p != "parentcheckboxEdxml") && (p != "autosend"))
                    {
                        string[] split = collection[p].Split(',');
                        foreach (var s in split)
                        {
                            if ((!s.ToLower().Contains("true")) && (!s.ToLower().Contains("false")))
                            {
                                AgencyViewModel senddonvi = new AgencyViewModel();
                                senddonvi.Madinhdanh = p;
                                senddonvi.strtendonvi = s;
                                listdonvi.Add(senddonvi);
                            }
                        }
                    }
                }
            }
            donvi.listdonvi = listdonvi;
            if (donvi.listdonvi.Count() > 0)
            {
                var edxml = _edxml.Sender(idvanbanEdxml, (int)enumGuiVanban.intloaivanban.Vanbandi, donvi);
                //_edxml.Sender(idvanbanEdxml, (int)enumGuiVanban.intloaivanban.Vanbandi, donvi);
                return Json(edxml);
            }
            else
            {
                return Json("Không tìm thấy đơn vị gửi văn bản điện tử");
            }
        }

        #endregion Email

        #region Vanbanlienquan

        public ActionResult _SearchVBDiLQ(int idhoso)
        {
            SearchVBViewModel model = _vanban.GetViewSearch();
            ViewBag.idhoso = idhoso;
            return PartialView(model);
        }

        public ActionResult ListVBDiLienquan(int idhoso, SearchVBViewModel model)
        {
            model.isSearch = (model.isSearch == true) ? true : false;
            ViewBag.idhoso = idhoso;
            return View(model);
        }

        public ActionResult Vanbandilienquan_Read(
            [DataSourceRequest]DataSourceRequest request, int idhoso,
            bool? isSearch,
            string strngaykycat, int? idloaivb, int? idsovb,
            int? intsobd, int? intsokt, string strngaykybd, string strngaykykt,
            string strkyhieu, string strnguoiky, string strnguoisoan, string strnguoiduyet,
            string strnoinhan, string strtrichyeu, string strhantraloi, string strdonvisoan
            )
        {
            int currentPage = request.Page;

            // luu trang dang xem vao session
            _session.InsertObject(AppConts.SessionSearchPageType, EnumSession.PageType.VBDiLQ);
            _session.InsertObject(AppConts.SessionSearchPageValues, currentPage);

            int _SearchType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchType));
            if ((_SearchType == (int)EnumSession.SearchType.SearchVBDiLQ) && (isSearch == true))
            {
                // lay cac gia tri search trong session
                string strSearchValues = _session.GetObject(AppConts.SessionSearchTypeValues).ToString();

                int _idloaivb = _session.GetIntSearchValues("idloaivb", strSearchValues);
                int _idsovb = _session.GetIntSearchValues("idsovb", strSearchValues);
                string _strngaykycat = _session.GetStringSearchValues("strngaykycat", strSearchValues);


                int _intsobd = _session.GetIntSearchValues("intsobd", strSearchValues);
                int _intsokt = _session.GetIntSearchValues("intsokt", strSearchValues);
                string _strngaykybd = _session.GetStringSearchValues("strngaykybd", strSearchValues);
                string _strngaykykt = _session.GetStringSearchValues("strngaykykt", strSearchValues);
                string _strkyhieu = _session.GetStringSearchValues("strkyhieu", strSearchValues);
                string _strnguoiky = _session.GetStringSearchValues("strnguoiky", strSearchValues);
                string _strnguoisoan = _session.GetStringSearchValues("strnguoisoan", strSearchValues);
                string _strnguoiduyet = _session.GetStringSearchValues("strnguoiduyet", strSearchValues);

                string _strtrichyeu = _session.GetStringSearchValues("strtrichyeu", strSearchValues);
                string _strnoinhan = _session.GetStringSearchValues("strnoinhan", strSearchValues);
                string _strhantraloi = _session.GetStringSearchValues("strhantraloi", strSearchValues);
                string _strdonvisoan = _session.GetStringSearchValues("strdonvisoan", strSearchValues);

                return Json(_vanban.GetListVanbandilienquan
                    (idhoso,
                    _strngaykycat, _idloaivb, _idsovb,
                    _intsobd, _intsokt, _strngaykybd, _strngaykykt,
                    _strkyhieu, _strnguoiky, _strnguoisoan, _strnguoiduyet,
                    _strnoinhan, _strtrichyeu, _strhantraloi, _strdonvisoan
                )
                .OrderByDescending(p => p.dtengayky)
                .ThenByDescending(p => p.intso)
                .ToDataSourceResult(request));
            }
            else
            {
                // khong co luu tim kiem
                return Json(_vanban.GetListVanbandilienquan
                    (idhoso,
                    strngaykycat, idloaivb, idsovb,
                    intsobd, intsokt, strngaykybd, strngaykykt,
                    strkyhieu, strnguoiky, strnguoisoan, strnguoiduyet,
                    strnoinhan, strtrichyeu, strhantraloi, strdonvisoan
                )
                .OrderByDescending(p => p.dtengayky)
                .ThenByDescending(p => p.intso)
                .ToDataSourceResult(request));
            }

        }


        [HttpPost]
        public ActionResult _SaveVBDiLienquan(string stridvanban, int idhosocongviec)
        {
            List<int> listidvanban = new List<int>();

            // kiem tra chuoi idvanban khac rong
            if (!string.IsNullOrEmpty(stridvanban))
            {
                string[] idvanban = stridvanban.Split(new Char[] { ',' });
                foreach (var p in idvanban)
                {
                    if (!string.IsNullOrEmpty(p))
                    {
                        int id = Convert.ToInt32(p);
                        listidvanban.Add(id);
                    }
                }
                int kq = _vanban.SaveVBDiLienquan(listidvanban, idhosocongviec);
                return Json(kq);
            }
            else
            {
                return Json(0);
            }
        }

        public ActionResult HosoVBDiLQ_Read(
            [DataSourceRequest]DataSourceRequest request,
            int idhoso)
        {
            return Json(_vanban.GetHosoVBDiLQ(idhoso)
                    .OrderByDescending(p => p.dtengayky)
                    .ThenByDescending(p => p.intso)
                    .ToDataSourceResult(request));
        }

        #endregion Vanbanlienquan

        #region TonghopVBDT

        public ActionResult TonghopVBDonvi_Read(
             [DataSourceRequest]DataSourceRequest request,
            int? iddonvi, string strngaykybd, string strngaykykt, int? intSoNgaygui
            )
        {
            var vbdi = _vanban.GetListVBDientuDonvi(iddonvi, strngaykybd, strngaykykt, intSoNgaygui);

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

        public ActionResult TonghopLoaiVBDT_Read(
             [DataSourceRequest]DataSourceRequest request,
            int? idloai, string strngaykybd, string strngaykykt
            )
        {
            var vbdi = _vanban.GetListLoaiVBDientu(idloai, strngaykybd, strngaykykt);

            DataSourceResult result = vbdi.OrderByDescending(p => p.dtengayky)
                                            .ThenByDescending(p => p.intso)
                                            .ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        public ActionResult TonghopLoaiVBDT_Export(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        #endregion TonghopVBDT

        #region TruclienthongTinh

        [HttpPost]
        public JsonResult _ConnectToTruclienthongTinh()
        {
            var webservice = _truclienthong.ConnectGateway();
            if (webservice != null)
            {
                return Json("Kết nối thành công");
            }else
            {
                return Json("Lỗi !!!");
            }
        }

        public ActionResult _GetDonviTrucTinh(int id)
        {
            var donvi = _truclienthong.GetAllOrganization();
            ViewBag.idvanban = id;
            return PartialView(donvi);
        }

        [HttpPost]
        public JsonResult _SendVBTruclienthongTinh(int idvanbanTrucTinh, FormCollection collection)
        {
            string strgiatri = "";           
            
            List<OrganizationVM> listdonvi = new List<OrganizationVM>();

            foreach (string p in collection)
            {
                strgiatri = collection[p].ToLower();
                if (strgiatri.Contains("true"))
                {
                    if ((p != "parentcheckboxTrucTinh") && (p != "autosend"))
                    {
                        string[] split = collection[p].Split(',');
                        foreach (var s in split)
                        {
                            if ((!s.ToLower().Contains("true")) && (!s.ToLower().Contains("false")))
                            {
                                OrganizationVM senddonvi = new OrganizationVM();
                                senddonvi.code = p;
                                senddonvi.name = s;
                                listdonvi.Add(senddonvi);
                            }
                        }
                    }
                }
            }
            //donvi.listdonvi = listdonvi;
            //if (donvi.listdonvi.Count() > 0)
            //{
            //    var edxml = _edxml.Sender(idvanbanEdxml, (int)enumGuiVanban.intloaivanban.Vanbandi, donvi);
            //    //_edxml.Sender(idvanbanEdxml, (int)enumGuiVanban.intloaivanban.Vanbandi, donvi);
            //    return Json(edxml);
            //}
            //else
            //{
            //    return Json("Không tìm thấy đơn vị gửi văn bản điện tử");
            //}
            return Json("OK");
        }


        #endregion TruclienthongTinh
    }
}