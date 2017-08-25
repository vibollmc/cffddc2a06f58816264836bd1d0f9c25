using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Entities
{
    public class QuytrinhNode
    {
        public int intid { get; set; }
        public int intidquytrinh { get; set; }
        public string NodeId { get; set; }
        // loai node: begin, end, task
        public int intloai { get; set; }
        public string strten { get; set; }
        public int intleft { get; set; }
        public int inttop { get; set; }
    }
    public enum enumNode
    {
        Begin = 1,
        End = 2,
        Task = 3,
    }
}
