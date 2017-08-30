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
using QLVB.DTO.Vanbanden;
using QLVB.Common.Date;
using QLVB.Common.Sessions;
using QLVB.Common.Utilities;
using DocumentFormat.OpenXml.Packaging;
using QLVB.WebUI.Common.OpenXML;
using DocumentFormat.OpenXml.Spreadsheet;

namespace QLVB.WebUI.Controllers
{
    public class VanbandenController : Controller
    {
        #region Constructor

        private IVanbandenManager _vanban;
        private ISessionServices _session;
        private IMailManager _mail;
        private IEdxmlManager _edxmlManager;
        private ITrucLienthongTinhManager _truclienthongtinhManager;
        public VanbandenController(IVanbandenManager vanban, ISessionServices session, IMailManager mail, IEdxmlManager edxmlManager, ITrucLienthongTinhManager truclienthongtinhManager)
        {
            _vanban = vanban;
            _session = session;
            _mail = mail;
            _edxmlManager = edxmlManager;
            _truclienthongtinhManager = truclienthongtinhManager;
        }
        #endregion Constructor

        #region ViewIndex

        //=========================================================
        // cơ chế lưu page và search:
        // Index: - từ menu: khong có isBack thì SearchType = Nosearch
        //        - từ các nút back: sửa, phân XLVB,..: có isBack thì SearchType = SearchVBDen
        // Listvanban: nếu isBack thì trả về trang đang lưu
        //          
        // Vanbanden_Read: - Kiểm tra điều kiện SearchType, nếu la SearchVBDen thì tìm kiếm theo SearchType
        //=========================================================

        public ActionResult Index(bool? isBack)
        {
            ToolbarViewModel tbar = _vanban.GetToolbar();
            ViewBag.IsDuyetvb = tbar.Duyetvb == "none" ? false : true;

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
            ToolbarViewModel model = _vanban.GetToolbar();
            return PartialView(model);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 1000, VaryByParam = "none")]
        public ActionResult _Category()
        {
            CategoryViewModel model = _vanban.GetCategory();
            return PartialView(model);
        }


        public ActionResult _ListVanbanden
            (bool? isSearch, bool? isBack,
            string strngaydencat, int? idloaivb, int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly, string strdangvanban
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
                if (_PageType == (int)EnumSession.PageType.VBDen)
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

        public ActionResult _ListVanbanden_2
            (bool? isSearch, bool? isBack,
            string strngaydencat, int? idloaivb, int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly, string strdangvanban
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
                if (_PageType == (int)EnumSession.PageType.VBDen)
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

            model.strdangvanban = strdangvanban;

            return PartialView(model);

        }

        public ActionResult Vanbanden_Read
            ([DataSourceRequest]DataSourceRequest request,
            //bool isSearch,
            string strngaydencat, int? idloaivb, int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly, string strdangvanban
            )
        {
            int currentPage = request.Page;
            string highlightResult = string.Empty;
            // luu trang dang xem vao session
            _session.InsertObject(AppConts.SessionSearchPageType, EnumSession.PageType.VBDen);
            _session.InsertObject(AppConts.SessionSearchPageValues, currentPage);

            //if (currentPage != 1)
            //{
            //    // luu trang dang xem vao session
            //    _session.InsertObject(AppConts.SessionPageType, EnumSession.PageType.VBDen);
            //    _session.InsertObject(AppConts.SessionPageValues, currentPage);
            //}

            int _SearchType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchType));

            IEnumerable<ListVanbandenViewModel> vbden;

            if (_SearchType == (int)EnumSession.SearchType.SearchVBDen)
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

                string _strdangvanban = _session.GetStringSearchValues("strdangvanban", strSearchValues);

                highlightResult = _strtrichyeu;

