using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLVB.DTO.Truclienthongtinh
{
    [XmlRoot("organization")]
    public class OrganizationVM
    {
        public string code { get; set; }
        public string name { get; set; }
        public string secureKey { get; set; }
        public string description { get; set; }
        public string signature { get; set; }
        public string systemType { get; set; }
        public string orgExternal { get; set; }
        public string active { get; set; }
        public string edxmlCode { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string isDepartment { get; set; }
        public bool DuocChon { get; set; }
        public int ChuanLienThong { get; set; }
    }
}
