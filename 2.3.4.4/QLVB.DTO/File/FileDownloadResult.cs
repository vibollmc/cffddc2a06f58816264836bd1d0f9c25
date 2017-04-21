using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.File
{
    public class FileDownloadResult
    {

        public string fileName { get; set; }
        public string strmota { get; set; }

        public string filePath { get; set; }

        public string physicalFilePath { get; set; }

        // public byte[] fileData { get; set; }


        public FileDownloadResult(string fileName, //byte[] fileData, 
                string strmota, string filePath, string physicalFilePath)
        {
            this.fileName = fileName;
            //this.fileData = fileData;
            this.strmota = strmota;
            this.filePath = filePath;
            this.physicalFilePath = physicalFilePath;
        }
    }


}
