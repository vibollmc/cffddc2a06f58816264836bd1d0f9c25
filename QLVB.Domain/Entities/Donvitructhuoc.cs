using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class Donvitructhuoc
    {
        public Int32 Id { get; set; }

        public Int32? ParentId { get; set; }

        public Int32 intlevel { get; set; }

        public string strmadonvi { get; set; }
        public string strtendonvi { get; set; }

        public Nullable<Int32> intorder { get; set; }

        public Nullable<Int32> inttrangthai { get; set; }

        //public List<Donvitructhuoc> Donvis { get; set; }
        //public virtual Donvitructhuoc donviParent { get; set; }

        [ForeignKey("ParentId")]
        public ICollection<Donvitructhuoc> donviChild { get; set; }
    }

    public class enumDonvitructhuoc
    {
        public enum inttrangthai
        {
            IsActive = 1,
            NotActive = 0
        }
    }
}
