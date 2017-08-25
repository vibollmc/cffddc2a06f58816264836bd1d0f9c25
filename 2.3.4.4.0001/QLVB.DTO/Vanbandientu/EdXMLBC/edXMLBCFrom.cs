using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandientu.EdXMLBC
{
    public class edXMLBaocao
    {
        // don vi gui
        public string FromOrganId { get; set; }
        public string FromEmail { get; set; }
        public string FromTimestamp { get; set; }

        // vanban gui
        public string CodeNumber { get; set; }
        public string CodeNotation { get; set; }
        public string PromulgationDate { get; set; }

        // don vi nhan
        public string ToOrganId { get; set; }
        public string ToEmail { get; set; }
        public string ToTimestamp { get; set; }
    }
}
