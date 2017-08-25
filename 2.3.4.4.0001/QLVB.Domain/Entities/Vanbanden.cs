using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class Vanbanden
    {
        public int intid { get; set; }

        public int? intsoden { get; set; }

        public string strkyhieu { get; set; }

        public DateTime? strngayden { get; set; }

        public DateTime? strngayky { get; set; }

        public int? intidkhoiphathanh { get; set; }

        public string strnoiphathanh { get; set; }

        public string strnoigui { get; set; }

        public string strtrichyeu { get; set; }

        [Required(ErrorMessage = "*")]

        public int? intidnguoiduyet { get; set; }

        public string strnoinhan { get; set; }

        public string strtomtatnoidung { get; set; }

        public string strnguoiky { get; set; }

        public string strtukhoa { get; set; }

        public int? intidkhan { get; set; }

        public int? intidmat { get; set; }

        public int? intiddiachiluutru { get; set; }

        public int? intidphanloaivanbanden { get; set; }

        // trạng thái văn bản
        // thay đổi so với qlvb1
        public int? inttrangthai { get; set; }
        public int? intiddonvinhap { get; set; }

        // trường này có dư không?????
        public string strnoidung { get; set; }

        public int? intidnguoitao { get; set; }

        public DateTime? strngaytao { get; set; }

        public int? intidnguoisua { get; set; }

        public DateTime? strngaysua { get; set; }

        public int? intidlinhvuc { get; set; }

        //-????????????????
        public int? intmucquantrong { get; set; }

        public int intidsovanban { get; set; }
        public int? intquyphamphapluat { get; set; }

        public int? intidldvp { get; set; }

        public int? bitguivbdt { get; set; }

        public DateTime? strhanxuly { get; set; }

        public string strtraloivanbanso { get; set; }

        // ==== them 3 truong
        public int? intsoban { get; set; }

        public int? intsoto { get; set; }

        // truong nhan biet van ban dien tu (intvbdt trong qlvb vpub)
        // nen sua lai la: dang van ban: vb giay, vb dt, hoac ca hai
        public int? intdangvb { get; set; }

        public int? intpublic { get; set; }

        public  int? intidvanbandenmail { get; set; }
    }

    public class enumVanbanden
    {
        public enum inttrangthai
        {
            Chuaduyet = 0,
            Daduyet = 1
        }
        public enum intpublic
        {
            Private = 0,
            Public = 1
        }
        public enum intquyphamphapluat
        {
            Co = 1,
            Khong = 0
        }

        public enum bitguivbdt
        {
            Khong = 0,
            Dagui = 1
        }

        public enum intvbdt
        {
            VBGiay = 0,     // văn bản giấy
            VBDT = 1,       // văn bản điện tử
            VBDT_Giay = 2   // cả 2 loại văn bản giấy và điện tử
        }
    }
}