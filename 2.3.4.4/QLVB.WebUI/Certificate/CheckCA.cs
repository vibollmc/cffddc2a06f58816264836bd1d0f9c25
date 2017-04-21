using QLVB.Common.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLVB.WebUI.Certificate
{
    public class CheckCA
    {
        private ISessionServices _logger;

        string strcheckCA = "";
        public CheckCA()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string get_strCheckCA()
        {

            return strcheckCA;
        }
        public void set_strCheckCA(string strcheckCA)
        {
            this.strcheckCA = strcheckCA;


        }
    }
}