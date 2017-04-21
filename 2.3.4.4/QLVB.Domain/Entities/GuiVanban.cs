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
    }
}