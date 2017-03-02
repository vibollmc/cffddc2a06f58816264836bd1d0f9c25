using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;

namespace DLTD.Web.Main.Common
{
    public static class WordReader
    {
        public static string DocxConvertToHtml(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var byteArray = File.ReadAllBytes(filePath);
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(byteArray, 0, byteArray.Length);
                using (var wDoc = WordprocessingDocument.Open(memoryStream, true))
                {
                    var destFileName = new FileInfo(fileName.Replace(".docx", ".html"));
                    var imageDirectoryName = destFileName.FullName.Substring(0, destFileName.FullName.Length - 5) + "_files";
                    var imageCounter = 0;
                    var pageTitle = (string)wDoc.CoreFilePropertiesPart.GetXDocument().Descendants(DC.title).FirstOrDefault() ?? fileName;

                    var settings = new HtmlConverterSettings()
                    {
                        PageTitle = pageTitle,
                        FabricateCssClasses = true,
                        CssClassPrefix = "pt-",
                        RestrictToSupportedLanguages = false,
                        RestrictToSupportedNumberingFormats = false,
                        ListItemImplementations = new Dictionary<string, Func<string, int, string, string>>()
                        {
                            {"fr-FR", ListItemTextGetter_fr_FR.GetListItemText},
                            {"tr-TR", ListItemTextGetter_tr_TR.GetListItemText},
                            {"ru-RU", ListItemTextGetter_ru_RU.GetListItemText},
                            {"sv-SE", ListItemTextGetter_sv_SE.GetListItemText},
                        },
                        ImageHandler = imageInfo =>
                        {
                            var localDirInfo = new DirectoryInfo(imageDirectoryName);
                            if (!localDirInfo.Exists)
                                localDirInfo.Create();
                            ++imageCounter;
                            var extension = imageInfo.ContentType.Split('/')[1].ToLower();
                            ImageFormat imageFormat = null;
                            switch (extension)
                            {
                                case "png":
                                    // Convert png to jpeg.
                                    extension = "gif";
                                    imageFormat = ImageFormat.Gif;
                                    break;
                                case "gif":
                                    imageFormat = ImageFormat.Gif;
                                    break;
                                case "bmp":
                                    imageFormat = ImageFormat.Bmp;
                                    break;
                                case "jpeg":
                                    imageFormat = ImageFormat.Jpeg;
                                    break;
                                case "tiff":
                                    // Convert tiff to gif.
                                    extension = "gif";
                                    imageFormat = ImageFormat.Gif;
                                    break;
                                case "x-wmf":
                                    extension = "wmf";
                                    imageFormat = ImageFormat.Wmf;
                                    break;
                            }

                            // If the image format isn't one that we expect, ignore it,
                            // and don't return markup for the link.
                            if (imageFormat == null)
                                return null;

                            var imageFileName = imageDirectoryName + "/image" +
                                imageCounter.ToString() + "." + extension;
                            try
                            {
                                imageInfo.Bitmap.Save(imageFileName, imageFormat);
                            }
                            catch (System.Runtime.InteropServices.ExternalException)
                            {
                                return null;
                            }
                            var img = new XElement(Xhtml.img,
                                new XAttribute(NoNamespace.src, imageFileName),
                                imageInfo.ImgStyleAttribute,
                                imageInfo.AltText != null ?
                                    new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                            return img;
                        }
                    };
                    var html = HtmlConverter.ConvertToHtml(wDoc, settings);

                    // Note: the xhtml returned by ConvertToHtmlTransform contains objects of type
                    // XEntity.  PtOpenXmlUtil.cs define the XEntity class.  See
                    // http://blogs.msdn.com/ericwhite/archive/2010/01/21/writing-entity-references-using-linq-to-xml.aspx
                    // for detailed explanation.
                    //
                    // If you further transform the XML tree returned by ConvertToHtmlTransform, you
                    // must do it correctly, or entities will not be serialized properly.

                    var htmlString = html.ToString(SaveOptions.DisableFormatting);
                    return htmlString;
                }
            }
        }
    }
}