using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using QLVB.Common.Sessions;
using QLVB.Common.Utilities;
using QLVB.DTO.Vanbandi;
using Store.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLVB.WebUI.Controllers.Store
{
    public class TracuuVanbandiController : Controller
    {
        #region Constructor
        private ITracuuVanbandiManager _vanban;
        private ISessionServices _session;


        public TracuuVanbandiController(ITracuuVanbandiManager vanban, ISessionServices session)
        {
            _vanban = vanban;
            _session = session;

        }

        #endregion Constructor

        #region ViewIndex
        public ActionResult Index(bool? isBack)
        {
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
            QLVB.DTO.TracuuVanbandi.ToolbarViewModel tbar = _vanban.GetToolbar();
            return PartialView(tbar);
        }


        public ActionResult _ListVanbandi(
            bool? isSearch, bool? isBack,
            string strngaykycat, int? idloaivb, int? idsovb, string strvbphathanh,
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
            string strngaykycat, int? idloaivb, int? idsovb, string strvbphathanh,
            int? intsobd, int? intsokt, string strngaykybd, string strngaykykt,
            string strkyhieu, string strnguoiky, string strnguoisoan, string strnguoiduyet,
            string strnoinhan, string strtrichyeu, string strhantraloi, string strdonvisoan,
             int? idkhan, int? idmat
            )
        {
            int currentPage = request.Page;
            string highlightResult = string.Empty;
            // luu trang dang xem vao session
            //_session.InsertObject(AppConts.SessionSearchPageType, EnumSession.PageType.VBDi);
            //_session.InsertObject(AppConts.SessionSearchPageValues, currentPage);

            IEnumerable<ListVanbandiViewModel> vbdi;

            int _SearchType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchType));
            if (_SearchType == (int)EnumSession.SearchType.SearchTracuuVBDi)
            {
                // lay cac gia tri search trong session
                string strSearchValues = _session.GetObject(AppConts.SessionSearchTypeValues).ToString();

                int _idloaivb = _session.GetIntSearchValues("idloaivb", strSearchValues);
                int _idsovb = _session.GetIntSearchValues("idsovb", strSearchValues);
                string _strngaykycat = _session.GetStringSearchValues("strngaykycat", strSearchValues);
                string _strvbphathanh = _session.GetStringSearchValues("strvbphathanh", strSearchValues);

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
                        _strngaykycat, _idloaivb, _idsovb, _strvbphathanh,
                        _intsobd, _intsokt, _strngaykybd, _strngaykykt,
                        _strkyhieu, _strnguoiky, _strnguoisoan, _strnguoiduyet,
                        _strnoinhan, _strtrichyeu, _strhantraloi, _strdonvisoan,
                        _idkhan, _idmat
                    );
            }
            else
            {
                highlightResult = strtrichyeu;
                // khong co luu tim kiem
                vbdi = _GetListvanban(
                        strngaykycat, idloaivb, idsovb, strvbphathanh,
                        intsobd, intsokt, strngaykybd, strngaykykt,
                        strkyhieu, strnguoiky, strnguoisoan, strnguoiduyet,
                        strnoinhan, strtrichyeu, strhantraloi, strdonvisoan,
                        idkhan, idmat
                    );
            }
            DataSourceResult result = vbdi.OrderByDescending(p => p.dtengayky)
                                            .ThenByDescending(p => p.intso)
                                            .ToDataSourceResult(request);


            return Json(result);
        }

        private IEnumerable<ListVanbandiViewModel> _GetListvanban(
            string strngaykycat, int? idloaivb, int? idsovb, string strvbphathanh,
            int? intsobd, int? intsokt, string strngaykybd, string strngaykykt,
            string strkyhieu, string strnguoiky, string strnguoisoan, string strnguoiduyet,
            string strnoinhan, string strtrichyeu, string strhantraloi, string strdonvisoan,
             int? idkhan, int? idmat
            )
        {
            return _vanban.GetListVanbandi(
                    strngaykycat, idloaivb, idsovb, strvbphathanh,
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




    }
}