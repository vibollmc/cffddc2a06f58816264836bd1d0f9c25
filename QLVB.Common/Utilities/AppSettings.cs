using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace QLVB.Common.Utilities
{
    public class AppSettings
    {
        #region Private Methods

        private static string GetValue(string Key)
        {
            AppSettingsReader reader = new AppSettingsReader();
            string Value = reader.GetValue(Key, typeof(string)).ToString();
            if (!string.IsNullOrEmpty(Value))
            {
                return Value;
            }
            return string.Empty;
        }

        private static string GetString(string Key, string DefaultValue)
        {
            string Setting = GetValue(Key);
            if (!string.IsNullOrEmpty(Setting))
            {
                return Setting;
            }
            return DefaultValue;
        }

        private static bool GetBool(string Key, bool DefaultValue)
        {
            string Setting = GetValue(Key);
            if (!string.IsNullOrEmpty(Setting))
            {
                switch (Setting.ToLower())
                {
                    case "false":
                    case "0":
                    case "n":
                        return false;
                    case "true":
                    case "1":
                    case "y":
                        return true;
                }
            }
            return DefaultValue;
        }

        private static int GetInt(string Key, int DefaultValue)
        {
            string Setting = GetValue(Key);
            if (!string.IsNullOrEmpty(Setting))
            {
                int i;
                if (int.TryParse(Setting, out i))
                {
                    return i;
                }
            }
            return DefaultValue;
        }

        private static double GetDouble(string Key, double DefaultValue)
        {
            string Setting = GetValue(Key);
            if (!string.IsNullOrEmpty(Setting))
            {
                double d;
                if (double.TryParse(Setting, out d))
                {
                    return d;
                }
            }
            return DefaultValue;
        }

        private static byte GetByte(string Key, byte DefaultValue)
        {
            string Setting = GetValue(Key);
            if (!string.IsNullOrEmpty(Setting))
            {
                byte b;
                if (byte.TryParse(Setting, out b))
                {
                    return b;
                }
            }
            return DefaultValue;
        }

        #endregion Private Methods

        /// <summary>
        /// load config tu file AppSettings.config
        /// </summary>
        #region Thongtin chung

        public static string SiteName
        {
            get { return GetString("SiteName", "HỆ THỐNG QUẢN LÝ VĂN BẢN"); }
        }

        public static string DefaultKendoTheme
        {
            get { return GetString("DefaultKendoTheme", "bootstrap"); }
        }

        public static string SideBarLeft
        {
            get { return GetString("SideBarLeft", "sidebar-left-mini"); }
        }

        public static bool FixHeaderBar
        {
            get { return GetBool("FixHeaderBar", false); }
        }

        public static string Noidung
        {
            get { return GetString("Noidung", "~/Noidung"); }
        }

        public static string BackupDatabase
        {
            get { return GetString("BackupDatabase", "~/App_Data"); }
        }

        public static int DisplayItemsPerPage
        {
            get { return GetInt("DisplayItemsPerPage", 20); }
        }

        public static string DefaultSiteTheme
        {
            get
            {
                string theme = GetString("DefaultSiteTheme", "Metro");
                theme = "~/Content/themes/" + theme;
                return theme;
            }
        }
        /// <summary>
        /// quy dinh nhap tra loi vb trong them moi vanbanden/di
        /// </summary>
        public static string TraloiVB
        {
            get { return GetString("TraloiVB", "1-T-YYYY"); }
        }

        /// <summary>
        /// quy dinh dat ten van ban de dinh kem file tu dong
        /// </summary>
        public static string QDTenVBAttach
        {
            get { return GetString("QDTenVBAttach", "T-1-YYYY"); }
        }

        public static bool IsWorkflow
        {
            get
            {
                return GetBool("IsWorkflow", false);
            }
        }

        public static bool IsKetthucHosoVanban
        {
            get
            {
                return GetBool("IsKetthucHosoVanban", false);
            }
        }

        #endregion Thong tin chung

        #region dinhkemfile
        /// <summary>
        /// cho phep bat/tat tu dong dinh kem file 
        /// </summary>
        public static bool AutoSynch
        {
            get { return GetBool("AutoSynch", false); }
        }

        public static string UserSynch
        {
            get { return GetString("UserSynch", "admin"); }
        }

        public static string PassSynch
        {
            get { return GetString("PassSynch", "admin"); }
        }
        #endregion dinhkemfile

        #region VBDT

        public static bool IsEmailBTTTT
        {
            get
            {
                DateTime dtengayht = DateTime.Now;
                string strngaybd = GetString("EmailBTTTT", "30/05/2014");
                DateTime? dtengaybd = Date.DateServices.FormatDateEn(strngaybd);

                return (dtengayht > dtengaybd) ? true : false;
            }
        }
        public static int LoaiVBDT
        {
            get { return GetInt("LoaiVBDT", 2803); }
        }
        public static bool MahoaFileBase64
        {
            get
            {
                return GetBool("MahoaFileBase64", false);
            }
        }
        public static bool IsZipFile
        {
            get
            {
                return GetBool("IsZipFile", false);
            }
        }
        public static bool IsLogVBDTDen
        {
            get
            {
                return GetBool("IsLogVBDTDen", false);
            }
        }
        public static bool IsLogVBDTDi
        {
            get
            {
                return GetBool("IsLogVBDTDi", false);
            }
        }
        public static bool IsAutoReceiveMail
        {
            get
            {
                return GetBool("IsAutoReceiveMail", false);
            }
        }
        public static bool IsAutoSendMail
        {
            get
            {
                return GetBool("IsAutoSendMail", false);
            }
        }
        public static int AutoEmailPerRequest
        {
            get
            {
                int count = GetInt("AutoEmailPerRequest", 10);
                if (count > 30)
                {
                    count = 30;
                }
                return count;
            }
        }
        public static int TimeAutoEmail
        {
            get
            {
                int time = GetInt("TimeAutoEmail", 10);
                if (time < 5)
                {
                    time = 5;
                }
                return time;
            }
        }


        #endregion VBDT

        #region System

        public static bool IsFullText
        {
            get { return GetBool("IsFullText", false); }
        }

        public static int LoaiVBDen
        {
            get
            {
                return GetInt("LoaiVBDen", 1);
            }
        }
        public static bool Partition
        {
            get { return GetBool("Partition", false); }
        }

        #endregion System

        #region LienthongEdxml
        public static bool IsEdxml
        {
            get { return GetBool("IsEdxml", false); }
        }
        public static string ServiceEdxml
        {
            get { return GetString("ServiceEdxml", "https://ketnoiquocgia.tphcm.gov.vn/ihorae"); }
        }

        public static string MaEdxmlDiaphuong
        {
            get { return GetString("MaEdxmlDiaphuong", "H19"); }
        }

        public static string DiadanhDiaphuong
        {
            get { return GetString("DiadanhDiaphuong", "Đồng Nai"); }
        }

        #endregion LienthongEdxml

        #region Sovanbandi

        public static bool IsSoVBDi
        {
            get { return GetBool("IsSoVBDi", false); }
        }

        public static DateTime? NgayBDSoVBDi
        {
            get
            {
                string strngaybd = GetString("NgayBDSoVBDi", "01/01/2016");
                DateTime? dtengaybd = Date.DateServices.FormatDateEn(strngaybd);

                return dtengaybd;
            }
        }

        #endregion Sovanbandi
    }
}
