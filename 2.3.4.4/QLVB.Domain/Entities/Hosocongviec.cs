using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{

    public class Hosocongviec
    {

        public int intid { get; set; }

        public int? intsotudong { get; set; }

        public string strsohieuht { get; set; }

        public int? intloai { get; set; }

        public int? intmucdo { get; set; }

        public int? intlinhvuc { get; set; }

        public int? intiddonvi { get; set; }

        public string strtieude { get; set; }

        public DateTime? strngaymohoso { get; set; }

        public DateTime? strthoihanxuly { get; set; }

        public string strnoidung { get; set; }

        public int? inttrangthai { get; set; }

        public DateTime? strngayketthuc { get; set; }

        public string strketqua { get; set; }

        public int? intidnguoinhap { get; set; }

        public int? intidnguoihoanthanh { get; set; }

        public int? intsoden { get; set; }

        public int? intkhan { get; set; }

        public int? intmat { get; set; }

        public int? intluuhoso { get; set; }

        public int? intdonghoso { get; set; }
    }

    public class enumHosocongviec
    {
        public enum intloai
        {
            Vanbanden = 1,
            Vanbanden_Quytrinh = 11,
            Vanbandi = 2,
            Vanbanduthao = 3,
            Giaiquyetcongviec = 4
        }

        public enum inttrangthai
        {
            Dangxuly = 0,
            Dahoanthanh = 1
        }

        public enum intluuhoso
        {
            Khong = 0,
            //luu ho so
            Co = 1,

            DangTrinhky = 2,

            // doi voi hosoquytrinh
            TamngungXL = 3
        }
        public enum intdonghoso
        {
            Khong = 0,
            Co = 1
        }
    }
}