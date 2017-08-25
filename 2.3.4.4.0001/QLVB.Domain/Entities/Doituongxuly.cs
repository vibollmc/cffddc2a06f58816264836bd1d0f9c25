using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class Doituongxuly
    {
        public int intid { get; set; }

        // ho so cong viec
        public int? intidhosocongviec { get; set; }

        // can bo xu ly
        public int? intidcanbo { get; set; }

        // vai tro xu ly
        public int? intvaitro { get; set; }

        // thao tac cua nguoi su dung
        // moi them vao so voi qlvb1
        public string strthaotac { get; set; }

        // can bo them nguoi xu ly
        public int? intnguoitao { get; set; }

        // thoi gian them nguoi xu ly
        public DateTime? strngaytao { get; set; }

        // vai tro cu truoc khi bi chuyen xu ly
        public int? intvaitrocu { get; set; }

        public int? intnguoichuyen { get; set; }

        public DateTime? strngaychuyen { get; set; }
    }

    public class enumDoituongxuly
    {
        public enum intvaitro_doituongxuly
        {
            Lanhdaogiaoviec = 1,
            Lanhdaophutrach = 2,
            Xulychinh = 3,
            Phoihopxuly = 4,
            Chuyenxuly = 5
            //CapnhatVBGiay = 6,  // co the khong su dung
            //CapnhatVBDientu = 7, // co the khong su dung 
            //KetthucHoso = 8,
            //MolaiHoso = 9
        }
        // union voi table doituongxuly
        public enum intvaitro_chitiethoso
        {
            CapnhatVBGiay = 1,  // co the khong su dung
            CapnhatVBDientu = 2, // co the khong su dung 
            KetthucHoso = 3,
            MolaiHoso = 4,
            Phathanhvanban_dongHS = 5
        }

        public enum intvaitrocu_doituongxuly
        {
            Lanhdaogiaoviec = 1,
            Lanhdaophutrach = 2,
            Xulychinh = 3,
            Phoihopxuly = 4,
        }

    }
}
