using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class Phieutrinh
    {
        public int intid { get; set; }
        public int? intidhosocongviec { get; set; }
        public int? intidcanbotrinh { get; set; }
        public string strnoidungtrinh { get; set; }
        public DateTime? strngaytrinh { get; set; }
        public int? inttrangthaitrinh { get; set; }
        public int? intidlanhdao { get; set; }
        public string strykienchidao { get; set; }
        public DateTime? strngaychidao { get; set; }
        public int? inttrangthaichidao { get; set; }
    }

    public class enumphieutrinh
    {
        public enum inttrangthaitrinh
        {
            Dangtrinh = 0,
            Datrinh = 1
        }
        public enum inttrangthaichidao
        {
            Chuachidao = 0,
            Dachidao = 1
        }
    }
}
