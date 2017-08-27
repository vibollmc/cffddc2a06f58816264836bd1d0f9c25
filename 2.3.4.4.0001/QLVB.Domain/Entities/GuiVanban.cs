using System;

namespace QLVB.Domain.Entities
{
    public class GuiVanban
    {
        public int intid { get; set; }

        public int? intidvanban { get; set; }

        public int? intloaivanban { get; set; }

        public int? intiddonvi { get; set; }

        public int? inttrangthaigui { get; set; }

        public DateTime? strngaygui { get; set; }

        public int? inttrangthainhan { get; set; }

        public DateTime? strngaynhan { get; set; }
        public DateTime? strngaytiepnhan { get; set; }
        public DateTime? strngayhoanthanh { get; set; }
        public DateTime? strngaydangxuly { get; set; }

        public int? intloaigui { get; set; }
        public string strtendonvi { get; set; }
    }

    public class enumGuiVanban
    {
        public enum inttrangthaigui
        {
            Chuagui = 0,
            Dagui = 1,
            Chuagui_normal = 2,  // gui binh thuong, khong theo tieu chuan TTTH, BTTTT
        }
        public enum intloaivanban
        {
            Vanbanden = 1,
            Vanbandi = 2
        }
        public enum inttrangthainhan
        {
            Chuanhan = 0,
            Danhan = 1
        }
        public enum inttrangthaiphanhoi
        { 
            DaDen=01,
            Guinham=02,
            Datiepnhan = 03,
            Phancong =04,
            Dangxuly = 05,
            Hoanthanh=06,
            XoaVanBan=13
            
        }
        public enum intloaigui
        {
            Email = 1, 
            Tructinh = 2,
            Chinhphu = 3
        }
    }
}