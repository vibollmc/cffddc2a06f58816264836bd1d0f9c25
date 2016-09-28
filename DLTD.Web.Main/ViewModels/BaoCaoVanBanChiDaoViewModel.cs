using System;
using System.Collections.Generic;
using System.Data;
using DLTD.Web.Main.Common;

namespace DLTD.Web.Main.ViewModels
{
    public class BaoCaoVanBanChiDaoViewModel
    {
        public BaoCaoVanBanChiDaoViewModel()
        {
            ReportDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }

        public DataTable Data { get; set; }
        public string ReportDate { get; private set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; } 
        public Controllers.Report Report { get; set; }

        public string ReportPath { get; set; }
        public string ReportDataString { get; set; }
    }
}