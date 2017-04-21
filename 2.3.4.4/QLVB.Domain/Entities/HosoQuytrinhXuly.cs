using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QLVB.Domain.Entities
{
    /// <summary>
    /// luu cac thong tin quy trinh xu ly cua cac ho so cong viec
    /// </summary>
    public class HosoQuytrinhXuly
    {
        public int intid { get; set; }
        public int intidhoso { get; set; }

        public int intidquytrinh { get; set; }
        public DateTime? strNgayapdung { get; set; }

        public int intidFrom { get; set; }
        public string nodeidFrom { get; set; }
        public string strTenNodeFrom { get; set; }
        public int? intLeft { get; set; }
        public int? intTop { get; set; }


        public int? intidTo { get; set; }
        public string nodeidTo { get; set; }
        public string strlabel { get; set; }

        public int? intidCanbo { get; set; }
        public int? intVaitro { get; set; }
        public int? intSongay { get; set; }
        public int? intNext { get; set; }
        public int? intHoanthanh { get; set; }
        // cho biet tai node nay co yeu cau xu ly dong thoi khong
        public int? intXulyDongthoi { get; set; }

        // trang thai xu ly cua tung node
        public int? inttrangthai { get; set; }

        public DateTime? strNgaybd { get; set; }
        public DateTime? strNgaykt { get; set; }

    }
    public class enumHosoQuytrinhXuly
    {
        // giong nhu doi tuong xu ly
        //public enum intVaitro
        //{

        //}
        public enum inttrangthai
        {
            ChuaXuly = 0,
            DangXuly = 1,
            DaXuly = 2
        }
        public enum intNext
        {
            Khong = 0,
            Co = 1
        }
        public enum intHoanthanh
        {
            Khong = 0,
            Co = 1
        }
        public enum intXulyDongthoi
        {
            Khong = 0,
            Co = 1
        }
    }
}
