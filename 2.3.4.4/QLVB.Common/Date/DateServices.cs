using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Common.Date
{
    // Vietnamese( vie-vi )
    // English( eng-en )
    public class DateServices
    {
        /// <summary>
        /// chuyen ngay anh mm/dd/yyyy sang ngay viet dd/mm/yyyy
        /// </summary>
        /// <param name="strngayEn"></param>
        /// <returns></returns>
        public static string FormatDateVN(string strngayEn)
        {
            //input: mm/dd/yyyy
            //output: dd/mm/yyy
            try
            {
                string ngay = strngayEn.Substring(3, 2);
                string thang = strngayEn.Substring(0, 2);
                string nam = strngayEn.Substring(strngayEn.Length - 4, 4);
                string _date = ngay + "/" + thang + "/" + nam;
                return _date;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// chuyen ngay Anh mm/dd/yyyy sang ngay Viet dd/mm/yyyy
        /// </summary>
        /// <param name="dtengayEn"></param>
        /// <returns>chuoi: dd/mm/yyy</returns>
        public static string FormatDateVN(DateTime? dtengayEn)
        {
            //input: mm/dd/yyyy
            //output: dd/mm/yyy
            try
            {
                string strngayVN = String.Format("{0:dd/MM/yyyy}", dtengayEn);
                return strngayVN;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// chuyen ngay Anh mm/dd/yyyy sang ngay Viet dd/mm/yyy co gio
        /// </summary>
        /// <param name="dtengayEn"></param>
        /// <returns>ngay: dd/mm/yyyy co gio</returns>
        //public static DateTime FormatDateVN_1(DateTime? dtengayEn)
        //{
        //    CultureInfo vi = new System.Globalization.CultureInfo("vi-VN");
        //    DateTime strngayVN1 = Convert.ToDateTime(dtengayEn, vi).Date;
        //    //string strngayVN = String.Format("{0:dd/MM/yyyy}", dtengayEn);
        //    //DateTime dt = DateTime.ParseExact(strngayVN, "dd/MM/yyyy", null);
        //    //DateTime dtengayVN = Convert.ToDateTime(strngayVN);
        //    //DateTime date2 = new DateTime(strngayVN1.Year, strngayVN1.Month, strngayVN1.Day);

        //    return strngayVN1;
        //}

        /// <summary>
        /// chuyen ngay gio Anh sang ngay gio Viet
        /// </summary>
        /// <param name="dtengayEn"></param>
        /// <returns>dd/mm/yyyy hh:mm:ss</returns>
        public static string FormatDateTimeVN(DateTime? dtengayEn)
        {
            //input: mm/dd/yyyy hh:mm:ss
            //output: dd/mm/yyy hh:mm:ss
            if (dtengayEn != null)
            {
                string strngayVN = String.Format("{0:dd/MM/yyyy HH:mm:ss}", dtengayEn);
                return strngayVN;
            }
            else
            {
                return string.Empty;
            }

        }
        /// <summary>
        /// chuyen datetime to string
        /// </summary>
        /// <param name="dteNgayEn"></param>
        /// <returns></returns>
        public static string FormatDateTimeEn(DateTime? dteNgayEn)
        {
            if (dteNgayEn != null)
            {
                string strngayVN = String.Format("{0:MM/dd/yyyy HH:mm:ss}", dteNgayEn);
                return strngayVN;
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// chuyen doi string ngay gio VN sang datetimeEn
        /// </summary>
        /// <param name="strNgaygioVN"></param>
        /// <returns></returns>
        public static DateTime? FormatDateTimeEn(string strNgaygioVN)
        {
            if (!string.IsNullOrEmpty(strNgaygioVN))
            {
                DateTime dt = DateTime.ParseExact(strNgaygioVN, "dd/MM/yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
                return dt;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// chuyen dinh dang ngay Viet dd/mm/yyyy sang ngay Anh mm/dd/yyy
        /// </summary>
        /// <param name="strngayVn"></param>
        /// <returns>mm/dd/yyyy</returns>
        public static DateTime? FormatDateEn(string strngayVn)
        {
            //input: dd/mm/yyyy
            //output: mm/dd/yyy
            if (!string.IsNullOrEmpty(strngayVn))
            {
                string ngay = strngayVn.Substring(0, 2);
                string thang = strngayVn.Substring(3, 2);
                string nam = strngayVn.Substring(strngayVn.Length - 4, 4);
                string _date3 = thang + "/" + ngay + "/" + nam;

                DateTime _date2 = DateTime.Now;
                _date2 = Convert.ToDateTime(_date3);

                return _date2;
            }
            else
            {
                return DateTime.Now;
            }
        }


        /// <summary>
        /// chuyen ngay Anh sang string MM/dd/yyyy
        /// </summary>
        /// <param name="dteNgayEn"></param>
        /// <returns></returns>
        public static string FormatDateEn(DateTime? dteNgayEn)
        {
            try
            {
                string strngay = String.Format("{0:MM/dd/yyyy}", dteNgayEn);
                return strngay;
            }
            catch
            {
                return null;
            }

        }
        /// <summary>
        /// dung trong category van ban
        /// </summary>
        /// <param name="dtengayEn">ngay hien tai </param>
        /// <param name="intsongay">so ngay lui lai so voi ngay hien tai</param>
        /// <returns></returns>
        public static string FormatDateVNCategory(DateTime dtengayEn, int intsongay)
        {
            //input: mm/dd/yyyy
            //output: dd/mm/yyy
            try
            {
                DateTime dtengay = dtengayEn.AddDays(-intsongay);
                string strngayVN = String.Format("{0:dd/MM/yyyy}", dtengay);
                //string ngay = dtengayEn.Day.ToString();
                //string thang = dtengayEn.Month.ToString();
                //string nam = dtengayEn.Year.ToString();
                //string _date = ngay + "/" + thang + "/" + nam;
                return strngayVN;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// cong them so ngay vao ngay muon them
        /// </summary>
        /// <param name="dtengayEn">ngay hien tai</param>
        /// <param name="intsongay">so ngay them vao</param>
        /// <returns></returns>
        public static string AddDateVN(DateTime dtengayEn, int intsongay)
        {
            //input: mm/dd/yyyy
            //output: dd/mm/yyy
            try
            {
                DateTime dtengay = dtengayEn.AddDays(intsongay);
                string strngayVN = String.Format("{0:dd/MM/yyyy}", dtengay);
                return strngayVN;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// cong them so ngay vao ngay muon them;
        /// khong tinh thu bay, cn, ngay nghi
        /// </summary>
        /// <param name="dtengayEn">ngay hien tai</param>
        /// <param name="intsongay">so ngay them vao</param>
        /// <returns></returns>
        public static DateTime AddThoihanxuly(DateTime dtengayEn, int intsongay)
        {
            //input: mm/dd/yyyy
            //output: dd/mm/yyy
            try
            {
                DateTime dtengay = dtengayEn;
                DateTime dtengaymoi = dtengayEn;
                while (intsongay > 0)
                {
                    dtengay = dtengaymoi.AddDays(1);
                    dtengaymoi = dtengay;
                    if (dtengay.DayOfWeek < DayOfWeek.Saturday &&
                        dtengay.DayOfWeek > DayOfWeek.Sunday &&
                        !IsNgayNghi(dtengay))
                    {
                        intsongay--;
                    }
                }
                return dtengay;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        /// <summary>
        /// kiem tra co phai la ngay nghi khong
        /// 30/4,1/5,2/9
        /// chua tinh cac ngay nghi them va lam bu (gio to hung vuong 10/3)
        /// </summary>
        /// <param name="dteNgayEn"></param>
        /// <returns>false: neu khong la ngay nghi
        ///          true: la ngay nghi</returns>
        public static bool IsNgayNghi(DateTime dteNgayEn)
        {
            try
            {
                DateTime dtengay = dteNgayEn;
                bool flag = false;
                if (dtengay.Day == 30 && dtengay.Month == 4) { flag = true; }
                if (dtengay.Day == 1 && dtengay.Month == 5) { flag = true; }
                if (dtengay.Day == 2 && dtengay.Month == 9) { flag = true; }
                return flag;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// dem so ngay them vao tu ngay bat dau den ngay ket thuc. khong tinh t7,cn, ngay le
        /// dung trong quy trinh
        /// </summary>
        /// <param name="dteNgaybd"></param>
        /// <param name="dteNgaykt"></param>
        /// <returns></returns>
        public static int DemSongayThemvao(DateTime dteNgaybd, DateTime dteNgaykt)
        {
            try
            {
                int intsongay = 0;
                // so ngay giua 2 ngay bd va kt
                intsongay = (dteNgaykt - dteNgaybd).Days;

                DateTime dtengay = dteNgaybd;
                DateTime dtengaymoi = dteNgaybd;

                // so ngay lam viec trong tuan
                int countngay = intsongay;

                for (int i = 0; i < intsongay; i++)
                {
                    dtengay = dtengaymoi.AddDays(1);
                    dtengaymoi = dtengay;
                    if ((dtengay.DayOfWeek == DayOfWeek.Saturday) ||
                        (dtengay.DayOfWeek == DayOfWeek.Sunday) ||
                        (IsNgayNghi(dtengay)))
                    {
                        countngay--;
                    }
                }
                return countngay;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //public static DateTime AddWorkdays(this DateTime originalDate, int workDays)
        //{
        //    DateTime tmpDate = originalDate;
        //    while (workDays > 0)
        //    {
        //        tmpDate = tmpDate.AddDays(1);
        //        if (tmpDate.DayOfWeek < DayOfWeek.Saturday &&
        //            tmpDate.DayOfWeek > DayOfWeek.Sunday)// &&
        //            //tmpDate.IsHoliday())
        //            workDays--;
        //    }
        //    return tmpDate;
        //}

        //public static bool IsHoliday(this DateTime originalDate)
        //{
        //    // INSERT YOUR HOlIDAY-CODE HERE!
        //    return false;
        //}

        /// <summary>
        /// lay ngay dau tuan cua ngay hien tai
        /// </summary>
        /// <param name="dtengayhientai"></param>
        /// <returns></returns>
        public static DateTime GetDauTuan(DateTime dtengayhientai)
        {
            try
            {
                DateTime dtedautuan = dtengayhientai;

                if (dtengayhientai.DayOfWeek == DayOfWeek.Monday) { dtedautuan = dtengayhientai; }
                if (dtengayhientai.DayOfWeek == DayOfWeek.Tuesday) { dtedautuan = dtengayhientai.AddDays(-1); }
                if (dtengayhientai.DayOfWeek == DayOfWeek.Wednesday) { dtedautuan = dtengayhientai.AddDays(-2); }
                if (dtengayhientai.DayOfWeek == DayOfWeek.Thursday) { dtedautuan = dtengayhientai.AddDays(-3); }
                if (dtengayhientai.DayOfWeek == DayOfWeek.Friday) { dtedautuan = dtengayhientai.AddDays(-4); }
                if (dtengayhientai.DayOfWeek == DayOfWeek.Saturday) { dtedautuan = dtengayhientai.AddDays(-5); }
                if (dtengayhientai.DayOfWeek == DayOfWeek.Sunday) { dtedautuan = dtengayhientai.AddDays(-6); }

                return dtedautuan;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        /// <summary>
        /// lay ngay cuoi tuan cua ngay hien tai
        /// </summary>
        /// <param name="dtengayhientai"></param>
        /// <returns></returns>
        public static DateTime GetCuoiTuan(DateTime dtengayhientai)
        {
            try
            {
                DateTime dtecuoituan = dtengayhientai;

                if (dtengayhientai.DayOfWeek == DayOfWeek.Monday) { dtecuoituan = dtengayhientai.AddDays(4); }
                if (dtengayhientai.DayOfWeek == DayOfWeek.Tuesday) { dtecuoituan = dtengayhientai.AddDays(3); }
                if (dtengayhientai.DayOfWeek == DayOfWeek.Wednesday) { dtecuoituan = dtengayhientai.AddDays(2); }
                if (dtengayhientai.DayOfWeek == DayOfWeek.Thursday) { dtecuoituan = dtengayhientai.AddDays(1); }
                if (dtengayhientai.DayOfWeek == DayOfWeek.Friday) { dtecuoituan = dtengayhientai; }
                if (dtengayhientai.DayOfWeek == DayOfWeek.Saturday) { dtecuoituan = dtengayhientai.AddDays(-1); }
                if (dtengayhientai.DayOfWeek == DayOfWeek.Sunday) { dtecuoituan = dtengayhientai.AddDays(-2); }

                return dtecuoituan;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

    }
}
