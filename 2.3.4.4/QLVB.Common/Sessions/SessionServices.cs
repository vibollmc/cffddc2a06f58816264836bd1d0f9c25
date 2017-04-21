using QLVB.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLVB.Common.Sessions
{
    public class SessionServices
    {
        public void InsertObject(string Key, object Obj)
        {
            //if ((Obj != null))
            //{
            //if (HttpContext.Current.Session[Key] == null)
            //{
            HttpContext.Current.Session[Key] = Obj;
            //}
            //}
        }

        public object GetObject(string Key)
        {
            return HttpContext.Current.Session[Key];
        }

        public void RemoveObject(string Key)
        {
            if ((HttpContext.Current.Session[Key] != null))
            {
                HttpContext.Current.Session[Key] = null;
            }
        }

        public void ClearAllObject()
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
        }

        public object[] GetAllObject()
        {
            List<object> value = new List<object>();
            foreach (string key in HttpContext.Current.Session.Keys)
            {
                value.Add(HttpContext.Current.Session[key]);
            }
            return value.ToArray();
        }

        public int GetUserId()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session[AppConts.SessionUserId]);
            return userid;
        }

        public int GetRealUserId()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session[AppConts.SessionRealUserId]);
            return userid;
        }

        public void SessionStart()
        {
            InsertObject(AppConts.SessionUserId, 0);
            //InsertObject(AppConts.SessionDonviId, 0);
            //InsertObject(AppConts.SessionUserName, "");
            InsertObject(AppConts.SessionUserRoles, "");
            //InsertObject(AppConts.SessionMyRoute, "");

            InsertObject(AppConts.SessionSearchPageType, EnumSession.PageType.NoPage);
            InsertObject(AppConts.SessionSearchPageValues, 0);

            InsertObject(AppConts.SessionSearchType, EnumSession.SearchType.NoSearch);
            InsertObject(AppConts.SessionSearchTypeValues, "");

            //InsertObject(AppConts.SessionPageVBDen, 1);
            //InsertObject(AppConts.SessionPageVBDi, 1);
            //InsertObject(AppConts.SessionSearchVBDen, "");
            //InsertObject(AppConts.SessionSearchVBDi, "");

            // khong duoc reset vao realuserid
            // chi duoc insert duy nhat khi login
            //InsertObject(AppConts.SessionRealUserId, 0);
        }

        //public static void SessionStop()
        //{
        //    RemoveObject("UserId");
        //    RemoveObject("UserName");
        //    RemoveObject("UserRoles");
        //    RemoveObject("MyRoute");
        //}

        /// <summary>
        /// tach chuoi de lay cac gia tri search
        /// </summary>
        /// <param name="key">gia tri can tim</param>
        /// <param name="strSearchValues">chuoi search trong session</param>
        /// <returns>gia tri tim thay 
        /// khong tim thay: string.empty
        /// </returns>
        public string GetStringSearchValues(string key, string strSearchValues)
        {
            string strketqua = string.Empty;
            string[] _searchvalue = strSearchValues.Split(new Char[] { ';' });
            string strgiatri = string.Empty;
            foreach (var p in _searchvalue)
            {
                strgiatri = p;
                // p="intsodenbd=1"
                if (strgiatri.Contains(key))
                {
                    int len = key.Length;
                    strketqua = strgiatri.Substring(len + 1);
                }
            }
            return strketqua;
        }

        /// <summary>
        /// tach chuoi de lay cac gia tri search
        /// </summary>
        /// <param name="key">gia tri can tim</param>
        /// <param name="strSearchValues">chuoi search trong session</param>
        /// <returns>gia tri tim thay    
        /// khong tim thay: 0
        /// </returns>
        public int GetIntSearchValues(string key, string strSearchValues)
        {
            string strketqua = GetStringSearchValues(key, strSearchValues);
            if (string.IsNullOrEmpty(strketqua))
            {
                strketqua = "0";
            }
            return Convert.ToInt32(strketqua);
        }
    }
}
