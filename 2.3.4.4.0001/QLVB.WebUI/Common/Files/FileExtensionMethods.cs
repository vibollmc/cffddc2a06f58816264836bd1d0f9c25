using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace QLVB.WebUI.Common.Files
{
    public static class FileExtensionMethods
    {
        public static byte[] GetFileData(this string fileName, string filePath)
        {
            var fullFilePath = string.Format("{0}/{1}", filePath, fileName);
            if (!File.Exists(fullFilePath))
                throw new FileNotFoundException("The file does not exist.", fullFilePath);
            return File.ReadAllBytes(fullFilePath);
        }
    }
}