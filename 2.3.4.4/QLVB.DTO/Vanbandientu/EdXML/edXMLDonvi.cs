using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandientu.EdXML
{
    public class edXMLDonvi
    {
        // bat buoc 
        public string OrganId { get; set; }
        public string OrganName { get; set; }
        public string OrganAdd { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        // tuy chon
        public string OrganizationInCharge { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }

    }
}
