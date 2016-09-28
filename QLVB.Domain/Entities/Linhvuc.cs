using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    
    public class Linhvuc
    {
    
        public Int32 intid { get; set; }

        public string strtenlinhvuc { get; set; }

        public string strkyhieu { get; set; }

        public string strghichu { get; set; }

        //  moi so voi qlvb 1
        //  thoi han xu ly tinh trong linh vuc iso , hoac mot cua
        public DateTime? strthoihanxuly { get; set; }

        // phan loai iso, mot cua
        public int? intloai { get; set; }

        public int? inttrangthai { get; set; }
    }

    public class enumLinhvuc
    {
        public enum intloai
        {
            Vanbanthuong = 1,
            Iso = 2,
            Motcua = 3
        }
        public enum inttrangthai
        {
            NotActive = 0,
            IsActive = 1
        }
    }
}