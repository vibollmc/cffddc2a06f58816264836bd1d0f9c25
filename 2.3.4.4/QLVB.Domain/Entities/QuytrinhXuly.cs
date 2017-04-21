using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    public class QuytrinhXuly
    {
        /// <summary>
        /// table quytrinhxuly va quytrinhNode giong nhau voi intidNode???
        /// </summary>
        public int intid { get; set; }
        public int intidNode { get; set; }
        public int intidDonvi { get; set; }
        public int intidCanbo { get; set; }
        public int intVaitro { get; set; }
        public int intSoNgay { get; set; }
        public int intNext { get; set; }
        public int intHoanthanh { get; set; }

        // cho biet tai node nay co yeu cau xu ly dong thoi khong
        public int? intXulyDongthoi { get; set; }

    }

    public class enumQuytrinhXuly
    {
        public enum intVaitro
        {
            Khongthamgia = 0,
            Lanhdaogiaoviec = 1,
            Lanhdaophutrach = 2,
            Xulychinh = 3,
            Phoihopxuly = 4
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
