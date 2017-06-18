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
using QLVB.DTO.Hoso;
using QLVB.Common.Utilities;
using QLVB.Common.Logging;
using QLVB.Common.Date;
using QLVB.Common.Sessions;

namespace QLVB.WebUI.Controllers
{
    public class HosoController : Controller
    {
        #region Constructor

        private IHosoManager _hoso;
        private ILogger _logger;
        private ISessionServices _session;
        private IVanbandenManager _vanban;
        private IEdxmlManager _edxmlManager;
        public HosoController(IHosoManager hoso, ILogger logger, ISessionServices session, IEdxmlManager edxmlManager, IVanbandenManager vanban)
        {
            _hoso = hoso;
            _logger = logger;
            _session = session;
            _edxmlManager = edxmlManager;
            _vanban = vanban;
        }

        #endregion Constructor

        #region Index

        public ActionResult Index(bool? isBack)
        {
            ViewBag.idnguoinhap = _session.GetUserId();
            if (isBack == true)
            {
                // luu session tim kiem
                //SessionService.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchVBDen);
            }
            else
            {   // khong co isBack (nhan vao menu hoso)
                // thi reset session ve Nosearch
                isBack = false;
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            }
            ViewBag.isBack = isBack;
            return View();
        }
        [ChildActionOnly]
        public ActionResult _ToolbarCongviec()
        {
            return PartialView();
        }

        public ActionResult _CategoryCongviec()
        {
            CategoryCongviecViewModel model = _hoso.GetCategoryCongviec();
            return PartialView(model);
        }

        public ActionResult _ListCongviec(
             bool? isSearch, bool? isBack,
            string strngayhscat, string strxuly, int? intsobd, int? intsokt, string strngayhsbd, string strngayhskt,
            string strtieude, string strhantraloi, int? idlinhvuc
            )
        {
            SearchCongviecViewModel model = new SearchCongviecViewModel();

            model.isSearch = isSearch == true ? true : false;
            model.isBack = isBack == true ? true : false;

            int intPage = 1;
            if (isBack == true)
            {   // tra ve page dang xem khi quay lai
                int _PageType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchPageType));
                if (_PageType == (int)EnumSession.PageType.HSCV)
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

            model.strngayhscat = strngayhscat;
            model.strxuly = strxuly;
            model.intsobd = intsobd;
            model.intsokt = intsokt;
            model.strngayhsbd = strngayhsbd;
            model.strngayhskt = strngayhskt;
            model.strtieude = strtieude;
            //model.strxulychinh = strxulychinh;
            model.strhantraloi = strhantraloi;
            model.idlinhvuc = idlinhvuc;

            return PartialView(model);
        }

