using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    // HIEN KHONG SU DUNG
    // TABLE SO VANBAN CO THEM TRUONG KHOI PHAT HANH
    public class SovbKhoiph
    {
        public int intid { get; set; }

        public int intidsovb { get; set; }

        public int? intidkhoiph { get; set; }

        public int? intidloaivb { get; set; }

        public int? intloai { get; set; }
    }

    public class enumSovbKhoiph
    {
        public enum intloai
        {
            Vanbanden = 1,
            Vanbanphathanh = 2
        }
    }
}