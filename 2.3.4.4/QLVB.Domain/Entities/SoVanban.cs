using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class SoVanban
    {
        public int intid { get; set; }

        public string strten { get; set; }

        public string strkyhieu { get; set; }

        public string strghichu { get; set; }

        // trong van ban den
        public int? intidkhoiph { get; set; }

        // dung trong van ban di
        // tuong ung voi so van ban di
        public int? intidloaivb { get; set; }

        public int? intloai { get; set; }
        public int? intorder { get; set; }

        // --- mac dinh chon trong combox so van ban khi them moi van ban
        public bool IsDefault { get; set; }

        public int? inttrangthai { get; set; }

        //===========bo sung so voi qlvb1 ===========
        // dung de cau hinh co cho phep xem toan bo so van ban den/di khong?
        public int? intquyenxemsovb { get; set; }
    }
    public class enumSovanban
    {
        public enum inttrangthai
        {
            IsActive = 1,
            NotActive = 0
        }

        public enum intloai
        {
            [Display(Name = "Sổ văn bản đến")]
            Vanbanden = 1,
            [Display(Name = "Sổ văn bản đi")]
            Vanbanphathanh = 2
        }
        public enum intquyenxemsovb
        {
            Choxem = 0,
            Khongduocxem = 1
        }
    }
}