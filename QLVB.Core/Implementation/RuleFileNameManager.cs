using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Common.Logging;
using QLVB.Domain.Entities;

namespace QLVB.Core.Implementation
{
    public class RuleFileNameManager : IRuleFileNameManager
    {
        #region Constructor

        private ILogger _logger;
        private ISoVanbanRepository _sovbRepo;
        private IVanbandenRepository _vbdenRepo;
        private IVanbandiRepository _vbdiRepo;
        private IHoibaovanbanRepository _hoibaovanbanRepo;
        public RuleFileNameManager(ISoVanbanRepository sovbRepo,
            IVanbandenRepository vbdenRepo, IVanbandiRepository vbdiRepo,
            ILogger logger, IHoibaovanbanRepository hoibaovbRepo)
        {
            _sovbRepo = sovbRepo;
            _vbdenRepo = vbdenRepo;
            _vbdiRepo = vbdiRepo;
            _logger = logger;
            _hoibaovanbanRepo = hoibaovbRepo;
        }

        #endregion Constructor

        /// <summary>
        /// lay tên sổ văn bản
        /// </summary>
        /// <param name="strquytac"></param>
        /// <param name="strtenvanban"></param>
        /// <returns>vd: CV</returns>
        public string GetMaSo(string strquytac, string strtenvanban)
        {
            // 1-T-YYYY
            // 0 - 2 - 4
            int posTenso = strquytac.IndexOf("T");
            int posSovb = strquytac.IndexOf("1");
            int posYear = strquytac.IndexOf("YYYY");
            // kiem tra vi tri nao =0 

            string kq = string.Empty;

            // Số văn bản trước : 1-T-YYYY
            if (posSovb == 0)
            {
                int first_ = strtenvanban.IndexOf("-");
                string strSovb = strtenvanban.Substring(0, first_);

                strtenvanban = strtenvanban.Substring(first_ + 1);
                int second_ = strtenvanban.IndexOf("-");
                string strtenso = strtenvanban.Substring(0, second_);

                strtenvanban = strtenvanban.Substring(second_ + 1);

                string stryear = strtenvanban.Substring(0, 4);

                kq = strtenso;
            }

            // tên sổ trước: T-1-YYYY
            if (posTenso == 0)
            {
                int first_ = strtenvanban.IndexOf("-");
                string strtenso = strtenvanban.Substring(0, first_);

                strtenvanban = strtenvanban.Substring(first_ + 1);
                int second_ = strtenvanban.IndexOf("-");

                string strSovb = strtenvanban.Substring(0, second_);
                strtenvanban = strtenvanban.Substring(second_ + 1);

                string stryear = strtenvanban.Substring(0, 4);

                kq = strtenso;
            }
            return kq;
        }

        /// <summary>
        /// lấy số văn bản
        /// </summary>
        /// <param name="strquytac"></param>
        /// <param name="strtenvanban"></param>
        /// <returns>vd: 234</returns>
        public int GetSo(string strquytac, string strtenvanban)
        {
            // 1-T-YYYY
            // 0 - 2 - 4
            int posTenso = strquytac.IndexOf("T");
            int posSovb = strquytac.IndexOf("1");
            int posYear = strquytac.IndexOf("YYYY");
            // kiem tra vi tri nao =0 

            int kq = 0;

            // Số văn bản trước : 1-T-YYYY
            if (posSovb == 0)
            {
                int first_ = strtenvanban.IndexOf("-");
                string strSovb = strtenvanban.Substring(0, first_);

                strtenvanban = strtenvanban.Substring(first_ + 1);
                int second_ = strtenvanban.IndexOf("-");
                string strtenso = strtenvanban.Substring(0, second_);

                strtenvanban = strtenvanban.Substring(second_ + 1);

                string stryear = strtenvanban.Substring(0, 4);

                kq = Convert.ToInt32(strSovb);
            }

            // tên sổ trước: T-1-YYYY
            if (posTenso == 0)
            {
                int first_ = strtenvanban.IndexOf("-");
                string strtenso = strtenvanban.Substring(0, first_);

                strtenvanban = strtenvanban.Substring(first_ + 1);
                int second_ = strtenvanban.IndexOf("-");

                string strSovb = strtenvanban.Substring(0, second_);
                strtenvanban = strtenvanban.Substring(second_ + 1);

                string stryear = strtenvanban.Substring(0, 4);

                kq = Convert.ToInt32(strSovb);
            }
            return kq;
        }

