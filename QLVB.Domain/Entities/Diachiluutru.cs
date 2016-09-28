using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class Diachiluutru
    {
        public Int32 Id { get; set; }

        public Int32? ParentId { get; set; }

        public Int32 intlevel { get; set; }

        public string strtendonvi { get; set; }

        public Int32? inttrangthai { get; set; }

        [ForeignKey("ParentId")]
        public ICollection<Diachiluutru> donviChild { get; set; }
    }

    public class enumDiachiluutru
    {
        public enum inttrangthai
        {
            IsActive = 1,
            NotActive = 0
        }
    }
}
