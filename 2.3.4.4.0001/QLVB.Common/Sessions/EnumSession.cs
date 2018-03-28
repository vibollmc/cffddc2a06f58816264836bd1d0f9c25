using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Common.Sessions
{
    public class EnumSession
    {
        public enum SearchType
        {
            NoSearch = 0,
            SearchVBDen = 1,
            SearchVBDi = 2,
            SearchVBDenDientu = 3,
            SearchVBDiDientu = 4,
            ListTinhhinhVBDen = 5,
            SearchVBDenLQ = 6,
            SearchVBDiLQ = 7,
            SearchHSCV = 8,
            SearchTracuuVBDen = 9,
            SearchTracuuVBDi = 10
        }
        public enum PageType
        {
            NoPage = 0,
            VBDen = 1,
            VBDi = 2,
            VBDenDientu = 3,
            VBDiDientu = 4,
            TinhhinhVBDen = 5,
            VBDenLQ = 6,
            VBDiLQ = 7,
            HSCV = 8,
            TinhhinhQuytrinh = 9,
            TracuuVBDen = 10,
            TracuuVBDi = 11
        }

        public enum TonghopXuly
        {
            Vanbanden = 1,
            Quytrinh = 2,
            Ykcd = 3,
            Vanbandi =4
        }



    }
}
