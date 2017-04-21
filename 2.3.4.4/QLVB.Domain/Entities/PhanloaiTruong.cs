using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class PhanloaiTruong
    {
        public Int32 intid { get; set; }

        // --- loai van ban
        public Int32 intloai { get; set; }

        public Int32 intidmotatruong { get; set; }

        public Int32 intidphanloaivanban { get; set; }

        public bool IsDisplay { get; set; }

        public Int32? intorder { get; set; }

        public bool IsRequire { get; set; }

        public string strmota { get; set; }

        public Int32? intloaitruong { get; set; }
    }

    public class enumPhanloaiTruong
    {
        public enum intloai
        {
            // giong nhau cho 3 table
            //  phanloaivanban
            //  motatruong
            // phanloaitruong
            vanbanden = 1,
            vanbandi = 2,
            vanbanduthao = 3
        }
        public enum intloaitruong
        {
            Default = 1,        // default: là các trường mặc định
            AddNew = 2          // addnew: là các trường do người dùng mới thêm vào
        }

    }
}