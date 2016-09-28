using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Common.Sessions
{
    /// <summary>
    /// interface for session and owin
    /// </summary>
    public interface ISessionServices
    {
        void InsertObject(string Key, object Obj);

        object GetObject(string Key);

        void RemoveObject(string Key);

        void ClearAllObject();

        object[] GetAllObject();

        int GetUserId();

        int GetRealUserId();

        void SessionStart();

        /// <summary>
        /// tach chuoi de lay cac gia tri search
        /// </summary>
        /// <param name="key">gia tri can tim</param>
        /// <param name="strSearchValues">chuoi search trong session</param>
        /// <returns>gia tri tim thay 
        /// khong tim thay: string.empty
        /// </returns>
        string GetStringSearchValues(string key, string strSearchValues);

        /// <summary>
        /// tach chuoi de lay cac gia tri search
        /// </summary>
        /// <param name="key">gia tri can tim</param>
        /// <param name="strSearchValues">chuoi search trong session</param>
        /// <returns>gia tri tim thay    
        /// khong tim thay: 0
        /// </returns>
        int GetIntSearchValues(string key, string strSearchValues);
    }
}
