using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class QuytrinhVersion
    {
        public int intid { get; set; }
        public int intidquytrinh { get; set; }
        public DateTime strNgayApdung { get; set; }

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


        public int? intidCanboCapnhat { get; set; }
        public DateTime? strNgayCapnhat { get; set; }




    }
    public class enumQuytrinhVersion
    {
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
