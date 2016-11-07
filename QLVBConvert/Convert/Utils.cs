using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Configuration;


namespace Convert
{
    public class Stringconnect
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
    }


    public static class Utils
    {
        static readonly Logging Logger = new Logging();


        #region stringconnect

        public static void GetConnectionStrings()
        {
            var settings =
                ConfigurationManager.ConnectionStrings;

            if (settings == null) return;
            foreach (ConnectionStringSettings cs in settings)
            {
                var name = cs.Name;

                if (cs.Name == "QLVBDatabase")
                {
                    var provider = cs.ProviderName;
                    var connect = cs.ConnectionString;
                    var cs2 = cs.ElementInformation;

                    //cs.ConnectionString = "new connection string";

                }

            }
        }

        public static string GetConnectionStringByName(string name)
        {
            // Assume failure. 
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            var settings =
                ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string. 
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }

        public static string GetStrConnectCsdl(Stringconnect strconnect)
        {
            //string strservername = ".\\sqlexpress2005";
            //string strdatabase = "194";
            //string strusername = "sa";
            //string strpassword = "123456";
            var strservername = strconnect.Server;
            var strdatabase = strconnect.Database;
            var strusername = strconnect.Username;
            var strpassword = strconnect.Pass;
            var strConn = "Persist Security Info=true;User Id="
            + strusername + ";PASSWORD=" + strpassword + "; Initial Catalog=" + strdatabase + "; Data Source="
            + strservername + ";MultipleActiveResultSets=True;";
            return strConn;
        }

        public static SqlConnection GetSqlConnection(Stringconnect strconnect)
        {
            var strcon = Utils.GetStrConnectCsdl(strconnect);
            var sqlConnection1 = new SqlConnection(strcon);
            return sqlConnection1;
        }



        #endregion stringconnect

        public static int CountTable(Stringconnect strconnect)
        {
            var sqlConnection1 = Utils.GetSqlConnection(strconnect);
            var cmd = new SqlCommand
            {
                CommandText = SQLQuery.countTableInDatabase,
                CommandType = CommandType.Text,
                Connection = sqlConnection1
            };

            //cmd.CommandText = "SELECT COUNT(*) FROM Customers";

            sqlConnection1.Open();

            var returnValue = cmd.ExecuteScalar();

            sqlConnection1.Close();
            return System.Convert.ToInt32(returnValue);
        }

        /// <summary>
        /// chi dem cac truong trong database nguon
        /// </summary>
        /// <param name="table"></param>
        /// <param name="strconnectnguon"></param>
        /// <returns></returns>
        public static int CountFields(string table, Stringconnect strconnectnguon)
        {
            //string strcon = SQLQuery.GetStrConnectCSDLNguon();
            var sqlConnection1 = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();
            var count = 0;

            //cmd.CommandText = "SELECT COUNT(*) FROM Customers";
            cmd.CommandText = SQLQuery.countFieldsInTable(table);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                count++;
            }

            reader.Close();
            sqlConnection1.Close();

            return count;
        }

        /// <summary>
        /// dem record trong database nguon
        /// </summary>
        /// <param name="table"></param>
        /// <param name="strconnectnguon"></param>
        /// <returns></returns>
        public static int CountRows(string table, Stringconnect strconnectnguon)
        {
            var sqlConnection1 = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand
            {
                CommandText = SQLQuery.CountRows(table),
                CommandType = CommandType.Text,
                Connection = sqlConnection1
            };

            //cmd.CommandText = "SELECT COUNT(*) FROM Customers";

            sqlConnection1.Open();

            var returnValue = cmd.ExecuteScalar();

            sqlConnection1.Close();

            return System.Convert.ToInt32(returnValue);
        }


        public static void RunQuery(string sqlquery, Stringconnect strconnect)
        {
            var sqlConnection1 = GetSqlConnection(strconnect);
            var cmd = new SqlCommand
            {
                CommandText = sqlquery,
                CommandType = CommandType.Text,
                Connection = sqlConnection1
            };

            //cmd.CommandText = "UPDATE Customers SET ContactTitle = 'Sales Manager' WHERE CustomerID = 'ALFKI'";

            sqlConnection1.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Error(sqlquery);
                Logger.Error(ex.Message);
            }
            sqlConnection1.Close();
        }

        public static void RunQuery(string sqlquery, Stringconnect strconnect, SqlParameter[] lstParameters)
        {
            var sqlConnection1 = GetSqlConnection(strconnect);
            var cmd = new SqlCommand
            {
                CommandText = sqlquery,
                CommandType = CommandType.Text,
                Connection = sqlConnection1
            };

            cmd.Parameters.AddRange(lstParameters);
            //cmd.CommandText = "UPDATE Customers SET ContactTitle = 'Sales Manager' WHERE CustomerID = 'ALFKI'";

            sqlConnection1.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Error(sqlquery);
                Logger.Error(ex.Message);
            }
            sqlConnection1.Close();
        }


        /// <summary>
        /// insert vao database, khong co mo/dong ket noi
        /// </summary>
        /// <param name="sqlquery"></param>
        /// <param name="sqlConnection"></param>
        public static void InsertQuery(string sqlquery, SqlConnection sqlConnection)
        {
            var cmd = new SqlCommand
            {
                CommandText = sqlquery,
                CommandType = CommandType.Text,
                Connection = sqlConnection
            };

            //cmd.CommandText = "UPDATE Customers SET ContactTitle = 'Sales Manager' WHERE CustomerID = 'ALFKI'";
            //sqlConnection.Open();

            cmd.ExecuteNonQuery();
        }

        public static void UpdateQuery(string sqlquery, SqlConnection sqlConnection)
        {
            var cmd = new SqlCommand
            {
                CommandText = sqlquery,
                CommandType = CommandType.Text,
                Connection = sqlConnection
            };

            //cmd.CommandText = "UPDATE Customers SET ContactTitle = 'Sales Manager' WHERE CustomerID = 'ALFKI'";
            //sqlConnection.Open();
            cmd.ExecuteNonQuery();
        }

        #region GetValue
        public static string GetStringNullCheck(this IDataReader reader, string columnname)
        {
            if (HasColumn(reader, columnname))
            {
                var ordinal = reader.GetOrdinal(columnname);
                var value = reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                value = ConvertUnicode.Utf8ToUnicode(value);
                value = value.Replace("\"", "");
                value = value.Replace("'", "");
                return value;
            }
            Logger.Error("Not Found column: " + columnname);
            return null;
        }

        public static string GetStringNullCheck_Unicode(this IDataReader reader, string columnname)
        {
            if (HasColumn(reader, columnname))
            {
                var ordinal = reader.GetOrdinal(columnname);
                var value = reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
                return string.IsNullOrEmpty(value) ? null : value;
            }
            Logger.Error("Not Found column: " + columnname);
            return null;
        }

        public static int? GetIntNullCheck(this IDataReader reader, string columnname)
        {
            try
            {
                if (HasColumn(reader, columnname))
                {
                    var ordinal = reader.GetOrdinal(columnname);
                    try
                    {
                        var kq = reader.IsDBNull(ordinal) ? (int?) null : reader.GetInt32(ordinal);
                        //kq = (kq == 0) ? null : kq;
                        return kq;
                    }
                    catch
                    {
                        var kq = GetBitNullCheck(reader, columnname);
                        var intkq = (kq == true) ? 1 : (int?) null;
                        return intkq;
                    }
                }
                Logger.Error("Not Found column: " + columnname);
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error("Errror Column " + columnname + ". " + ex.Message);
                return null;
            }

        }
        public static int? GetIntNullCheck_NotLog(this IDataReader reader, string columnname)
        {
            try
            {
                if (HasColumn(reader, columnname))
                {
                    var ordinal = reader.GetOrdinal(columnname);
                    try
                    {
                        var kq = reader.IsDBNull(ordinal) ? (int?) null : reader.GetInt32(ordinal);
                        //kq = (kq == 0) ? null : kq;
                        return kq;
                    }
                    catch
                    {
                        var kq = GetBitNullCheck(reader, columnname);
                        var intkq = (kq == true) ? 1 : (int?) null;
                        return intkq;
                    }
                }
                //_logger.Error("Not Found column: " + columnname);
                return null;
            }
            catch (Exception ex)
            {
                //_logger.Error("Errror Column " + columnname + ". " + ex.Message);
                return null;
            }

        }

        public static int? GetSmallIntNullCheck(this IDataReader reader, string columnname)
        {
            if (HasColumn(reader, columnname))
            {
                var ordinal = reader.GetOrdinal(columnname);
                var kq = reader.IsDBNull(ordinal) ? (int?) null : reader.GetInt16(ordinal);
                return kq;
            }
            Logger.Error("Not Found column: " + columnname);
            return null;
        }

        public static DateTime? GetDateTimeNullCheck(this IDataReader reader, string columnname)
        {
            if (HasColumn(reader, columnname))
            {
                var ordinal = reader.GetOrdinal(columnname);
                return reader.IsDBNull(ordinal) ? (DateTime?) null : reader.GetDateTime(ordinal);
            }
            Logger.Error("Not Found column: " + columnname);
            return null;
        }

        public static bool? GetBitNullCheck(this IDataReader reader, string columnname)
        {
            if (HasColumn(reader, columnname))
            {
                var ordinal = reader.GetOrdinal(columnname);
                var kq = reader.IsDBNull(ordinal) ? (bool?) null : reader.GetBoolean(ordinal);
                return kq;
            }
            Logger.Error("Not Found column: " + columnname);
            return null;
        }

        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (var i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        #endregion GetValue




    }
}
