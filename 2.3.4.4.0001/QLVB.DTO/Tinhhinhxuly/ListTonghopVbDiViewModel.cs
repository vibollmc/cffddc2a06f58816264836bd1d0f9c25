using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Tinhhinhxuly
{
    public class ListTonghopVbDiViewModel
    {
        public bool IsBack { get; set; }
        public LoaiNgay LoaiNgay { get; set; }
        public LoaiXuLyVbDi LoaiXuly { get; set; }
        public int Page { get; set; }
        public string Donvi { get; set; }
        public string Ngaybd { get; set; }
        public string Ngaykt { get; set; }
        public string LoaiVanBan { get; set; }
    }
}