        public ActionResult ListHoso_Read(
            [DataSourceRequest]DataSourceRequest request,
            string strngayhscat, string strxuly, int? intsobd, int? intsokt, string strngayhsbd, string strngayhskt,
            string strtieude, string strhantraloi, int? idlinhvuc
            )
        {
            int currentPage = request.Page;
            // luu trang dang xem vao session
            _session.InsertObject(AppConts.SessionSearchPageType, EnumSession.PageType.HSCV);
            _session.InsertObject(AppConts.SessionSearchPageValues, currentPage);

            int _SearchType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchType));

            IEnumerable<ListCongviecViewModel> hoso;

            if (_SearchType == (int)EnumSession.SearchType.SearchHSCV)
            {
                // lay cac gia tri search trong session
                string strSearchValues = _session.GetObject(AppConts.SessionSearchTypeValues).ToString();

                string _strngayhscat = _session.GetStringSearchValues("strngayhscat", strSearchValues);
                string _strxuly = _session.GetStringSearchValues("strxuly", strSearchValues);

                int _intsobd = _session.GetIntSearchValues("intsobd", strSearchValues);
                int _intsokt = _session.GetIntSearchValues("intsokt", strSearchValues);

                string _strngayhsbd = _session.GetStringSearchValues("strngayhsbd", strSearchValues);
                string _strngayhskt = _session.GetStringSearchValues("strngayhskt", strSearchValues);

                string _strtieude = _session.GetStringSearchValues("strtieude", strSearchValues);
                string _strhantraloi = _session.GetStringSearchValues("strhantraloi", strSearchValues);
                //string _strnguoixuly = _session.GetStringSearchValues("strnguoixuly", strSearchValues);

                int? _idlinhvuc = _session.GetIntSearchValues("idlinhvuc", strSearchValues);

                strngayhscat = _strngayhscat;
                strxuly = _strxuly;
                intsobd = _intsobd;
                intsokt = _intsokt;
                strngayhsbd = _strngayhsbd;
                strngayhskt = _strngayhskt;
                strtieude = _strtieude;
                strhantraloi = _strhantraloi;
                idlinhvuc = _idlinhvuc;

            }

            hoso = _GetListCongviec(
                 strngayhscat, strxuly, intsobd, intsokt, strngayhsbd, strngayhskt,
                 strtieude, strhantraloi, idlinhvuc);


            DataSourceResult result = hoso
                                .OrderByDescending(p => p.dtengayhs)
                                .ThenByDescending(p => p.intso)
                                .ToDataSourceResult(request);

            return Json(result);
        }

        private IEnumerable<ListCongviecViewModel> _GetListCongviec(
            string strngayhscat, string strxuly, int? intsobd, int? intsokt, string strngayhsbd, string strngayhskt,
            string strtieude, string strhantraloi, int? idlinhvuc
            )
        {

            return _hoso.GetListCongviec(
                    strngayhscat, strxuly, intsobd, intsokt, strngayhsbd, strngayhskt,
                    strtieude, strhantraloi, idlinhvuc);
        }

        public ActionResult SearchHoso()
        {
            SearchCongviecViewModel model = _hoso.GetViewSearch();
            return PartialView(model);
        }

        public ActionResult _formDeleteHoso()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult DeleteHoso(int id)
        {
            ResultFunction kq = _hoso.DeleteHoso(id);
            return Json(kq.id);
        }
        #endregion Index

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

        #region PhanXLVB
        /// <summary>
        /// phan xu ly van ban
        /// </summary>
        /// <param name="id">idvanban</param>
        /// <returns></returns>
        public ActionResult PhanXLVB(int id)
        {
            PhanXLVBViewModel model = _hoso.GetFormPhanXLVB(id);
            if (model.IsQuytrinh)
            {
                ViewBag.message = "Không phân xử lý cho hồ sơ đã có quy trình";
                return View("ErrorHoso");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult PhanXLVB(int idvanban, FormCollection collection)
        {
            PhanXLVBViewModel hoso = new PhanXLVBViewModel();
            hoso.HosocongviecModel = new Hosocongviec();
            hoso.intidvanban = idvanban;

            string strgiatri = "";
            foreach (string p in collection)
            {
                strgiatri = collection[p];
                if (!string.IsNullOrEmpty(strgiatri))
                {   // lay doi tuong xu ly 
                    if (p == "idhosocongviec") { hoso.HosocongviecModel.intid = Convert.ToInt32(strgiatri); }
                    //if (p == "idvanban") { hoso.intidvanban = Convert.ToInt32(strgiatri); }

                    if (p == "strtieude") { hoso.HosocongviecModel.strtieude = strgiatri; }
                    if (p == "idlinhvuc") { hoso.HosocongviecModel.intlinhvuc = Convert.ToInt32(strgiatri); }

                    if (p == "strnoidung") { hoso.HosocongviecModel.strnoidung = strgiatri; }

                    if (p == "idlanhdaogiaoviec") { hoso.intidlanhdaogiaoviec = Convert.ToInt32(strgiatri); }
                    // chon chuoi stridlanhdaophutrach
                    if (p == "idlanhdaophutrach") { hoso.stridlanhdaophutrach = strgiatri; }

                    if (p == "idxulychinh") { hoso.intidxulychinh = Convert.ToInt32(strgiatri); }
                    if (p == "strthoihanxuly") { hoso.HosocongviecModel.strthoihanxuly = DateServices.FormatDateEn(strgiatri); }

                    if (p == "IsDongHoso")
                    {
                        hoso.HosocongviecModel.intdonghoso = strgiatri.ToLower().Contains("true") ?
                                (int)enumHosocongviec.intdonghoso.Co : (int)enumHosocongviec.intdonghoso.Khong;
                    }
                }
                if (p == "strngaymohoso")
                {
                    hoso.HosocongviecModel.strngaymohoso = (!string.IsNullOrWhiteSpace(strgiatri)) ? DateServices.FormatDateEn(strgiatri) : DateTime.Now;
                }
            } // het duyet form collection


            ResultFunction kq = _hoso.SavePhanXLVB(idvanban, hoso);

            hoso.IsSave = (kq.id == (int)ResultViewModels.Success) ? true : false;

            hoso = _hoso.GetFormPhanXLVB(idvanban);

            return View(hoso);
        }


        [HttpPost]
        public ActionResult PhanXLNhieuVB(string idvanban, FormCollection collection)
        {
            List<int> listidvanban = new List<int>();

            // kiem tra chuoi idvanban khac rong
            if (!string.IsNullOrEmpty(idvanban))
            {
                string[] stridvanban = idvanban.Split(new Char[] { ',' });
                foreach (var p in stridvanban)
                {
                    if (!string.IsNullOrEmpty(p))
                    {
                        int id = Convert.ToInt32(p);
                        listidvanban.Add(id);
                    }
                }
            }
            else
            {
                return RedirectToAction("PhanXLNhieuVBDen", "Vanbanden", new { listid = idvanban });
            }

            PhanXLVBViewModel hoso = new PhanXLVBViewModel();
            hoso.HosocongviecModel = new Hosocongviec();

            string strykienxuly = string.Empty;

            //hoso.intidvanban = idvanban;

            string strgiatri = "";
            foreach (string p in collection)
            {
                strgiatri = collection[p];
                if (!string.IsNullOrEmpty(strgiatri))
                {   // lay doi tuong xu ly

                    if (p == "strnoidung") { strykienxuly = strgiatri; }

                    if (p == "idlanhdaogiaoviec") { hoso.intidlanhdaogiaoviec = Convert.ToInt32(strgiatri); }
                    // chon chuoi stridlanhdaophutrach
                    if (p == "idlanhdaophutrach") { hoso.stridlanhdaophutrach = strgiatri; }

                    if (p == "idxulychinh") { hoso.intidxulychinh = Convert.ToInt32(strgiatri); }

                    if (p == "strthoihanxuly") { hoso.HosocongviecModel.strthoihanxuly = DateServices.FormatDateEn(strgiatri); }

                    if (p == "strngaymohoso") { hoso.HosocongviecModel.strngaymohoso = DateServices.FormatDateEn(strgiatri); }
                }
            } // het duyet form collection

            _hoso.SavePhanXLNhieuVB(listidvanban, strykienxuly, hoso);

            return RedirectToAction("PhanXLNhieuVBDen", "Vanbanden", new { listid = idvanban });

        }

        public ActionResult ThemHoso(int? id)
        {
            id = (id > 0) ? id : 0;
            PhanXLVBViewModel model = _hoso.GetFormThemHoso((int)id);
            return View(model);
        }
        [HttpPost]
        public ActionResult ThemHoso(FormCollection collection)
        {

            PhanXLVBViewModel hoso = new PhanXLVBViewModel();
            hoso.HosocongviecModel = new Hosocongviec();

            string strgiatri = "";
            foreach (string p in collection)
            {
                strgiatri = collection[p];
                if (!string.IsNullOrEmpty(strgiatri))
                {   // lay doi tuong xu ly 
                    if (p == "idhosocongviec") { hoso.HosocongviecModel.intid = Convert.ToInt32(strgiatri); }
                    //if (p == "idvanban") { hoso.intidvanban = Convert.ToInt32(strgiatri); }

                    if (p == "strtieude") { hoso.HosocongviecModel.strtieude = strgiatri; }
                    if (p == "idlinhvuc") { hoso.HosocongviecModel.intlinhvuc = Convert.ToInt32(strgiatri); }

                    if (p == "strnoidung") { hoso.HosocongviecModel.strnoidung = strgiatri; }

                    if (p == "idlanhdaogiaoviec") { hoso.intidlanhdaogiaoviec = Convert.ToInt32(strgiatri); }
                    // chon chuoi stridlanhdaophutrach
                    if (p == "idlanhdaophutrach") { hoso.stridlanhdaophutrach = strgiatri; }

                    if (p == "idxulychinh") { hoso.intidxulychinh = Convert.ToInt32(strgiatri); }
                    if (p == "strthoihanxuly") { hoso.HosocongviecModel.strthoihanxuly = DateServices.FormatDateEn(strgiatri); }
                }
                if (p == "strngaymohoso")
                {
                    hoso.HosocongviecModel.strngaymohoso = (!string.IsNullOrWhiteSpace(strgiatri)) ? DateServices.FormatDateEn(strgiatri) : DateTime.Now;
                }
            } // het duyet form collection


            ResultFunction kq = _hoso.SaveHoso(hoso);

            hoso.IsSave = (kq.id == (int)ResultViewModels.Success) ? true : false;

            hoso = _hoso.GetFormThemHoso(hoso.HosocongviecModel.intid);

            return View(hoso);

        }

        public JsonResult GetCanbothuchien(int? iddonvi)
        {
            if (iddonvi != null)
            {
                var canbo = _hoso.GetListCanbo((int)iddonvi);
                return Json(canbo, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        #endregion PhanXLVB

        #region XulyHoso

        #region Thongtinchung

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="intBack">
        /// 1: vanbanden
        /// 2: tinh hinh xu ly       
        /// 
        /// </param>
        /// <returns></returns>
        public ActionResult XulyHoso(int id, int? intBack)
        {
            // kiem tra user co quyen xu ly hs
            if (_hoso.IsXulyHoso(id))
            {
                ViewBag.idhosocongviec = id;
                ViewBag.intBack = intBack;
                return View();
            }
            else
            {
                _logger.Warn(AppConts.ErrLog + " hồ sơ: " + id.ToString());
                return new ViewResult { ViewName = AppConts.ErrAccessDenied };
            }
        }

        [ChildActionOnly]
        public ActionResult _ToolbarXuly(int id, int? intBack)
        {
            ToolbarXulyViewModel tbar = _hoso.GetToolbarXuly(id, intBack);
            return PartialView(tbar);
        }


        /// <summary>
        /// danh sach tat ca nguoi dang xu ly cua ho so : 
        /// Lanh dao giao viec, lanh dao phu trach, xu ly chinh, phoi hop xu ly
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns></returns>

        public ActionResult _DanhsachNguoixuly(int idhoso)
        {
            var canbo = _hoso.GetDanhsachNguoixuly(idhoso);
            return PartialView(canbo);
        }

        /// <summary>
        ///  cac thong tin chung cua ho so
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns></returns>

        public ActionResult _ThongtinHoso(int idhoso)
        {
            ThongtinHosoViewModel model = _hoso.GetThongtinHoso(idhoso);
            return PartialView(model);
        }

        /// <summary>
        /// cac thong tin, qua trinh xu ly ho so
        /// </summary>
        /// <param name="idhoso"></param>
        /// <returns></returns>

        public ActionResult _ThongtinXuly(int idhoso)
        {
            DetailHosoViewModel model = _hoso.GetDetailHoso(idhoso);
            if (model.idhosocongviec == 0)
            {
                return new ViewResult { ViewName = AppConts.ErrAccessDenied };
            }
            else
            {
                return PartialView(model);
            }
        }

        public ActionResult _HosoVBDenLQ(int idhoso)
        {
            ViewBag.idhoso = idhoso;
            return PartialView();
        }

        public ActionResult _HosoVBDiLQ(int idhoso)
        {
            ViewBag.idhoso = idhoso;
            return PartialView();
        }

        #endregion Thongtinchung

        #region Phoihopxuly

        public ActionResult _ViewPhoihopxuly()
        {
            return PartialView();
        }

        public ActionResult _Phoihopxuly(int idhosocongviec)
        {
            PhoihopXulyViewModel user = _hoso.GetUserPhoihopxuly(idhosocongviec);
            return PartialView(user);
        }

        /// <summary>
        /// submit list idcanbo phoi hop xu ly
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _SavePhoihopxuly(int idhoso, FormCollection collection)
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

            ResultFunction kq = _hoso.SavePhoihopxuly(idhoso, listidcanbo);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json((int)kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        #endregion Phoihopxuly

        #region Ykien

        public ActionResult _AddYkien()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult _SaveYkien(int idhoso, string strykien)
        {
            ResultFunction kq = _hoso.SaveYkienxuly(idhoso, strykien);
            return Json(kq.id);
        }

        public ActionResult _YkienXulyNhanh(int idhoso)
        {
            ViewBag.idhoso = idhoso;
            return PartialView();
        }

        #endregion Ykien

        #region KetthucHoso

        [HttpPost]
        public ActionResult _LuuHoso(int idhoso)
        {
            ResultFunction kq = _hoso.LuuHoso(idhoso);
            
            

            if (kq.id == (int)ResultViewModels.Success)
            {
                
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        [HttpPost]
        public ActionResult _KetthucHoso(int idhoso)
        {
            ResultFunction kq = _hoso.HoanthanhHoso(idhoso);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        [HttpPost]
        public ActionResult _Trinhky(int idhoso)
        {
            ResultFunction kq = _hoso.Trinhky(idhoso);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }
        //==================== xu ly nhieu ho so =============
        [HttpPost]
        public ActionResult _LuuNhieuHoso(string listid)
        {
            List<int> listidvanban = new List<int>();

            // kiem tra chuoi idvanban khac rong
            if (!string.IsNullOrEmpty(listid))
            {
                string[] stridvanban = listid.Split(new Char[] { ',' });
                foreach (var p in stridvanban)
                {
                    if (!string.IsNullOrEmpty(p))
                    {
                        int id = Convert.ToInt32(p);
                        listidvanban.Add(id);
                    }
                }
            }

            ResultFunction kq = _hoso.LuuNhieuHoso(listidvanban);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        [HttpPost]
        public ActionResult _KetthucNhieuHoso(string listid)
        {
            List<int> listidvanban = new List<int>();

            // kiem tra chuoi idvanban khac rong
            if (!string.IsNullOrEmpty(listid))
            {
                string[] stridvanban = listid.Split(new Char[] { ',' });
                foreach (var p in stridvanban)
                {
                    if (!string.IsNullOrEmpty(p))
                    {
                        int id = Convert.ToInt32(p);
                        listidvanban.Add(id);
                    }
                }
            }

            ResultFunction kq = _hoso.HoanthanhNhieuHoso(listidvanban);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }


        #endregion KetthucHoso

        #region Phieutrinh

        public ActionResult _Phieutrinh(int idphieutrinh, int idhoso)
        {
            PhieutrinhViewModel model = _hoso.GetPhieutrinh(idphieutrinh, idhoso);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult _SavePhieutrinh(FormCollection collection)
        {
            PhieutrinhViewModel model = new PhieutrinhViewModel();
            string strgiatri = string.Empty;
            foreach (string p in collection)
            {
                strgiatri = collection[p];
                if (!string.IsNullOrEmpty(strgiatri))
                {
                    if (p == "idphieutrinh") { model.idphieutrinh = Convert.ToInt32(strgiatri); }
                    if (p == "idhoso_phieutrinh") { model.idhoso = Convert.ToInt32(strgiatri); }
                    if (p == "idlanhdao") { model.idlanhdaotrinh = Convert.ToInt32(strgiatri); }
                    if (p == "strnoidungtrinh") { model.strnoidungtrinh = strgiatri; }
                    if (p == "strykienchidao") { model.strykienchidao = strgiatri; }
                }
            }

            if (model.idphieutrinh == 0)
            {
                // them moi noi dung trinh                
                model.idphieutrinh = _hoso.SaveNoidungtrinh(model.idhoso, model.idlanhdaotrinh, model.strnoidungtrinh);
            }
            else
            {
                // cho y kien chi dao
                _hoso.SaveYkienchidao(model.idphieutrinh, model.idlanhdaotrinh, model.strykienchidao);
            }

            return Json(model.idphieutrinh);
        }
        #endregion Phieutrinh

        #region Quytrinh

        public ActionResult _ThongtinQuytrinh(int idhoso)
        {

            return PartialView();
        }

        [HttpPost]
        public ActionResult _KetthucBuocXuly(int idhoso)
        {
            ResultFunction kq = _hoso.KetthucBuocXuly(idhoso);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        public ActionResult _DisplayQuytrinh(int idhoso)
        {
            string jsFlowchart = _hoso.ReadFlowChart(idhoso, 0);
            ViewBag.jsFlowchart = jsFlowchart;

            return PartialView();
        }

        [HttpPost]
        public ActionResult _ReturnXuly(int idhoso)
        {
            ResultFunction kq = _hoso.ReturnBuocXuly(idhoso);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        [HttpPost]
        public ActionResult _TamngungQuytrinh(int idhoso, string strngay)
        {
            ResultFunction kq = _hoso.TamngungQuytrinh(idhoso, true, strngay);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        [HttpPost]
        public ActionResult _TieptucQuytrinh(int idhoso)
        {
            ResultFunction kq = _hoso.TamngungQuytrinh(idhoso, false, "");
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        public ActionResult _ChangeQuytrinh(int idhoso, int? update)
        {
            int intloai = 0;
            intloai = (update >= 0) ? (int)update : 0;
            ViewBag.intUpdate = intloai;

            string jsFlowchart = _hoso.ReadFlowChart(idhoso, intloai);
            ViewBag.jsFlowchart = jsFlowchart;

            return PartialView();
        }
        public ActionResult _EditHosoQuytrinhXuly(int idhoso, string NodeId)
        {
            EditHosoQuytrinhXulyViewModel model = _hoso.GetEditHosoQuytrinhXuly(idhoso, NodeId);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult _SaveHosoQuytrinhXuly(EditHosoQuytrinhXulyViewModel model)
        {
            ResultFunction kq = _hoso.UpdateHosoQuytrinhXuly(model);
            if (kq.id == (int)ResultViewModels.Success)
            {
                return Json(kq.id);
            }
            else
            {
                return Json(kq.message);
            }
        }

        [HttpPost]
        public ActionResult _ChonBuocXuly(int idhoso, string NodeId)
        {
            ResultFunction kq = _hoso.ChonBuocXuly(idhoso, NodeId);
            // tra ve jsonFlowchart
            return Json(kq.message);

        }


        #endregion Quytrinh

        #region PhathanhVB

        public ActionResult _DuthaoVB(int idhoso, string listfile)
        {
            PhathanhVBViewModel model = _hoso.GetPhathanhVanban(idhoso, listfile);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult _DuthaoVB(PhathanhVBViewModel model)
        {
            ResultFunction kq = _hoso.SaveVanbanPhathanh(model);
            return Json(kq.id);
        }

        #endregion PhathanhVB


        #endregion XulyHoso

        #region PhanQuytrinh

        /// <summary>
        /// thuc hien viec phan quy trinh xu ly vao van ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PhanQuytrinh(int id)
        {
            PhanQuytrinhViewModel model = _hoso.GetFormPhanQuytrinh(id);
            return View(model);

        }

        public JsonResult GetQuytrinh(int? idloai)
        {
            if (idloai != null)
            {
                var quytrinh = _hoso.GetQuytrinh((int)idloai);
                return Json(quytrinh, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult PhanQuytrinh(int idvanban, FormCollection collection)
        {
            PhanQuytrinhViewModel hoso = new PhanQuytrinhViewModel();
            hoso.HosocongviecModel = new Hosocongviec();
            hoso.intidvanban = idvanban;

            string strgiatri = "";
            foreach (string p in collection)
            {
                strgiatri = collection[p];
                if (!string.IsNullOrEmpty(strgiatri))
                {
                    // lay quy trinh
                    if (p == "intidquytrinh") { hoso.intidquytrinh = Convert.ToInt32(strgiatri); }
                    if (p == "intidloaiquytrinh") { hoso.intidloaiquytrinh = Convert.ToInt32(strgiatri); }
                    // lay doi tuong xu ly 
                    if (p == "idhosocongviec") { hoso.HosocongviecModel.intid = Convert.ToInt32(strgiatri); }
                    //if (p == "idvanban") { hoso.intidvanban = Convert.ToInt32(strgiatri); }

                    if (p == "strtieude") { hoso.HosocongviecModel.strtieude = strgiatri; }
                    if (p == "idlinhvuc") { hoso.HosocongviecModel.intlinhvuc = Convert.ToInt32(strgiatri); }

                    if (p == "strnoidung") { hoso.HosocongviecModel.strnoidung = strgiatri; }

                    //if (p == "idlanhdaogiaoviec") { hoso.intidlanhdaogiaoviec = Convert.ToInt32(strgiatri); }
                    //// chon chuoi stridlanhdaophutrach
                    //if (p == "idlanhdaophutrach") { hoso.stridlanhdaophutrach = strgiatri; }

                    //if (p == "idxulychinh") { hoso.intidxulychinh = Convert.ToInt32(strgiatri); }
                    //if (p == "strthoihanxuly") { hoso.HosocongviecModel.strthoihanxuly = DateServices.FormatDateEn(strgiatri); }
                }
                if (p == "strngaymohoso")
                {
                    hoso.HosocongviecModel.strngaymohoso = (!string.IsNullOrWhiteSpace(strgiatri)) ? DateServices.FormatDateEn(strgiatri) : DateTime.Now;
                }
            } // het duyet form collection


            ResultFunction kq = _hoso.SavePhanQuytrinh(idvanban, hoso);

            hoso.IsSave = (kq.id == (int)ResultViewModels.Success) ? true : false;

            hoso = _hoso.GetFormPhanQuytrinh(idvanban);

            return View(hoso);
        }

        #endregion PhanQuytrinh




    }
}