using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Contract;
using QLVB.Common.Logging;
using QLVB.Common.Sessions;
using Store.DAL;
using QLVB.DTO.Vanbanden;
using QLVB.Domain.Entities;
using QLVB.DTO.File;
using QLVB.Common.Date;
using Store.DAL.Implementation;
using Store.DAL.Abstract;
using QLVB.Common.Utilities;
using QLVB.DTO;
using QLVB.Domain.Abstract;

namespace Store.Core.Implementation
{
    public class TracuuVanbandenManager : ITracuuVanbandenManager
    {
        #region Constructor

        private ILogger _logger;
        private ISessionServices _session;
        //=======Store==================
        private IStoreVanbandenRepository _vanbandenRepo;
        private IStoreHoibaovanbanRepository _hoibaovanbanRepo;

        private IStoreAttachVanbanRepository _fileRepo;
        private IStoreHosocongviecRepository _hosocongviecRepo;
        private IStoreHosovanbanRepository _hosovanbanRepo;
        private IStoreDoituongxulyRepository _doituongRepo;

        private IStoreVanbandiRepository _vanbandiRepo;

        private Store.Core.Contract.IStoreFileManager _fileManager;
        //==========QLVB =============
        private IPhanloaiVanbanRepository _plvanbanRepo;
        private ISoVanbanRepository _sovbRepo;
        private IKhoiphathanhRepository _khoiphRepo;
        private ICanboRepository _canboRepo;
        private IConfigRepository _configRepo;
        private ITinhchatvanbanRepository _tinhchatvbRepo;
        private QLVB.Core.Contract.IRoleManager _role;



        public TracuuVanbandenManager(ILogger logger, ISessionServices session, IStoreVanbandenRepository vanbandenRepo,
            IStoreHoibaovanbanRepository hoibaovanbanRepo,
            IStoreAttachVanbanRepository fileRepo, IStoreHosocongviecRepository hosocongviecRepo,
            IStoreHosovanbanRepository hosovanbanRepo, IStoreDoituongxulyRepository doituongRepo,
            IStoreVanbandiRepository vanbandiRepo,

            IPhanloaiVanbanRepository plvanbanRepo, ISoVanbanRepository sovbRepo,
            IKhoiphathanhRepository khoiphRepo, ICanboRepository canboRepo,
            ITinhchatvanbanRepository tinhchatvbRepo, Store.Core.Contract.IStoreFileManager fileManager,
            IConfigRepository configRepo, QLVB.Core.Contract.IRoleManager role
            )
        {
            _logger = logger;
            _session = session;
            _vanbandenRepo = vanbandenRepo;
            _hoibaovanbanRepo = hoibaovanbanRepo;
            _fileRepo = fileRepo;
            _hosocongviecRepo = hosocongviecRepo;
            _hosovanbanRepo = hosovanbanRepo;
            _doituongRepo = doituongRepo;
            _vanbandiRepo = vanbandiRepo;

            _plvanbanRepo = plvanbanRepo;
            _sovbRepo = sovbRepo;
            _khoiphRepo = khoiphRepo;
            _canboRepo = canboRepo;
            _tinhchatvbRepo = tinhchatvbRepo;
            _fileManager = fileManager;
            _configRepo = configRepo;
            _role = role;
        }

        #endregion Constructor

        #region Listvanban

        #region RawSql

        public IEnumerable<ListVanbandenViewModel> GetListVanbanden
            (string strngaydencat, int? idloaivb,
            int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly
            )
        {
            string strSearchValues = _SqlSearchVBDen
                (strngaydencat, idloaivb,
                idkhoiph, idsovb, xuly,
                intsodenbd, intsodenkt, strngaydenbd, strngaydenkt,
                strngaykybd, strngaykykt, strsokyhieu, strnguoiky,
                strnoigui, strtrichyeu, strnguoixuly
                );

            bool isViewVBDenDaXL = true;//_configRepo.GetConfigToBool(ThamsoHethong.IsViewVBDenDaXL);

            int _searchType = Convert.ToInt32(_session.GetObject(AppConts.SessionSearchType));
            if (_searchType == (int)EnumSession.SearchType.SearchTracuuVBDen)
            {
                isViewVBDenDaXL = true;
            }
            //====================================================
            // kiem tra nhung van ban user duoc quyen xem/xuly
            //====================================================
            bool isViewAllvb = _role.IsRole(RoleTracuuVanbanden.Xemtatcavb);

            int intloai_sql = AppSettings.LoaiVBDen;
            string strqueryvbden = string.Empty;
            switch (intloai_sql)
            {
                case 0:
                    strqueryvbden = _SqlGetAllVBden_v0(isViewVBDenDaXL, isViewAllvb, strSearchValues);
                    break;
                case 1:
                    strqueryvbden = _SqlGetAllVBden_v1(isViewVBDenDaXL, isViewAllvb, strSearchValues);
                    break;
                case 2:
                    strqueryvbden = _SqlGetAllVBden_v2(isViewVBDenDaXL, isViewAllvb, strSearchValues);
                    break;
            }

            string query = strqueryvbden; //_SqlGetAllVBden_v2(isViewVBDenDaXL, isViewAllvb, strSearchValues);

            IEnumerable<ListVanbandenViewModel> listvb = (IEnumerable<ListVanbandenViewModel>)_vanbandenRepo.RunSqlListVBDen(query);

            return listvb;

        }

