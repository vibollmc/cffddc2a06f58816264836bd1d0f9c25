using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hoso
{
    public class PhanXLVBViewModel
    {
        public bool IsSave { get; set; }

        public Hosocongviec HosocongviecModel { get; set; }
        public int? intidlanhdaogiaoviec { get; set; }

        // hien khong su dung stridlanhdaophutrach
        // chuyen sang dung list<int> intidlanhdaophutrach
        public string stridlanhdaophutrach { get; set; }
        public List<int> listidlanhdaophutrach { get; set; }

        public int? intDonvi { get; set; }
        public IEnumerable<QLVB.DTO.Donvi.EditDonviViewModel> listDonvi { get; set; }

        public IEnumerable<QLVB.DTO.Donvi.ListUserViewModel> listCanbo { get; set; }
        public int? intidxulychinh { get; set; }


        public Hosovanban HosovanbanModel { get; set; }

        public int intidvanban { get; set; }
        //public Vanbanden VanbandenModel { get; set; }

        public IEnumerable<Doituongxuly> DoituongxulyModel { get; set; }

        public IEnumerable<CanboViewModel> LanhdaogiaoviecModel { get; set; }

        public IEnumerable<CanboViewModel> LanhdaophutrachModel { get; set; }

        public IEnumerable<CanboViewModel> XulychinhModel { get; set; }

        public IEnumerable<Hosoykienxuly> HosoykienxulyModel { get; set; }

        public IEnumerable<QLVB.Domain.Entities.Linhvuc> LinhvucModel { get; set; }

        // xac dinh co phai da duoc phan quy trinh khong?
        public bool IsQuytrinh { get; set; }

        public bool IsDonghoso { get; set; }


    }


}
