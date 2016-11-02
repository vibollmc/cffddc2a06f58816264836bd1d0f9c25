using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLTD.Web.Main.Models.QLVB
{
    public class Donvitructhuoc
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public int intlevel { get; set; }

        public string strmadonvi { get; set; }
        public string strtendonvi { get; set; }

        public Nullable<int> intorder { get; set; }

        public Nullable<int> inttrangthai { get; set; }

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
