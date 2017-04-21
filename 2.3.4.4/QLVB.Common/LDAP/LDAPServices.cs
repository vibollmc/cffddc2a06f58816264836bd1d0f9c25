using LinqToLdap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.Protocols;
using QLVB.Common.Utilities;

namespace QLVB.Common.LDAP
{
    public class LDAPServices
    {
        /// <summary>
        /// kiem tra user tren ldap
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <returns>
        /// -1: loi ldap
        ///  0: sai username, pass
        ///  1: thanh cong
        /// </returns>
        public static int ValidateUserLDAP(string username, string pass)
        {
            bool connectLDAPServer = ConnectToLDAPServer(AppSettings.ldapUser, AppSettings.ldapPass);

            if (connectLDAPServer)
            {
                bool checkUser = ConnectToLDAPServer(username, pass);
                if (checkUser)
                {   // dang nhap thanh cong
                    return 1;
                }
                else
                {   // dang nhap khong thanh cong
                    return 0;
                }
            }
            else
            {   // loi ket noi ldap
                return -1;
            }
        }

        /// <summary>
        /// kiem tra ket noi LDAP Server
        /// </summary>
        /// <returns></returns>
        private static bool ConnectToLDAPServer(string username, string password)
        {
            try
            {
                var ldapConfiguration = new LdapConfiguration()
                  .MaxPageSizeIs(50);

                System.Net.NetworkCredential userLdap = new System.Net.NetworkCredential(username, password);

                //config.ConfigurePooledFactory("192.168.2.200")
                //     .AuthenticateBy(AuthType.Ntlm)
                //     .AuthenticateAs(userLdap)
                //     .MinPoolSizeIs(0)
                //     .MaxPoolSizeIs(5)
                //     .UsePort(389)
                //     .ProtocolVersion(3);

                ldapConfiguration.ConfigureFactory(AppSettings.ldapServerName)
                        .AuthenticateAs(userLdap)
                        .UsePort(AppSettings.ldapServerPort);

                var connection = ldapConfiguration.ConnectionFactory.GetConnection();

                var example = new { Cn = "" };

                string nameContext = AppSettings.ldapCN + "," + AppSettings.ldapDC;
                //  "CN=Users,DC=vpubtdn,DC=org"

                using (var context = new DirectoryContext(connection))
                {
                    var user = context.Query(example, nameContext)
                        .Where(p => p.Cn == username)
                        .ToList();

                }

                ldapConfiguration.ConnectionFactory.ReleaseConnection(connection);

                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// kiem tra xem username da co trong ldap chua
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// true: da co
        /// false: chua co
        /// </returns>
        public static bool CheckUserNameLDAP(string username)
        {
            bool flag = false;
            try
            {
                var ldapConfiguration = new LdapConfiguration()
                  .MaxPageSizeIs(50);

                System.Net.NetworkCredential userLdap = new System.Net.NetworkCredential(
                                                        AppSettings.ldapUser,
                                                        AppSettings.ldapPass
                                                        );

                ldapConfiguration.ConfigureFactory(AppSettings.ldapServerName)
                        .AuthenticateAs(userLdap)
                        .UsePort(AppSettings.ldapServerPort);

                var connection = ldapConfiguration.ConnectionFactory.GetConnection();

                var example = new { Cn = "" };

                string nameContext = AppSettings.ldapCN + "," + AppSettings.ldapDC;
                //  "CN=Users,DC=vpubtdn,DC=org"

                using (var context = new DirectoryContext(connection))
                {
                    var user = context.Query(example, nameContext)
                        .Where(p => p.Cn == username)
                        .ToList();
                    if (user.Count > 0)
                    {
                        flag = true;
                    }
                }
                ldapConfiguration.ConnectionFactory.ReleaseConnection(connection);
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

    }
}
