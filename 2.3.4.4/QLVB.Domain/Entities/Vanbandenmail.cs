using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class Vanbandenmail
    {
        public int intid { get; set; }

        public DateTime? strngayky { get; set; }

        public string strkyhieu { get; set; }

        // ban cu: intidphanloaicongvanden
        public int? intidphanloaivanbanden { get; set; }

        public int? intkhan { get; set; }

        public int? intmat { get; set; }

        public int? intso { get; set; }

        public string strtrichyeu { get; set; }

        public string strnguoiky { get; set; }

        public string strnoigui { get; set; }

        public int? intsoto { get; set; }

        public int? intsoban { get; set; }

        public string strloaivanban { get; set; }

        public int? intguitra { get; set; }

        public int? inttrangthai { get; set; }

        public int? intattach { get; set; }

        // d/c mail  trong noidung message
        public string strAddressSend { get; set; }
        // d/c mail thuc te trong mail server
        public string strFromAddress { get; set; }

        public int? intloai { get; set; }

        public string strnoiguivb { get; set; }

        public DateTime? strngayguivb { get; set; }

        public DateTime? strngaynhanvb { get; set; }

        public string strmadinhdanh { get; set; }

        public DateTime? strhantraloi { get; set; }
    }

    public class enumVanbandenmail
    {
        public enum intloai
        {
            Vanbanden = 1,
            Vanbandi = 2
        }
        public enum intattach
        {
            Khong = 0,
            Co = 1
        }
        public enum inttrangthai
        {
            Chuacapnhat = 0,
            Dacapnhat = 1
        }
    }
}