        /// <summary>
        /// Lấy năm văn bản
        /// </summary>
        /// <param name="strquytac"></param>
        /// <param name="strtenvanban"></param>
        /// <returns>vd: 2013</returns>
        public int GetYear(string strquytac, string strtenvanban)
        {
            // 1-T-YYYY
            // 0 - 2 - 4
            int posTenso = strquytac.IndexOf("T");
            int posSovb = strquytac.IndexOf("1");
            int posYear = strquytac.IndexOf("YYYY");
            // kiem tra vi tri nao =0 

            int kq = 0;

            // Số văn bản trước : 1-T-YYYY
            if (posSovb == 0)
            {
                int first_ = strtenvanban.IndexOf("-");
                string strSovb = strtenvanban.Substring(0, first_);

                strtenvanban = strtenvanban.Substring(first_ + 1);
                int second_ = strtenvanban.IndexOf("-");
                string strtenso = strtenvanban.Substring(0, second_);

                strtenvanban = strtenvanban.Substring(second_ + 1);

                string stryear = strtenvanban.Substring(0, 4);

                kq = Convert.ToInt32(stryear);
            }

            // tên sổ trước: T-1-YYYY
            if (posTenso == 0)
            {
                int first_ = strtenvanban.IndexOf("-");
                string strtenso = strtenvanban.Substring(0, first_);

                strtenvanban = strtenvanban.Substring(first_ + 1);
                int second_ = strtenvanban.IndexOf("-");

                string strSovb = strtenvanban.Substring(0, second_);
                strtenvanban = strtenvanban.Substring(second_ + 1);

                string stryear = strtenvanban.Substring(0, 4);

                kq = Convert.ToInt32(stryear);
            }
            return kq;
        }

        /// <summary>
        /// lay idvanban 
        /// </summary>
        /// <param name="strquytac"></param>
        /// <param name="strtenvanban"></param>
        /// <returns>
        /// 0: loi khong tim thay id vanban
        /// !=0: id van ban
        /// </returns>
        public int GetIdVanban(string strquytac, string strtenvanban)
        {
            // 1-T-YYYY
            // 0 - 2 - 4
            int posTenso = strquytac.IndexOf("T");
            int posSovb = strquytac.IndexOf("1");
            int posYear = strquytac.IndexOf("YYYY");

            // kiem tra vi tri nao =0 
            int idvanban = 0;
            string strtenso = string.Empty;  // mã sổ văn bản
            int intso = 0;      // số văn bản
            int intyear = 0;

            // Số văn bản trước : 1-T-YYYY
            if (posSovb == 0)
            {
                int first_ = strtenvanban.IndexOf("-");
                string strSovb = strtenvanban.Substring(0, first_);
                intso = Convert.ToInt32(strSovb);

                strtenvanban = strtenvanban.Substring(first_ + 1);
                int second_ = strtenvanban.IndexOf("-");

                strtenso = strtenvanban.Substring(0, second_);
                strtenvanban = strtenvanban.Substring(second_ + 1);

                string stryear = strtenvanban.Substring(0, 4);
                intyear = Convert.ToInt32(stryear);
            }

            // tên sổ trước: T-1-YYYY
            if (posTenso == 0)
            {
                int first_ = strtenvanban.IndexOf("-");
                strtenso = strtenvanban.Substring(0, first_);

                strtenvanban = strtenvanban.Substring(first_ + 1);
                int second_ = strtenvanban.IndexOf("-");
                string strSovb = strtenvanban.Substring(0, second_);
                intso = Convert.ToInt32(strSovb);

                strtenvanban = strtenvanban.Substring(second_ + 1);
                string stryear = strtenvanban.Substring(0, 4);
                intyear = Convert.ToInt32(stryear);
            }


            int? intloai = _sovbRepo.GetActiveSoVanbans.Where(p => p.strkyhieu == strtenso).FirstOrDefault().intloai;

            if (intloai == (int)enumSovanban.intloai.Vanbanden)
            {
                idvanban = _vbdenRepo.Vanbandens.Where(p => p.intsoden == intso)
                                .Where(p => p.strngayden.Value.Year == intyear) // ==========
                                .Join(
                                    _sovbRepo.GetActiveSoVanbans.Where(p => p.strkyhieu == strtenso),
                                    v => v.intidsovanban,
                                    s => s.intid,
                                    (v, s) => v
                                )
                                .FirstOrDefault().intid;

            }
            if (intloai == (int)enumSovanban.intloai.Vanbanphathanh)
            {
                idvanban = _vbdiRepo.Vanbandis.Where(p => p.intso == intso)
                                .Where(p => p.strngayky.Year == intyear)  // ==============
                                .Join(
                                    _sovbRepo.GetActiveSoVanbans.Where(p => p.strkyhieu == strtenso),
                                    v => v.intidsovanban,
                                    s => s.intid,
                                    (v, s) => v
                                )
                                .FirstOrDefault().intid;
            }

            return idvanban;
        }

