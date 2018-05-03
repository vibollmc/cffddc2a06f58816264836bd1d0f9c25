using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Tinhhinhxuly
{
    public class XLVanbandi
    {
        public int? IdDonvi { get; set; }
        public string Donvi { get; set; }
        public int Dagui { get; set; }
        public int Tiepnhan { get; set; }
        public int DangXuly { get; set; }
        public int Hoanthanh { get; set; }
        public int Quahan { get; set; }
        public int Hoanthanhtrehan { get; set; }
    }

    public enum LoaiNgay
    {
        NgayKy = 1,
        NgayGui = 2
    }

    public enum LoaiXuLyVbDi
    {
        Dagui = 1,
        Datiepnhan = 2,
        Dangxuly = 3,
        Quahan = 4,
        Hoanthanh = 5,
        Hoanthanhtrehan = 6
    }
}
