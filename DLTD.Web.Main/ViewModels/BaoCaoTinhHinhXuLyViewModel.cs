using System;
using System.Data;
using Microsoft.SqlServer.Server;

namespace DLTD.Web.Main.ViewModels
{
    public class BaoCaoTinhHinhXuLyViewModel
    {
        public BaoCaoTinhHinhXuLyViewModel()
        {
            this.NgayBaoCao = string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now);
        }
        public string NgayBaoCao { get; set; }
        public string BaoCaoTheo { get; set; }
        public string DieuKienTimKiem { get; set; }
        public DataTable Data { get; set; }
    }
}