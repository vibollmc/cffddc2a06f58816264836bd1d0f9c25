using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbanden
{
    public class FormPhanXLNhieuVBViewModel
    {
        public bool IsSave { get; set; }

        public DateTime? dteNgaymohoso { get; set; }

        public DateTime? dteHanxuly { get; set; }

        public IEnumerable<CanboViewModel> LanhdaogiaoviecModel { get; set; }

        public IEnumerable<CanboViewModel> LanhdaophutrachModel { get; set; }

        public IEnumerable<CanboViewModel> XulychinhModel { get; set; }

        public IEnumerable<QLVB.DTO.Donvi.EditDonviViewModel> listDonvi { get; set; }

        public int? intidxulychinh { get; set; }


    }
}
