using System;
using System.Runtime.Serialization;

namespace QLVB.WebUI.Models
{
    /// <summary>
    /// dung trong web api auto upload
    /// </summary>    
    [DataContract]
    public class FileDesc
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string path { get; set; }

        [DataMember]
        public long size { get; set; }

        public FileDesc(int _id, string n, string p, long s)
        {
            id = _id;
            name = n;
            path = p;
            size = s;
        }
    }
}