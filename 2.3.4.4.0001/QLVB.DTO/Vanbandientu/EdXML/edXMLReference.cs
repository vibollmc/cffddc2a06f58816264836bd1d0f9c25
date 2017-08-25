using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLVB.DTO.Vanbandientu.EdXML
{
    public class edXMLReference
    {
        [XmlAttribute(Namespace = "http://www.w3.org/1999/xlink")]
        public string href { get; set; }
        public string AttachmentName { get; set; }
        public string Description { get; set; }
    }

    public class Reference
    {
        [XmlAttribute(Namespace = "http://www.w3.org/1999/xlink")]
        public string href { get; set; }

        public string AttachmentName { get; set; }
        public string Description { get; set; }
    }
}
