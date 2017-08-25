using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class Hosovanban
    {

        public int intid { get; set; }

        public int intidhosocongviec { get; set; }

        public int intloai { get; set; }

        public int intidvanban { get; set; }

        public int? inttrangthai { get; set; }
    }

    public class enumHosovanban
    {
        public enum intloai
        {
            Vanbanden = 10,
            Vanbanden_quytrinh = 11,
            Vanbandi = 20,
            Vanbandi_quytrinh = 21
        }

        public enum inttrangthai
        {
            Dangxuly = 0,
            Dahoanthanh = 1
        }
    }
}