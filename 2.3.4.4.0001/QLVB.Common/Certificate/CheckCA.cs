using QLVB.Common.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Common.Utilities;

namespace QLVB.Common.Certificate
{
    public class CheckCA : ICheckCA
    {

        private ISessionServices _session;
        public CheckCA(ISessionServices session)
        {
            _session = session;
        }
        public string get_strCheckCA()
        {
            try
            {
                if (!AppSettings.IsCA)
                {
                    set_strCheckCA("OK");
                }
                
                var strcheckCA = _session.GetObject("CA").ToString();

                return strcheckCA;
            }
            catch
            {
                return "";
            }

        }
        public void set_strCheckCA(string strcheckCA)
        {
            _session.InsertObject("CA", strcheckCA);


        }
    }
}
