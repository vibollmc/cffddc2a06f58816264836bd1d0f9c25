using System;

namespace DLTD.Web.Main.Models.QLVB
{
    
    public class Tochucdoitac
    {
        public int intid { get; set; }

        public int intidkhoi { get; set; }

        public string strmatochucdoitac { get; set; }

        public string strtentochucdoitac { get; set; }

        public string strdiachi { get; set; }

        public string strphone { get; set; }

        public string strfax { get; set; }

        public string stremail { get; set; }

        public string stremailvbdt { get; set; }

        //  truong cua qlvb 1 là bittrangthai
        public int Isvbdt { get; set; }

        // trang thai khi xoa
        public int inttrangthai { get; set; }

        public int? inthoibao { get; set; }

        // ma dinh danh theo quy dinh cua bo TTTT
        public string strmadinhdanh { get; set; }
    }

    public class enumTochucdoitac
    {
        public enum isvbdt
        {
            IsActive = 1,  // yes/no
            NotActive = 0
        }
        public enum inttrangthai
        {
            IsActive = 1,
            NotActive = 0
        }
        public enum inthoibao
        {
            IsActive = 1,
            NotActive = 0
        }

    }
}