using System.Collections.Generic;
using DocumentFormat.OpenXml.Spreadsheet;

namespace QLVB.WebUI.Common.OpenXML
{
    /// <summary>
    /// Class to help to build an Excel workbook
    /// </summary>
    /// <remarks>
    /// The following code can be found on Code Project at:
    /// http://www.codeproject.com/Articles/670141/Read-and-Write-Microsoft-Excel-with-Open-XML-SDK
    /// </remarks>
    public class SLExcelStatus
    {
        public string Message { get; set; }
        public bool Success
        {
            get { return string.IsNullOrWhiteSpace(Message); }
        }
    }

    public class SLExcelData
    {
        public SLExcelStatus Status { get; set; }
        public Columns ColumnConfigurations { get; set; }
        public List<string> Headers { get; set; }
        public List<List<string>> DataRows { get; set; }
        public string SheetName { get; set; }

        public SLExcelData()
        {
            Status = new SLExcelStatus();
            Headers = new List<string>();
            DataRows = new List<List<string>>();
        }
    }
}