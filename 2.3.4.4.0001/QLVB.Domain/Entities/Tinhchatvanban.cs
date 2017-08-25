using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class Tinhchatvanban
    {
        public Int32 intid { get; set; }

        public Int32 intloai { get; set; }
        public string strtentinhchatvb { get; set; }

        public string strkyhieu { get; set; }

        public string strmota { get; set; }

        public int inttrangthai { get; set; }
    }

    public class enumTinhchatvanban
    {
        public enum intloai
        {
            Khan = 1,
            Mat = 2
        }
        public enum inttrangthai
        {
            NotActive = 0,
            IsActive = 1
        }
    }
}