using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class PhanloaiVanban
    {
        public Int32 intid { get; set; }

        public Int32 intloai { get; set; }

        public string strmavanban { get; set; }

        public string strkyhieu { get; set; }

        public string strtenvanban { get; set; }

        public string strghichu { get; set; }

        // --- mac dinh chon trong combox loai van ban khi them moi van ban
        public bool IsDefault { get; set; }

        //  ---- Is active-----------------
        public Int32 inttrangthai { get; set; }
    }

    public class enumPhanloaiVanban
    {
        public enum intloai
        {
            // giong nhau cho 3 table
            //  phanloaivanban
            //  motatruong
            // phanloaitruong
            [Display(Name = "Văn bản đến")]
            vanbanden = 1,
            [Display(Name = "Văn bản đi")]
            vanbandi = 2,
            [Display(Name = "Văn bản dự thảo")]
            vanbanduthao = 3
        }
        public enum inttrangthai
        {
            NotActive = 0,
            IsActive = 1
        }
    }
}