using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Common.Edxml
{
    /// <summary>
    ///  override edxml2 de thay doi default filepath 
    /// </summary>
    public static class EdXml2Override
    {
        public static System.IO.MemoryStream Compressed(INet.EdXml2.Attachment.FileAttach fa, string filepath)
        {
            System.IO.MemoryStream result;
            try
            {
                char[] listOldChar = new char[]
				{
					'=',
					'/',
					'\\',
					'?',
					'"',
					'+',
					':',
					'>',
					'<',
					'|'
				};
                char newChar = 'z';
                string arg = new INet.EdXml2.Util.EdXmlBoundary().Replace(listOldChar, newChar);
                string text = string.Format("{0}.{1}", arg, fa.FileExtention);
                string path = string.Format("{0}.{1}", arg, "zip");

                // thay doi duong dan mac dinh 
                string _text2 = filepath + "\\" + text;
                string _path2 = filepath + "\\" + path;

                using (FileStream fileStream = new FileStream(_text2, FileMode.Create, FileAccess.Write))
                {
                    byte[] array = new byte[fa.FileSize];
                    int count;
                    while ((count = fa.FileStream.Read(array, 0, array.Length)) > 0)
                    {
                        fileStream.Write(array, 0, count);
                    }
                    fileStream.Close();
                }
                using (ZipOutputStream zipOutputStream =
                    new ZipOutputStream(File.Create(_path2), Convert.ToInt32(fa.FileSize)))
                {
                    byte[] array = new byte[fa.FileSize];
                    ZipEntry zipEntry = new ZipEntry(ZipEntry.CleanName(text));
                    zipEntry.DateTime = DateTime.Now;
                    zipOutputStream.UseZip64 = UseZip64.Off;
                    zipOutputStream.PutNextEntry(zipEntry);
                    using (FileStream fileStream = new FileStream(_text2, FileMode.Open, FileAccess.Read))
                    {
                        int count2;
                        while ((count2 = fileStream.Read(array, 0, array.Length)) > 0)
                        {
                            zipOutputStream.Write(array, 0, count2);
                        }
                        fileStream.Close();
                    }
                    zipOutputStream.Finish();
                    zipOutputStream.Close();
                }
                MemoryStream memoryStream = null;
                using (FileStream fileStream = new FileStream(_path2, FileMode.Open, FileAccess.Read))
                {
                    byte[] array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
                    memoryStream = new MemoryStream(array);
                    fileStream.Close();
                }
                File.Delete(_path2);
                File.Delete(_text2);
                result = memoryStream;

            }
            catch (Exception ex)
            {
                result = null;
                throw ex;
            }
            return result;
        }

        // INet.EdXml2.Util.EdXmlUtil
        public static MemoryStream Decompressed(INet.EdXml2.Attachment.FileAttach fa, string filepath)
        {
            char[] listOldChar = new char[]
				{
					'=',
					'/',
					'\\',
					'?',
					'"',
					'+',
					':',
					'>',
					'<',
					'|'
				};
            char newChar = 'z';

            string arg = new INet.EdXml2.Util.EdXmlBoundary().Replace(listOldChar, newChar);
            string text = string.Format("{0}.{1}", arg, fa.FileExtention);
            string path = string.Format("{0}.{1}", arg, "zip");

            // thay doi duong dan mac dinh 
            string _text2 = filepath + "\\" + text;
            string _path2 = filepath + "\\" + path;

            MemoryStream result;
            try
            {
                //using (FileStream fileStream = new FileStream(_path2, FileMode.Create, FileAccess.Write))
                //{
                //    byte[] array = new byte[fa.FileSize];
                //    int count;
                //    while ((count = fa.FileStream.Read(array, 0, array.Length)) > 0)
                //    {
                //        fileStream.Write(array, 0, count);
                //    }
                //    fileStream.Close();
                //}
                using (FileStream fileStream = new FileStream(_text2, FileMode.Create, FileAccess.Write))
                {
                    byte[] array = new byte[fa.FileSize];
                    int count;
                    while ((count = fa.FileStream.Read(array, 0, array.Length)) > 0)
                    {
                        fileStream.Write(array, 0, count);
                    }
                    fileStream.Close();
                }
            }
            catch
            {
            }

            //using (ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(_path2), Convert.ToInt32(fa.FileSize)))
            //{
            //    while (zipInputStream.GetNextEntry() != null)
            //    {
            //        if (text != string.Empty)
            //        {
            //            using (FileStream fileStream2 = File.Create(_text2))
            //            {
            //                int num = Convert.ToInt32(fa.FileSize);
            //                byte[] array2 = new byte[fa.FileSize];
            //                while (true)
            //                {
            //                    num = zipInputStream.Read(array2, 0, array2.Length);
            //                    if (num <= 0)
            //                    {
            //                        break;
            //                    }
            //                    fileStream2.Write(array2, 0, num);
            //                }
            //            }
            //        }
            //    }
            //    zipInputStream.Close();
            //}
            //return result;

            try
            {
                MemoryStream memoryStream = null;
                using (FileStream fileStream = new FileStream(_text2, FileMode.Open, FileAccess.Read))
                {
                    byte[] array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
                    memoryStream = new MemoryStream(array);
                    fileStream.Close();
                }
                //File.Delete(_path2);
                File.Delete(_text2);
                result = memoryStream;
            }
            catch
            {
                result = null;
            }
            return result;



        }


    }

}