        private string _SqlSearchVBDen(
            string strngaydencat, int? idloaivb,
            int? idkhoiph, int? idsovb, string xuly,
            int? intsodenbd, int? intsodenkt, string strngaydenbd, string strngaydenkt,
            string strngaykybd, string strngaykykt, string strsokyhieu, string strnguoiky,
            string strnoigui, string strtrichyeu, string strnguoixuly
            )
        {
            string strWhere = string.Empty;
            string query = string.Empty;

            bool isSearch = false;
            //bool isCategory = false;
            string strSearchValues = string.Empty;
            // strSearchValues = "intsodenbd=1;intsodenkt=10;idloaivb=2;"
            //===========================================================
            // kiem tra cac gia tri string search

            //====================================================
            // tuy chon category 
            //====================================================
            if (!string.IsNullOrEmpty(strngaydencat))
            {
                strngaydencat = ValidateData.CheckInput(strngaydencat);

                DateTime? dtengayden = DateServices.FormatDateEn(strngaydencat);
                query = " strngayden='" + DateServices.FormatDateEn(dtengayden) + "' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                //isCategory = true;
                strSearchValues += "strngaydencat=" + strngaydencat + ";";
            }
            if ((idloaivb != null) && (idloaivb != 0))
            {
                query = " intidphanloaivanbanden=" + idloaivb;
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                //isCategory = true;
                strSearchValues += "idloaivb=" + idloaivb.ToString() + ";";
            }
            if ((idsovb != null) && (idsovb != 0))
            {
                query = " intidsovanban=" + idsovb;
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                //isCategory = true;
                strSearchValues += "idsovb=" + idsovb.ToString() + ";";
            }
            if ((idkhoiph != null) && (idkhoiph != 0))
            {
                query = " intidkhoiphathanh=" + idkhoiph;
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                //isCategory = true;
                strSearchValues += "idkhoiph=" + idkhoiph.ToString() + ";";
            }
            // tinh trang xu ly
            //if (!string.IsNullOrEmpty(xuly))
            //{
            //    xuly = ValidateData.CheckInput(xuly);
            //    switch (xuly)
            //    {
            //        case "chuaduyet":
            //            query = " vanbanden.inttrangthai=" + (int)enumVanbanden.inttrangthai.Chuaduyet;
            //            break;
            //        case "daxuly":
            //            query = " hscv.inttrangthai=" + (int)enumHosocongviec.inttrangthai.Dahoanthanh;
            //            break;
            //        case "xulychinh":
            //            int idcanbo = _session.GetUserId();
            //            string strhoten = _canboRepo.GetAllCanbo.FirstOrDefault(p => p.intid == idcanbo).strhoten;
            //            query = " hscv.inttrangthai=" + (int)enumHosocongviec.inttrangthai.Dangxuly
            //                + " and strnoinhan like N'%" + strhoten + "%' ";
            //            break;
            //        case "phoihopxl":
            //            idcanbo = _session.GetUserId();
            //            strhoten = _canboRepo.GetAllCanbo.FirstOrDefault(p => p.intid == idcanbo).strhoten;
            //            query = " hscv.inttrangthai=" + (int)enumHosocongviec.inttrangthai.Dangxuly
            //                + " and strnoinhan not like N'%" + strhoten + "%' ";
            //            break;
            //    }
            //    if (string.IsNullOrEmpty(strWhere))
            //    {
            //        strWhere += query;
            //    }
            //    else
            //    {
            //        strWhere += " and " + query;
            //    }
            //    isSearch = true;
            //    isCategory = true;
            //    strSearchValues += "xuly=" + xuly + ";";
            //}

            //====================================================
            // Search van ban
            //====================================================
            if ((intsodenkt != null) && (intsodenkt != 0))
            {
                if ((intsodenbd != null) && (intsodenbd != 0))
                {
                    query = " vanbanden.intsoden>=" + intsodenbd + " and vanbanden.intsoden<=" + intsodenkt;
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "intsodenbd=" + intsodenbd.ToString() + ";intsodenkt=" + intsodenkt.ToString() + ";";
                }
            }
            else
            {
                if ((intsodenbd != null) && (intsodenbd != 0))
                {
                    query = " vanbanden.intsoden=" + intsodenbd;
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "intsodenbd=" + intsodenbd.ToString() + ";";
                }
            }

            if (!string.IsNullOrEmpty(strngaydenkt))
            {
                strngaydenkt = ValidateData.CheckInput(strngaydenkt);
                if (!string.IsNullOrEmpty(strngaydenbd))
                {
                    strngaydenbd = ValidateData.CheckInput(strngaydenbd);
                    DateTime? dtngaydenbd = DateServices.FormatDateEn(strngaydenbd);
                    DateTime? dtngaydenkt = DateServices.FormatDateEn(strngaydenkt);
                    query = " strngayden>='" + DateServices.FormatDateEn(dtngaydenbd) + "' and strngayden<='" + DateServices.FormatDateEn(dtngaydenkt) + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "strngaydenbd=" + strngaydenbd + ";strngaydenkt=" + strngaydenkt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngaydenbd))
                {
                    strngaydenbd = ValidateData.CheckInput(strngaydenbd);
                    DateTime? dtngaydenbd = DateServices.FormatDateEn(strngaydenbd);
                    query = " strngayden='" + DateServices.FormatDateEn(dtngaydenbd) + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "strngaydenbd=" + strngaydenbd + ";";
                }
            }

