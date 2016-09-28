using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLVB.DTO.Vanbandientu.EdXML
{
    public class edXMLMessageHeader
    {
        public edXMLDonvi From { get; set; }
        [XmlElement(Namespace = "http://www.e-doc.vn/Schema/")]
        public List<edXMLDonvi> To { get; set; }
        public edXMLCode Code { get; set; }
        public edXMLPromulgationInfo PromulgationInfo { get; set; }
        public edXMLDocumentType DocumentType { get; set; }
        public string Subject { get; set; }
        public edXMLSignerInfo SignerInfo { get; set; }
        public edXMLOtherInfo OtherInfo { get; set; }
        public string Content { get; set; }
    }
}
