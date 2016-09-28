using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class Vanbandi
    {
        public int intid { get; set; }
        public int? intso { get; set; }

        public string strkyhieu { get; set; }
        public DateTime strngayky { get; set; }

        public string strdonvisoan { get; set; }

        public string strtrichyeu { get; set; }

        public string strnoinhan { get; set; }

        public string strnguoiky { get; set; }

        public string strnguoisoan { get; set; }

        public string strnguoiduyet { get; set; }

        public int? intidnguoiduyet { get; set; }

        public string strtomtat { get; set; }

        public int? intidkhan { get; set; }

        public int? intidmat { get; set; }

        public int? intsoban { get; set; }

        public int? intsoto { get; set; }

        public string strtukhoa { get; set; }

        public int? intiddiachiluutru { get; set; }

        public int? intidphanloaivanbandi { get; set; }

        // thay doi so voi qlvb1
        public int? inttrangthai { get; set; }

        public int? intiddonvinhap { get; set; }

        public string strnoidung { get; set; }

        public int? intidnguoitao { get; set; }

        public DateTime? strngaytao { get; set; }

        public int? intidnguoisua { get; set; }

        public DateTime? strngaysua { get; set; }

        public int? intidlinhvuc { get; set; }

        public int? intmucquantrong { get; set; }

        public int? intguivbdt { get; set; }

        public int? intidsovanban { get; set; }

        public int? intquyphamphapluat { get; set; }

        public string strmorong { get; set; }

        public DateTime? strhanxuly { get; set; }

        public int? intsosao { get; set; }

        public DateTime? strngaysao { get; set; }

        public DateTime? strngayhoanthanh { get; set; }

        public string strtraloivanbanso { get; set; }

        public int? intsobanFile { get; set; }

        public int? intsotoFile { get; set; }

        //dang van ban: vb giay, vb dt, hoac ca hai
        public int? intdangvb { get; set; }

        public int? intpublic { get; set; }

    }

    public class enumVanbandi
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
        public enum intguivbdt
        {
            Chuagui = 0,
            Dagui = 1
        }
        public enum intquyphamphapluat
        {
            Co = 1,
            Khong = 0
        }
    }
}