            if (!string.IsNullOrEmpty(strngaykykt))
            {
                if (!string.IsNullOrEmpty(strngaykybd))
                {
                    strngaykybd = ValidateData.CheckInput(strngaykybd);
                    strngaykykt = ValidateData.CheckInput(strngaykykt);

                    DateTime? dtngaykybd = DateServices.FormatDateEn(strngaykybd);
                    DateTime? dtngaykykt = DateServices.FormatDateEn(strngaykykt);
                    //vanban = vanban.Where(p => p.strngayky >= dtngaykybd)
                    //        .Where(p => p.strngayky <= dtngaykykt);
                    query = " strngayky>='" + DateServices.FormatDateEn(dtngaykybd) + "' and strngayky<='" + DateServices.FormatDateEn(dtngaykykt) + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "strngaykybd=" + strngaykybd + ";strngaykykt=" + strngaykykt + ";";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strngaykybd))
                {
                    strngaykybd = ValidateData.CheckInput(strngaykybd);
                    DateTime? dtngaykybd = DateServices.FormatDateEn(strngaykybd);
                    //vanban = vanban.Where(p => p.strngayky == dtngaykybd);
                    query = " strngayky='" + DateServices.FormatDateEn(dtngaykybd) + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                    isSearch = true;
                    strSearchValues += "strngaykybd=" + strngaykybd + ";";
                }
            }

            if (!string.IsNullOrEmpty(strsokyhieu))
            {
                strsokyhieu = ValidateData.CheckInput(strsokyhieu);
                // neu la so thi tim =
                // neu la chu thi tim like
                strsokyhieu = strsokyhieu.Trim();
                Dictionary<bool, string> result = ValidateData.SearchExactly(strsokyhieu);
                if (result.ContainsKey(true))
                {   // co tim kiem chinh xac "abc"
                    query = " strkyhieu = N'" + result[true] + "' ";
                }
                else
                {
                    query = " strkyhieu like N'%" + strsokyhieu + "%' ";
                }
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strsokyhieu=" + strsokyhieu + ";";
            }

