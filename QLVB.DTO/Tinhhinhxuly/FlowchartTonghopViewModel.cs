using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Tinhhinhxuly
{
    public class FlowchartTonghopViewModel
    {
        public IEnumerable<NodeViewModel> nodes { get; set; }
        public IEnumerable<ConnectionViewModel> connections { get; set; }
        public int numberOfElements { get; set; }
        public string strNgayApdung { get; set; }

        public IEnumerable<NodeXulyTonghopViewModel> xulys { get; set; }
    }
    public class NodeViewModel
    {
        public string Id { get; set; }
        public string text { get; set; }
        public int left { get; set; }
        public int top { get; set; }

    }
    public class ConnectionViewModel
    {
        public string from { get; set; }
        public string to { get; set; }
        public string label { get; set; }

    }

    public class NodeXulyTonghopViewModel
    {
        public string Id { get; set; }
        // canbo xu ly tai node
        public string strhotencanbo { get; set; }
        // trang thai xu ly tai node
        public int? inttrangthai { get; set; }
        public string strNgaybd { get; set; }
        public string strNgaykt { get; set; }

        public string strVaitro { get; set; }
        public string stNgayApdung { get; set; }
        public int? intSongay { get; set; }

        public int? intXulyDongthoi { get; set; }

        public int? intSoHoso { get; set; }


    }
}
