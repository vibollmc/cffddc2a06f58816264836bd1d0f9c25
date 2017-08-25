using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLVB.DTO.Vanbandientu.EdXML
{
    [Serializable]
    [XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class edXMLMessage
    {
        [XmlElement(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public edXMLHeader Header { get; set; }

        [XmlElement(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public edXMLBody Body { get; set; }

        [XmlElement(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public List<edXMLAttachment> Attachments { get; set; }
    }
}
