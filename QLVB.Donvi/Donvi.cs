using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Donvi
{
    public class Donvi
    {
        // UBND tỉnh Đồng Nai
        //oY4Rfm2Aazi8l9qbvb+9QLubD+F+O5NZV3N9dYe0V+0=
        //UBND tỉnh Đồng Nai  -  00.02.H19.e-doc.vn
        private static string strtendonvi = "VĂN PHÒNG UBND TỈNH ĐỒNG NAI";

        // mã định danh theo vb 3158/UBND-TTTH ngày 25/04/2013
        private static string MaDinhDanh = "00.02.H19.e-doc.vn";

        // su dung module Theo doi Y kien chi dao
        private static bool YKCD = false;

        public static string GetTenDonVi()
        {
            return strtendonvi;
        }

        public static string GetMaDinhDanh()
        {
            return MaDinhDanh;
        }

        public static bool IsWorkFlow()
        {
            return true;
        }

        public static bool IsYKCD()
        {
            return YKCD;
        }
    }
}
