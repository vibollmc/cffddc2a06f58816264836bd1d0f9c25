using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    /// <summary>
    /// giong voi table hosovanban
    /// </summary>
    public class Hosovanbanlienquan
    {

        public int intid { get; set; }

        public int intidhosocongviec { get; set; }

        public int intloai { get; set; }

        public int intidvanban { get; set; }

        public int? inttrangthai { get; set; }
    }

    public class enumHosovanbanlienquan
    {
        public enum intloai
        {
            Vanbanden = 10,
            Vanbandi = 20
        }

        public enum inttrangthai
        {
            Dangxuly = 0,
            Dahoanthanh = 1
        }
    }
}