        /// <summary>
        /// lấy tên sổ vb và số đến/đi của văn bản theo quy tắc đã có
        /// </summary>
        /// <param name="strquytac"></param>
        /// <param name="strtenvanban"></param>
        /// <returns> trả về
        /// loaivb: loại sổ văn bản (đến / đi)
        /// idvanban: id văn bản
        /// </returns>
        public Dictionary<string, string> GetFromFileName(string strquytac, string strtenvanban)
        {
            // 1-T-YYYY
            // 0 - 2 - 4
            int posTenso = strquytac.IndexOf("T");
            int posSovb = strquytac.IndexOf("1");
            int posYear = strquytac.IndexOf("YYYY");
            // kiem tra vi tri nao =0 

            Dictionary<string, string> result = new Dictionary<string, string>();
            // tên sổ văn bản   : tensovb
            // số văn bản       : sovb
            // năm              : namvb 
            // loại sổ          : loaisovb (vbden=1, vbdi = 2)
            // idvanban         : idvanban

            int idvanban = 0;
            string strtenso = string.Empty;  // mã sổ văn bản
            int intso = 0;      // số văn bản
            int intyear = 0;

            try
            {
                // Số văn bản trước : 1-T-YYYY
                if (posSovb == 0)
                {
                    int first_ = strtenvanban.IndexOf("-");
                    string strSovb = strtenvanban.Substring(0, first_);
                    intso = Convert.ToInt32(strSovb);
                    result.Add("sovb", strSovb);

                    strtenvanban = strtenvanban.Substring(first_ + 1);
                    int second_ = strtenvanban.IndexOf("-");
                    strtenso = strtenvanban.Substring(0, second_);
                    result.Add("tensovb", strtenso);

                    strtenvanban = strtenvanban.Substring(second_ + 1);
                    string stryear = strtenvanban.Substring(0, 4);
                    intyear = Convert.ToInt32(stryear);
                    result.Add("namvb", stryear);

                }

                // tên sổ trước: T-1-YYYY
                if (posTenso == 0)
                {
                    int first_ = strtenvanban.IndexOf("-");
                    strtenso = strtenvanban.Substring(0, first_);
                    result.Add("tensovb", strtenso);

                    strtenvanban = strtenvanban.Substring(first_ + 1);
                    int second_ = strtenvanban.IndexOf("-");
                    string strSovb = strtenvanban.Substring(0, second_);
                    intso = Convert.ToInt32(strSovb);
                    result.Add("sovb", strSovb);

                    strtenvanban = strtenvanban.Substring(second_ + 1);
                    string stryear = strtenvanban.Substring(0, 4);
                    intyear = Convert.ToInt32(stryear);
                    result.Add("namvb", stryear);

                }

                int intloai = 0; // nếu không tìm thấy ký hiệu sổ văn bản

                var sovb = _sovbRepo.GetActiveSoVanbans.Where(p => p.strkyhieu == strtenso).FirstOrDefault();
                if (sovb != null)
                {
                    intloai = (int)sovb.intloai;
                }

                result.Add("loaivb", intloai.ToString());

                if (intloai == (int)enumSovanban.intloai.Vanbanden)
                {
                    idvanban = _vbdenRepo.Vanbandens.Where(p => p.intsoden == intso)
                                    .Where(p => p.strngayden.Value.Year == intyear) // ==========
                                    .Join(
                                        _sovbRepo.GetActiveSoVanbans.Where(p => p.strkyhieu == strtenso),
                                        v => v.intidsovanban,
                                        s => s.intid,
                                        (v, s) => v
                                    )
                                    .FirstOrDefault().intid;

                }
                if (intloai == (int)enumSovanban.intloai.Vanbanphathanh)
                {
                    idvanban = _vbdiRepo.Vanbandis.Where(p => p.intso == intso)
                                    .Where(p => p.strngayky.Year == intyear)  // ==============
                                    .Join(
                                        _sovbRepo.GetActiveSoVanbans.Where(p => p.strkyhieu == strtenso),
                                        v => v.intidsovanban,
                                        s => s.intid,
                                        (v, s) => v
                                    )
                                    .FirstOrDefault().intid;
                }

                result.Add("idvanban", idvanban.ToString());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                result.Add("idvanban", "0");
                result.Add("loaivb", "0");
            }

            return result;
        }