            if (!string.IsNullOrEmpty(strtrichyeu))
            {
                strtrichyeu = ValidateData.CheckInput(strtrichyeu);
                strtrichyeu = strtrichyeu.Trim();
                bool isfulltext = AppSettings.IsFullText;
                if (isfulltext)
                {   // co fultext search
                    Dictionary<bool, string> result = ValidateData.SearchExactly(strtrichyeu);
                    string searchValues = string.Empty;
                    if (result.ContainsKey(true))
                    {   // co tim kiem chinh xac "abc"                    
                        searchValues = ValidateData.GhepChuoiFullTextSearch(result[true], (int)ValidateData.enumFullTextSearch.AND);
                    }
                    else
                    {
                        //query = " freetext(strtrichyeu,'" + strtrichyeu + "' ) ";
                        searchValues = ValidateData.GhepChuoiFullTextSearch(strtrichyeu, (int)ValidateData.enumFullTextSearch.OR);
                    }
                    query = " contains(strtrichyeu,'" + searchValues + "') ";
                }
                else
                {   // khong fulltext search
                    query = " strtrichyeu like N'%" + strtrichyeu + "%' ";
                }

                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strtrichyeu=" + strtrichyeu + ";";
            }

            if (!string.IsNullOrEmpty(strnguoixuly))
            {
                strnguoixuly = ValidateData.CheckInput(strnguoixuly);
                //vanban = vanban.Where(p => p.strnoinhan.Contains(strnguoixuly));
                query = " strnoinhan like N'%" + strnguoixuly + "%' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strnguoixuly=" + strnguoixuly + ";";
            }

            if (!string.IsNullOrEmpty(strnguoiky))
            {
                strnguoiky = ValidateData.CheckInput(strnguoiky);
                //vanban = vanban.Where(p => p.strnguoiky.Contains(strnguoiky));
                query = " strnguoiky like N'%" + strnguoiky + "%' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strnguoiky=" + strnguoiky + ";";
            }

            if (!string.IsNullOrEmpty(strnoigui))
            {
                strnoigui = ValidateData.CheckInput(strnoigui);
                //vanban = vanban.Where(p => p.strnoiphathanh.Contains(strnoigui));
                query = " strnoiphathanh like N'%" + strnoigui + "%' ";
                if (string.IsNullOrEmpty(strWhere))
                {
                    strWhere += query;
                }
                else
                {
                    strWhere += " and " + query;
                }
                isSearch = true;
                strSearchValues += "strnoigui=" + strnoigui + ";";
            }


            //========================================================
            // end search
            //========================================================


            if (!isSearch)
            {   // khong phai la search thi gioi han ngay hien thi                
                var ngay = _vanbandenRepo.Vanbandens.OrderByDescending(p => p.strngayden).FirstOrDefault();
                if (ngay != null)
                {
                    int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
                    DateTime? dtengaybd = ngay.strngayden.Value.AddDays(-intngay);

                    string strngaybd = DateServices.FormatDateEn(dtengaybd);
                    query = " strngayden >='" + strngaybd + "' ";
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += query;
                    }
                    else
                    {
                        strWhere += " and " + query;
                    }
                }

                // reset session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            }
            else
            {   // luu cac gia tri search vao session
                _session.InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.SearchTracuuVBDen);
                _session.InsertObject(AppConts.SessionSearchTypeValues, strSearchValues);

                // tim kiem thi hien thi tat ca