                vbden = _vanban.GetListVanbanden
                    (_strngaydencat, _idloaivb,
                    _idkhoiph, _idsovb, _xuly,
                    _intsodenbd, _intsodenkt, _strngaydenbd, _strngaydenkt,
                    _strngaykybd, _strngaykykt, _strsokyhieu, _strnguoiky,
                    _strnoigui, _strtrichyeu, _strnguoixuly, _strdangvanban
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
                    strnoigui, strtrichyeu, strnguoixuly, strdangvanban
                    );
            }

            DataSourceResult result = vbden.OrderByDescending(p => p.dtengayden)
                                            .ThenByDescending(p => p.intsoden)
                                            .ToDataSourceResult(request);

            foreach (ListVanbandenViewModel item in result.Data)
            {
                // chi xet y kien cua nhung van ban da duyet
                if (item.inttrangthai == (int)enumVanbanden.inttrangthai.Daduyet)
                {   // chi xet y kien cua nhung van ban dang xu ly
                    if (item.inttinhtrangxuly == (int)enumHosocongviec.inttrangthai.Dangxuly)
                    {
                        item.isykien = _vanban.GetYkienvanbanden(item.intidhoso);
                    }
                }
                // add highlight result
                //if (!string.IsNullOrEmpty(highlightResult))
                //{
                //    Dictionary<bool, string> keyHighlight = ValidateData.SearchExactly(highlightResult);
                //    if (keyHighlight.ContainsKey(true))
                //    {   // co tim kiem chinh xac "abc"   
                //        highlightResult = keyHighlight[true];
                //    }
                //    string[] keywords = highlightResult.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                //    foreach (string key in keywords)
                //    {
                //        //item.strtrichyeu = item.strtrichyeu.Replace(key, "<span class='HighLight'>" + key + "</span>");
                //        item.strtrichyeu = System.Text.RegularExpressions.Regex.Replace(item.strtrichyeu,
                //                            key, "<span class='HighLight'>" + key + "</span>",
                //                            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //    }
                //}
            }


            return Json(result);
        }

        [OutputCache(Duration = 1000, VaryByParam = "none")]
        public ActionResult _SearchVBDen()
        {
            SearchVBViewModel model = _vanban.GetViewSearch();
            return PartialView(model);
        }

        /// <summary>
        /// lay idhosocongviec cua van ban dang xem
        /// de hien thi nut Xu ly
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetIdHosocongviec(int idvanban)
        {
            int idhosocongviec = _vanban.GetIdHosocongviec(idvanban);
            return Json(idhosocongviec);
        }

        /// <summary>
        /// lay idhosocongviec cua van ban dang chon
        /// de hien thi menu phai
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetIdHosoCV(int idvanban)
        {
            int idhosocongviec = _vanban.GetIdHosoCV(idvanban);
            return Json(idhosocongviec);
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

        #region DuyetVanban

        [HttpPost]
        public ActionResult _AjaxDuyetVanban(int id)
        {
            ResultFunction kq = _vanban.DuyetVanban(id, (int)enumVanbanden.inttrangthai.Daduyet);
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
            ResultFunction kq = _vanban.DuyetVanban(id, (int)enumVanbanden.inttrangthai.Chuaduyet);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json((int)kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }
        #endregion DuyetVanban

        #region UpdateVanban


        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Themvanban(int? id, int? idmail)
        {
            ThemVanbanViewModel vanban = new ThemVanbanViewModel();
            //vanban.idmail = idmail;
            //ViewBag.IsSucess = true;
            //if ((idmail != 0) || (idmail != null))
            //{   // cap nhat van ban dien tu
            //    vanban = _vanban.GetLoaitruong(0, 0, 0);
            //}
            //else
            //{   // them moi van ban den binh thuong
            //    vanban = _vanban.GetLoaitruong(0, 0, id);
            //}
            vanban = _vanban.GetLoaitruong(0, 0, id, idmail);

            return View(vanban);
        }

        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [HttpPost]
        public ActionResult Themvanban(int? idloaivb, int? intAddNext, int? idmail,
            FormCollection collection, ThemVanbanViewModel vb)
        {
            ThemVanbanViewModel vanban = vb;
            //vanban.Vanbanden = vb.Vanbanden;

            string strgiatri = "";
            foreach (string p in collection)
            {   // lay cac gia tri tu form dien vao control 
                strgiatri = collection[p];
                if (!string.IsNullOrEmpty(strgiatri))
                {
                    if (p == "strngayden") { vanban.Vanbanden.strngayden = DateServices.FormatDateEn(collection[p]); }
                    if (p == "strngayky") { vanban.Vanbanden.strngayky = DateServices.FormatDateEn(collection[p]); }
                    if (p == "strhanxuly") { vanban.Vanbanden.strhanxuly = DateServices.FormatDateEn(collection[p]); }

                    //if (p == "strnoiphathanh") { vanban.Vanbanden.strnoiphathanh = collection[p]; }
                    if (p == "strnoiphathanh-auto") { vanban.Vanbanden.strnoiphathanh = collection[p]; }
                    if (p == "strkyhieu") { vanban.Vanbanden.strkyhieu = collection[p]; }

                    if (p == "strnoinhan") { vanban.Vanbanden.strnoinhan = collection[p]; }

                    if (p == "idsovanban") { vanban.Vanbanden.intidsovanban = Convert.ToInt32(strgiatri); }
                    if (p == "idloaivanban") { vanban.Vanbanden.intidphanloaivanbanden = Convert.ToInt32(strgiatri); }
                    if (p == "idkhoiphathanh") { vanban.Vanbanden.intidkhoiphathanh = Convert.ToInt32(strgiatri); }
                    if (p == "idnguoiduyet") { vanban.Vanbanden.intidnguoiduyet = Convert.ToInt32(strgiatri); }

                    if (p == "idmail" && !string.IsNullOrEmpty(strgiatri))
                    {
                        vanban.Vanbanden.intidvanbandenmail = Convert.ToInt32(strgiatri);
                    }

                    if (p == "IsQPPL")
                    {
                        if (strgiatri.ToLower().Contains("true"))
                        {
                            vanban.IsQPPL = true;
                            vanban.Vanbanden.intquyphamphapluat = (int)enumVanbanden.intquyphamphapluat.Co;
                        }
                        else
                        {
                            vanban.IsQPPL = false;
                            vanban.Vanbanden.intquyphamphapluat = (int)enumVanbanden.intquyphamphapluat.Khong;
                        }
                    }

                }
            }

            if (intAddNext == 1)
            {   // them tiep: giu nguyen cac truong chi tang so van ban den
                vanban = _vanban.GetLoaitruong(idloaivb, vanban.Vanbanden.intidsovanban, 0, 0);
                vanban.IsSave = false;
                vanban.Vanbanden = vb.Vanbanden;
                AjaxSovanban sovb = _vanban.GetSovanban(vanban.Vanbanden.intidsovanban);
                vanban.Vanbanden.intsoden = sovb.intsoden;
                vanban.Vanbanden.intidkhoiphathanh = sovb.idkhoiph;
                // reset cac truong
                vanban.Vanbanden.intid = 0;
                vanban.Vanbanden.strngayden = DateTime.Now;
                vanban.idmail = idmail;
                vanban.Vanbanden.intidvanbandenmail = idmail;
                ModelState.Clear();
                return View(vanban);
                //return RedirectToAction("Themvanban", "Vanbanden", new { id = string.Empty });
            }

            // thay doi loai van ban, giu lai cac gia tri tren form
            if (idloaivb != 0)
            {
                vanban = _vanban.GetLoaitruong(idloaivb, vanban.Vanbanden.intidsovanban, 0, idmail);
                vanban.IsSave = false;
                vanban.Vanbanden = vb.Vanbanden;
            }
            else
            {   // submit form
                // ghi nhan van ban

                // kiem tra dieu kien ghi nhan
                if (vb.Vanbanden.intid == 0)
                {   // them moi                    
                    int intidvanban = _vanban.Savevanban(vanban);

                    vanban = _vanban.GetLoaitruong(0, 0, intidvanban, idmail);
                    vanban.IsSave = (intidvanban != 0) ? true : false;

                    if (vanban.Vanbanden.intidvanbandenmail != null && intidvanban != 0)
                    {
                        //_edxmlManager.SendStatus(intidvanban, "03", "Đã tiếp nhận", null, null);

                        _truclienthongtinhManager.SendStatus(intidvanban, "03", "Đã Tiếp Nhận", null, null);
                    }
                
                }
                else
                {   // cap nhat
                    bool kq = _vanban.Editvanban(vb.Vanbanden.intid, vb.Vanbanden);

                    vanban = _vanban.GetLoaitruong(0, 0, vb.Vanbanden.intid, idmail);
                    vanban.IsSave = kq;
                }
            }

            return View(vanban);
        }

        public ActionResult _AjaxGetTenDonvi(int idkhoiph)
        {
            var donvi = _vanban.GetTenDonvi(idkhoiph);
            return Json(donvi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult _AjaxGetSovb(int idsovb)
        {
            AjaxSovanban sovb = _vanban.GetSovanban(idsovb);
            return Json(sovb);
        }

        [HttpPost]
        public ActionResult _AjaxKiemtraVBtrung(string strsokyhieu, string strngayky, string strcoquan, int idmail)
        {
            CheckVBTrungViewModel kq = _vanban.KiemtraVBtrung(strsokyhieu, strngayky, strcoquan, idmail);
            return Json(kq);
        }

        [HttpPost]
        public ActionResult _CapnhatThoigianXulyVBDT(int idvanban)
        {
            ResultFunction kq = _vanban.CapnhatThoihanXulyVBDT(idvanban);
            return Json(kq);
        }
        [HttpPost]
        public ActionResult _CapnhatVBDTDinhkem(int idvanban, int idmail)
        {
            ResultFunction kq = _vanban.CapnhatVBDTDinhkem(idvanban, idmail);
            return Json(kq);
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
        #endregion UpdateVanban

        #region Capquyenxem

        [OutputCache(Duration = 2000, VaryByParam = "none")]
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

        public ActionResult _UploadVBDen(int idvanban)
        {
            UploadVBDenViewModel model = _vanban.GetListFile(idvanban);

            return PartialView(model);
        }

        #endregion UploadFile

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

            string _strdangvanban = _session.GetStringSearchValues("strdangvanban", strSearchValues);

            vanban = new List<ListVanbandenViewModel>(_vanban.GetListVanbanden
                    (_strngaydencat, _idloaivb,
                    _idkhoiph, _idsovb, "",
                    _intsodenbd, _intsodenkt, _strngaydenbd, _strngaydenkt,
                    _strngaykybd, _strngaykykt, _strsokyhieu, _strnguoiky,
                    _strnoigui, _strtrichyeu, _strnguoixuly, _strdangvanban
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

        #region PhanXLNhieuVB

        public ActionResult PhanXLNhieuVB()
        {
            return View();
        }

        public ActionResult _ListPhanXLNhieuVB()
        {
            return PartialView();
        }

        public ActionResult _FormPhanXLNhieuVB()
        {
            FormPhanXLNhieuVBViewModel model = _vanban.GetFormPhanXLNhieuVB();
            return PartialView(model);
        }
        #endregion PhanXLNhieuVB

        #region PhanXLNhieuVB_2(rightmenu)

        //[HttpPost]
        public ActionResult PhanXLNhieuVBDen(string listid)
        {
            ViewBag.listid = listid;
            return View();
        }
        public ActionResult _ListPhanXLNhieuVBDen(string listid)
        {
            return PartialView();
        }

        public ActionResult PhanXLNhieuVanbanden_Read
            ([DataSourceRequest]DataSourceRequest request, string listid)
        {

            var vbden = _vanban.GetListPhanXLNhieuVB(listid);

            DataSourceResult result = vbden.OrderByDescending(p => p.dtengayden)
                                            .ThenByDescending(p => p.intsoden)
                                            .ToDataSourceResult(request);
            return Json(result);
        }

        #endregion PhanXLNhieuVB_2


        #region ListVBDenLienquan

        public ActionResult _SearchVBDenLQ(int idhoso)
        {
            SearchVBViewModel model = _vanban.GetViewSearch();
            ViewBag.idhoso = idhoso;
            return PartialView(model);
        }

        public ActionResult ListVBDenLienquan(int idhoso, SearchVBViewModel model)
        {
            model.isSearch = (model.isSearch == true) ? true : false;
            ViewBag.idhoso = idhoso;
            return View(model);
        }

        public ActionResult Vanbandenlienquan_Read
            ([DataSourceRequest]DataSourceRequest request, int idhoso,
            bool? isSearch,
            string strngaydencat, int? idloaivb, int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly
            )
        {
            int currentPage = request.Page;

            // luu trang dang xem vao session
            _session.InsertObject(AppConts.SessionSearchPageType, EnumSession.PageType.VBDenLQ);
            _session.InsertObject(AppConts.SessionSearchPageValues, currentPage);

            //if (currentPage != 1)
            //{
            //    // luu trang dang xem vao session
            //    _session.InsertObject(AppConts.SessionPageType, EnumSession.PageType.VBDen);
            //    _session.InsertObject(AppConts.SessionPageValues, currentPage);
            //}

            int _SearchType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchType));
            if ((_SearchType == (int)EnumSession.SearchType.SearchVBDenLQ) && (isSearch == true))
            {
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

                return Json(_vanban.GetListVanbandenlienquan
                    (idhoso,
                    _strngaydencat, _idloaivb,
                    _idkhoiph, _idsovb, xuly,
                    _intsodenbd, _intsodenkt, _strngaydenbd, _strngaydenkt,
                    _strngaykybd, _strngaykykt, _strsokyhieu, _strnguoiky,
                    _strnoigui, _strtrichyeu, _strnguoixuly
                    )
                    .OrderByDescending(p => p.dtengayden)
                    .ThenByDescending(p => p.intsoden)
                    .ToDataSourceResult(request));
            }
            else
            {
                // khong co luu tim kiem
                return Json(_vanban.GetListVanbandenlienquan
                    (idhoso,
                    strngaydencat, idloaivb,
                    idkhoiph, idsovb, xuly,
                    intsodenbd, intsodenkt, strngaydenbd, strngaydenkt,
                    strngaykybd, strngaykykt, strsokyhieu, strnguoiky,
                    strnoigui, strtrichyeu, strnguoixuly
                    )
                    .OrderByDescending(p => p.dtengayden)
                    .ThenByDescending(p => p.intsoden)
                    .ToDataSourceResult(request));
            }
        }

        [HttpPost]
        public ActionResult _SaveVBDenLienquan(string stridvanban, int idhosocongviec)
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
                int kq = _vanban.SaveVBDenLienquan(listidvanban, idhosocongviec);
                return Json(kq);
            }
            else
            {
                return Json(0);
            }

        }

        public ActionResult HosoVBDenLQ_Read(
            [DataSourceRequest]DataSourceRequest request,
            int idhoso)
        {
            return Json(_vanban.GetHosoVBDenLQ(idhoso)
                    .OrderByDescending(p => p.dtengayden)
                    .ThenByDescending(p => p.intsoden)
                    .ToDataSourceResult(request));
        }


        #endregion ListVBDenLienquan

        #region Email

        public ActionResult Email(int id)
        {
            QLVB.DTO.Vanbandi.ListEmailDonviViewModel model = _vanban.GetListEmailDonvi(id);
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
                    if (p != "parentcheckbox")
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
                ResultFunction kq = _mail.GuiVBDT(idvanban, listSendDonvi, (int)enumGuiVanban.intloaivanban.Vanbanden, intAutoSend);
                return Json(kq);
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
            var kq = _mail.SendEmailKhac((int)enumGuiVanban.intloaivanban.Vanbanden,
                    idvanbanEmailKhac, donviEmailKhac, strtieudeEmailKhac, strnoidungEmailKhac);
            return Json(kq);
        }

        #endregion Email


    }
}
