using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.File
{
    public class UploadFileViewModel
    {
        public int IdFile { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }

        // dung de luu idfile
        public string Extension { get; set; }

        //public UploadFileViewModel(string name, string extension)
        //{
        //    this.Name = name;
        //    //this.Size = size;
        //    this.Extension = extension;
        //}
    }
}