        /// <summary>
        /// lien ket van ban den/di
        /// </summary>
        /// <param name="intLoai"></param>
        /// <param name="idvanbanden"></param>
        /// <param name="idvanbandi"></param>
        /// <returns></returns>
        public int LienketVanban(int intLoai, int idvanbanden, int idvanbandi)
        {
            // một vb đi chỉ trả lời cho 1 vb đến
            // nhưng nhiều vb đến có thể trả lời(hồi báo) cho 1 vb đi
            int result = 0;
            try
            {
                if ((idvanbanden > 0) && (idvanbandi > 0)
                    && (intLoai == (int)enumHoibaovanban.intloai.Vanbandi))
                {
                    // vb đi
                    var hoibao = _hoibaovanbanRepo.Hoibaovanbans
                        //.Where(p => p.intloai == (int)enumHoibaovanban.intloai.Vanbandi)
                                .Where(p => p.intloai == intLoai)
                                .Where(p => p.intTransID == idvanbandi).ToList();
                    if (hoibao.Count() > 0)
                    {
                        // xoa va them moi
                        foreach (var p in hoibao)
                        {
                            _hoibaovanbanRepo.Xoa(intLoai, idvanbandi, idvanbanden);
                        }
                        // them moi
                        _hoibaovanbanRepo.Them(intLoai, idvanbandi, idvanbanden);
                    }
                    else
                    {
                        // them moi
                        _hoibaovanbanRepo.Them(intLoai, idvanbandi, idvanbanden);
                    }
                    result = 1;
                }
                //=====================================================
                if ((idvanbanden > 0) && (idvanbandi > 0)
                    && (intLoai == (int)enumHoibaovanban.intloai.Vanbanden))
                {
                    // vb đến
                    _hoibaovanbanRepo.Them(intLoai, idvanbandi, idvanbanden);
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return result;

        }

        /// <summary>
        /// tra ve cac danh sach quy tac dat ten file
        /// </summary>
        /// <returns></returns>
        public string GetQuytacTenFile(string strquytac)
        {
            string result = string.Empty;
            // 1-T-YYYY
            // 0 - 2 - 4
            int posTenso = strquytac.IndexOf("T");
            int posSovb = strquytac.IndexOf("1");
            int posYear = strquytac.IndexOf("YYYY");

            // Số văn bản trước : 1-T-YYYY
            if (posSovb == 0)
            {
                result = "Số VB - Mã sổ - Năm";
            }

            // tên sổ trước: T-1-YYYY
            if (posTenso == 0)
            {
                result = "Mã sổ - Số VB - Năm";
            }
            return result;
        }
    }
}
