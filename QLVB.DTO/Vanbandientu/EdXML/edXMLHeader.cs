using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLVB.DTO.Vanbandientu.EdXML
{
    public class edXMLHeader
    {
        [XmlElement(Namespace = "http://www.e-doc.vn/Schema/")]
        public edXMLMessageHeader MessageHeader { get; set; }


        [XmlArray("TraceHeaderList", Namespace = "http://www.e-doc.vn/Schema/")]
        [XmlArrayItem("TraceHeader", Namespace = "http://www.e-doc.vn/Schema/")]
        public List<edXMLTraceHeader> TraceHeaderList { get; set; }


        [XmlArray("ErrorList", Namespace = "http://www.e-doc.vn/Schema/")]
        [XmlArrayItem("Error", Namespace = "http://www.e-doc.vn/Schema/")]
        public List<edXMLError> ErrorList { get; set; }



    }
}