                // Category thi gioi han ngay hien thi
                //if (isCategory)
                //{
                //    int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);
                //    DateTime? dtengaybd = DateTime.Now.AddDays(-intngay);
                //    string strngaybd = DateServices.FormatDateEn(dtengaybd);
                //    query = " strngayden >='" + strngaybd + "' ";
                //    if (string.IsNullOrEmpty(strWhere))
                //    {
                //        strWhere += query;
                //    }
                //    else
                //    {
                //        strWhere += " and " + query;
                //    }
                //    //vanban = vanban.Where(p => p.strngayden >= dtengaybd);
                //}
            }

            return strWhere;
        }

        /// <summary>
        /// tra ve tat ca cac van ban den duoc quyen xem
        /// </summary>
        /// <param name="isViewVBDenDaXL"></param>
        /// <param name="isViewAllVBDen"></param>
        /// <returns></returns>
        private string _SqlGetAllVBden_v0(bool isViewVBDenDaXL, bool isViewAllVBDen, string strSearchValues)
        {
            string stridcanbo = _session.GetUserId().ToString();

            string strSelect = "Select distinct vanbanden.intid,strKyhieu,strNgayden as dtengayden ,vanbanden.intSoden,strNoiphathanh "
            + ",strTrichyeu,vanbanden.inttrangthai ,intidphanloaivanbanden,strnoinhan,hsvb.intidhosocongviec as intidhoso "
            + ", case when ( intdangvb = 1) then cast(1 as bit) else cast(0 as bit) end as IsVbdt "
            + " ,case when( (select attachvanban.intidvanban ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as IsAttach "
            + ",(select top 1 hscv.inttrangthai ) as inttinhtrangxuly "

            //+ ", case when( (select yk.intid ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as isykien "

            + " From vanbanden "
            + " left outer join attachvanban on vanbanden.intid = attachvanban.intidvanban "
            + " and attachvanban.intloai=" + (int)enumAttachVanban.intloai.Vanbanden
            + " and attachvanban.inttrangthai=" + (int)enumAttachVanban.inttrangthai.IsActive

            + " left outer join hosovanban hsvb on hsvb.intidvanban = vanbanden.intid and hsvb.intloai=" + (int)enumHosovanban.intloai.Vanbanden
            + " left outer join hosocongviec hscv on hscv.intid=hsvb.intidhosocongviec " // and hscv.intloai=" + (int)enumHosocongviec.intloai.Vanbanden

            //+ " inner join hosocongviec hscv on hscv.intid=hsvb.intidhosocongviec " // and hscv.intloai=" + (int)enumHosocongviec.intloai.Vanbanden
                //+ " left outer join doituongxuly dtxl on dtxl.intidhosocongviec = hsvb.intidhosocongviec and dtxl.intidcanbo=" + stridcanbo
                //+ " left outer join hosoykienxuly yk on yk.intiddoituongxuly = dtxl.intid "
            ;

            string query = string.Empty;
            if (isViewAllVBDen)
            {
                // xem tat ca van ban den
                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + " where " + strSearchValues;
                }
                else
                {
                    query = strSelect;
                }
            }
            else
            {   // chi duoc xem nhung van ban den thuoc quyen xu ly/xem

                string strExists =
                    " and  ( "
                        + " Exists (Select intidvanban as intidvanban from vanbandencanbo "
                                    + " where vanbandencanbo.intidvanban=vanbanden.intID and intidcanbo=" + stridcanbo + " ) "
                        + " or Exists "
                                + " (select Hosovanban.intidvanban from Hosovanban "
                                + " inner join Doituongxuly on Doituongxuly.intidhosocongviec=Hosovanban.intidhosocongviec "
                                + " where intloai=" + (int)enumHosovanban.intloai.Vanbanden
                                + " and Doituongxuly.intidcanbo=" + stridcanbo
                                + " and Hosovanban.intidvanban=vanbanden.intID) "
                    //+ " or vanbanden.intpublic =" + (int)enumVanbanden.intpublic.Public // chay rat cham

                    + " )  "
                    + " and vanbanden.intpublic=" + (int)enumVanbanden.intpublic.Private
                    ;
                string vbpublic = " and vanbanden.intpublic=" + (int)enumVanbanden.intpublic.Public;

                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + " where " + strSearchValues + strExists;
                    query = query + " union " + strSelect + " where " + strSearchValues + vbpublic;
                }
                else
                {
                    query = strSelect + " where " + strExists;
                    query = query + " union " + strSelect + " where " + vbpublic;
                }
                //query = strSelect + strExists;
                //query = query + " union " + strSelect + vbpublic;

                if (!isViewVBDenDaXL)
                {
                    query = query + " and hscv.inttrangthai=" + (int)enumHosocongviec.inttrangthai.Dangxuly;
                }
            }

            string strOrder = " order by strngayden desc, intsoden desc ";
            query += strOrder;

            return query;
        }

        /// <summary>
        /// tra ve tat ca cac van ban den duoc quyen xem . da toi uu sql query
        /// </summary>
        /// <param name="isViewVBDenDaXL"></param>
        /// <param name="isViewAllVBDen"></param>
        /// <param name="strSearchValues"></param>
        /// <returns></returns>
        private string _SqlGetAllVBden_v1(bool isViewVBDenDaXL, bool isViewAllVBDen, string strSearchValues)
        {
            string stridcanbo = _session.GetUserId().ToString();

            string strSelect = "Select distinct vanbanden.intid,strKyhieu,strNgayden as dtengayden ,vanbanden.intSoden,strNoiphathanh "
            + ",strTrichyeu,vanbanden.inttrangthai ,intidphanloaivanbanden,strnoinhan,hsvb.intidhosocongviec as intidhoso "
            + ", case when ( intdangvb = 1) then cast(1 as bit) else cast(0 as bit) end as IsVbdt "
            + " ,case when( (select attachvanban.intidvanban ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as IsAttach "
            + ",(select hscv.inttrangthai ) as inttinhtrangxuly "

            //+ ", case when( (select yk.intid ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as isykien "

            + " From vanbanden "
            + " left outer join attachvanban on vanbanden.intid = attachvanban.intidvanban "
            + " and attachvanban.intloai=" + (int)enumAttachVanban.intloai.Vanbanden
            + " and attachvanban.inttrangthai=" + (int)enumAttachVanban.inttrangthai.IsActive

            + " left outer join hosovanban hsvb on hsvb.intidvanban = vanbanden.intid and hsvb.intloai=" + (int)enumHosovanban.intloai.Vanbanden
            + " left outer join hosocongviec hscv on hscv.intid=hsvb.intidhosocongviec " // and hscv.intloai=" + (int)enumHosocongviec.intloai.Vanbanden
            ;

            string query = string.Empty;
            if (isViewAllVBDen)
            {
                // xem tat ca van ban den
                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + " where " + strSearchValues;
                }
                else
                {
                    query = strSelect;
                }
            }
            else
            {   // chi duoc xem nhung van ban den thuoc quyen xu ly/xem

                string strExists =
                    " and  (( vanbanden.intpublic=" + (int)enumVanbanden.intpublic.Private
                            + " and ( Exists (Select intidvanban as intidvanban from vanbandencanbo "
                                        + " where vanbandencanbo.intidvanban=vanbanden.intID and intidcanbo=" + stridcanbo + " ) "
                            + " or Exists "
                                    + " (select Hosovanban.intidvanban from Hosovanban "
                                    + " inner join Doituongxuly on Doituongxuly.intidhosocongviec=Hosovanban.intidhosocongviec "
                                    + " and intloai=" + (int)enumHosovanban.intloai.Vanbanden
                                    + " and Doituongxuly.intidcanbo=" + stridcanbo
                                    + " and Hosovanban.intidvanban=hsvb.intidvanban) "
                                + " ) "
                            + " ) "
                        + " or vanbanden.intpublic=" + (int)enumVanbanden.intpublic.Public
                    + " ) "
                    ;

                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + " where " + strSearchValues + strExists;
                }
                else
                {
                    query = strSelect + " where " + strExists;
                }

                if (!isViewVBDenDaXL)
                {
                    query = query + " and hscv.inttrangthai=" + (int)enumHosocongviec.inttrangthai.Dangxuly;
                }
            }

            string strOrder = " order by strngayden desc, intsoden desc ";
            query += strOrder;

            return query;
        }

        private string _SqlGetAllVBden_v2(bool isViewVBDenDaXL, bool isViewAllVBDen, string strSearchValues)
        {
            string stridcanbo = _session.GetUserId().ToString();

            string strSelect = "Select distinct vanbanden.intid,strKyhieu,strNgayden as dtengayden ,vanbanden.intSoden,strNoiphathanh "
            + ",strTrichyeu,vanbanden.inttrangthai ,intidphanloaivanbanden,strnoinhan"
            + ",hsvb.intidhosocongviec as intidhoso "
            + ", case when ( intdangvb = 1) then cast(1 as bit) else cast(0 as bit) end as IsVbdt "
            + " ,case when( (select attachvanban.intidvanban ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as IsAttach "
            + ",(select hscv.inttrangthai ) as inttinhtrangxuly ";

            //+ ", case when( (select yk.intid ) is not null)then cast(1 as bit) ELSE cast(0 as bit) END as isykien "

            string strFrom = " From vanbanden "
            + " left outer join attachvanban on vanbanden.intid = attachvanban.intidvanban "
                            + " and attachvanban.intloai=" + (int)enumAttachVanban.intloai.Vanbanden
                            + " and attachvanban.inttrangthai=" + (int)enumAttachVanban.inttrangthai.IsActive

            + " left outer join hosovanban hsvb on hsvb.intidvanban = vanbanden.intid and hsvb.intloai=" + (int)enumHosovanban.intloai.Vanbanden
            + " left outer join hosocongviec hscv on hscv.intid=hsvb.intidhosocongviec " // and hscv.intloai=" + (int)enumHosocongviec.intloai.Vanbanden
            ;

            string query = string.Empty;
            if (isViewAllVBDen)
            {
                // xem tat ca van ban den
                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + strFrom + " where " + strSearchValues;
                }
                else
                {
                    query = strSelect + strFrom;
                }
            }
            else
            {   // chi duoc xem nhung van ban den thuoc quyen xu ly/xem

                string strFromExists = " left outer join vanbandencanbo vbcb on vbcb.intidvanban=vanbanden.intid and intidcanbo =" + stridcanbo
                    + " left outer join doituongxuly dtxl on dtxl.intidhosocongviec=hsvb.intidhosocongviec and dtxl.intidcanbo=" + stridcanbo
                    ;
                string strExists =
                    " and ( "
                            + "( vanbanden.intpublic =" + (int)enumVanbanden.intpublic.Private
                                        + " and (vbcb.intidvanban is not null or dtxl.intid is not null)) "
                            + " or vanbanden.intpublic =" + (int)enumVanbanden.intpublic.Public
                    + "	) "
                    ;

                if (!string.IsNullOrEmpty(strSearchValues))
                {
                    query += strSelect + strFrom + strFromExists + " where " + strSearchValues + strExists;
                }
                else
                {
                    query = strSelect + strFrom + strFromExists + " where " + strExists;
                }

                if (!isViewVBDenDaXL)
                {
                    query = query + " and hscv.inttrangthai=" + (int)enumHosocongviec.inttrangthai.Dangxuly;
                }
            }

            string strOrder = " order by strngayden desc, intsoden desc ";
            query += strOrder;

            return query;
        }

        #endregion RawSql


        //========================================


        public SearchVBViewModel GetViewSearch()
        {
            SearchVBViewModel model = new SearchVBViewModel();
            model.Khoiphathanh = _khoiphRepo.GetActiveKhoiphathanhs.OrderBy(p => p.strtenkhoi);
            model.Loaivanban = _plvanbanRepo.GetActivePhanloaiVanbans
                .Where(p => p.intloai == (int)enumPhanloaiVanban.intloai.vanbanden)
                .OrderBy(p => p.strtenvanban);
            model.Sovanban = _sovbRepo.GetActiveSoVanbans
                .Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanden)
                .OrderBy(p => p.strten);
            model.Nguoixuly = _canboRepo.GetActiveCanbo
                .Select(p => new CanboViewModel
                {
                    strhoten = p.strhoten
                });

            return model;
        }



        #endregion Listvanban


        #region ViewDetail

        public DetailVBDenViewModel GetViewDetail(int id)
        {
            int idcanbo = _session.GetUserId();
            //bool isView = _role.IsViewVanbanden(id, idcanbo);
            //if (isView == false)
            //{
            //    _logger.Warn("không có quyền xem văn bản: " + id.ToString());
            //    return new DetailVBDenViewModel();
            //}


            var cv = _vanbandenRepo.Vanbandens.FirstOrDefault(p => p.intid == id);

            var hsvb = _hosovanbanRepo.Hosovanbans
                        .Where(p => p.intloai == (int)enumHosovanban.intloai.Vanbanden)
                        .Where(p => p.intidvanban == id).ToList();
            int idhosocongviec = 0;
            string strtieude = "";
            if (hsvb.Count() != 0)
            {
                idhosocongviec = hsvb.FirstOrDefault().intidhosocongviec;
                strtieude = _hosocongviecRepo.Hosocongviecs
                            .FirstOrDefault(p => p.intid == idhosocongviec)
                            .strtieude;
            }

            string strvbmat = string.Empty;
            string strvbkhan = string.Empty;
            try
            {
                if ((cv.intidmat != null) && (cv.intidmat != 0))
                {
                    strvbmat = _tinhchatvbRepo.GetAllTinhchatvanbans.Where(p => p.intloai == (int)enumTinhchatvanban.intloai.Mat)
                            .FirstOrDefault(p => p.intid == cv.intidmat).strtentinhchatvb;
                }

                if ((cv.intidkhan != null) && (cv.intidkhan != 0))
                {
                    strvbkhan = _tinhchatvbRepo.GetAllTinhchatvanbans.Where(p => p.intloai == (int)enumTinhchatvanban.intloai.Khan)
                                    .FirstOrDefault(p => p.intid == cv.intidkhan).strtentinhchatvb;
                }
            }
            catch { }

            string strvanbandi = string.Empty;
            int idvanbandi = 0;
            string strvanbanphathanh = string.Empty;
            int idvanbanphathanh = 0;

            var hoibao = _hoibaovanbanRepo.Hoibaovanbans
                            .Where(p => p.intRecID == cv.intid)
                //.Where(p => p.intloai == (int)enumHoibaovanban.intloai.Vanbanden)
                //.FirstOrDefault();
                            .ToList();
            if (hoibao.Count > 0)
            {
                foreach (var hb in hoibao)
                {
                    if (hb.intloai == (int)enumHoibaovanban.intloai.Vanbanden)
                    {
                        idvanbandi = hb.intTransID;
                        var vbdi = _vanbandiRepo.Vanbandis.Where(p => p.intid == idvanbandi).FirstOrDefault();
                        strvanbandi = vbdi.intso.ToString() + "/" + vbdi.strkyhieu;
                    }
                    if (hb.intloai == (int)enumHoibaovanban.intloai.Vanbandi)
                    {
                        idvanbanphathanh = hb.intTransID;
                        var vbdi = _vanbandiRepo.Vanbandis.Where(p => p.intid == idvanbanphathanh).FirstOrDefault();
                        strvanbanphathanh = vbdi.intso.ToString() + "/" + vbdi.strkyhieu;
                    }
                }
            }

            IEnumerable<DownloadFileViewModel> downloadFiles = _fileRepo.AttachVanbans
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden)
                        .Where(p => p.intidvanban == id)
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Select(p => new DownloadFileViewModel
                        {
                            intid = p.intid,
                            strtenfile = p.strmota
                            //intloai = (int)p.intloai
                        }).ToList();
            foreach (var f in downloadFiles)
            {
                f.fileExt = _fileManager.GetFileExtention(f.strtenfile);
                f.strfiletypeimages = _fileManager.GetFileTypeImages(f.strtenfile);
                //f.fileIcon = _fileManager.getfi
                f.intloai = (int)enumDownloadFileViewModel.intloai.Vanbanden;
            }
            //========================================
            var vanban = new DetailVBDenViewModel();

            //vanban = cv,
            vanban.intid = cv.intid;
            vanban.strngayden = DateServices.FormatDateVN(cv.strngayden);
            vanban.intsoden = (int)cv.intsoden;
            vanban.strkyhieu = cv.strkyhieu;

            try
            {
                vanban.strsovanban = _sovbRepo.GetAllSoVanbans.Where(p => p.intloai == (int)enumSovanban.intloai.Vanbanden)
                        .FirstOrDefault(p => p.intid == cv.intidsovanban).strten;
            }
            catch { }

            try
            {
                vanban.strloaivanban = _plvanbanRepo.GetAllPhanloaiVanbans.Where(p => p.intloai == (int)enumPhanloaiVanban.intloai.vanbanden)
                        .FirstOrDefault(p => p.intid == cv.intidphanloaivanbanden).strtenvanban;
            }
            catch { }

            try
            {
                vanban.strtenkhoiphathanh = _khoiphRepo.GetAllKhoiphathanhs.FirstOrDefault(p => p.intid == cv.intidkhoiphathanh).strtenkhoi;
            }
            catch { }


            vanban.strtencoquanphathanh = cv.strnoiphathanh;
            vanban.strtrichyeu = cv.strtrichyeu;
            vanban.strngayky = DateServices.FormatDateVN(cv.strngayky);
            vanban.strnguoiky = cv.strnguoiky;

            vanban.strvbmat = strvbmat;
            vanban.strvbkhan = strvbkhan;

            try
            {
                vanban.strnguoixulybandau = _canboRepo.GetAllCanbo.FirstOrDefault(p => p.intid == cv.intidnguoiduyet).strhoten;
            }
            catch { }

            vanban.strnguoixulychinh = cv.strnoinhan;
            vanban.strhantraloi = DateServices.FormatDateVN(cv.strhanxuly);
            vanban.strtraloivanban = cv.strtraloivanbanso;

            // vanban hoibao cua van ban den
            vanban.idvanbandi = idvanbandi;
            vanban.strvanbandi = strvanbandi;
            // vanban phat hanh cua vanban den
            vanban.idvanbanphathanh = idvanbanphathanh;
            vanban.strvanbanphathanh = strvanbanphathanh;

            vanban.intidhosocongviec = idhosocongviec;
            vanban.strhosovanban = strtieude;

            vanban.isattach = _fileRepo.AttachVanbans
                        .Where(p => p.inttrangthai == (int)enumAttachVanban.inttrangthai.IsActive)
                        .Where(p => p.intloai == (int)enumAttachVanban.intloai.Vanbanden)
                        .Where(p => p.intidvanban == id)
                        .Any();

            vanban.DownloadFiles = downloadFiles;

            return vanban;
        }


        #endregion ViewDetail




    }
}
