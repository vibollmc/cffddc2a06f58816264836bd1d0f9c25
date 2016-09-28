using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.DTO.Quytrinh
{
    public class ListLoaiQuytrinhViewModel
    {
        public IEnumerable<EditLoaiQuytrinhViewModel> LoaiQuytrinh { get; set; }
        // them tung quy trinh cu the

        public IEnumerable<EditQuytrinhViewModel> Quytrinh { get; set; }
    }

    public class CacLoaiQuytrinhViewModel
    {
        public IEnumerable<EditLoaiQuytrinhViewModel> LoaiQuytrinh { get; set; }
        // them tung quy trinh cu the

        //public IEnumerable<QuytrinhViewModel> Quytrinh { get; set; }

    }
}
