using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hoso
{
    public class PhieutrinhViewModel
    {
        public int idphieutrinh { get; set; }
        public int idhoso { get; set; }

        public int idcanbotrinh { get; set; }
        public string strcanbotrinh { get; set; }
        public string strnoidungtrinh { get; set; }
        public string strngaytrinh { get; set; }
        public DateTime? dtengaytrinh { get; set; }

        public IEnumerable<DanhsachNguoixulyViewModel> lanhdao { get; set; }
        public bool IsChoykienchidao { get; set; }

        public int idlanhdaotrinh { get; set; }
        public string strlanhdaotrinh { get; set; }
        public string strykienchidao { get; set; }
        public string strngaychidao { get; set; }
        public DateTime? dtengaychidao { get; set; }

    }


}
