using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLVB.DTO.Vanbandientu.EdXML
{
    public class edXMLDocument
    {
        //[XmlElement(Namespace = "http://www.e-doc.vn/Schema/")]
        public List<edXMLAttachment> Attach { get; set; }
    }
}
