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
    public class stringconnect
    {
        public string server { get; set; }
        public string database { get; set; }
        public string username { get; set; }
        public string pass { get; set; }
    }


    public static class Utils
    {
        static Logging _logger = new Logging();


        #region stringconnect

        public static void GetConnectionStrings()
        {
            ConnectionStringSettingsCollection settings =
                ConfigurationManager.ConnectionStrings;

            if (settings != null)
            {
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
        }

        public static string GetConnectionStringByName(string name)
        {
            // Assume failure. 
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string. 
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }

        public static string GetStrConnectCSDL(stringconnect strconnect)
        {
            //string strservername = ".\\sqlexpress2005";
            //string strdatabase = "194";
            //string strusername = "sa";
            //string strpassword = "123456";
            string strservername = strconnect.server;
            string strdatabase = strconnect.database;
            string strusername = strconnect.username;
            string strpassword = strconnect.pass;
            string strConn = "Persist Security Info=true;User Id="
            + strusername + ";PASSWORD=" + strpassword + "; Initial Catalog=" + strdatabase + "; Data Source="
            + strservername + ";MultipleActiveResultSets=True;";
            return strConn;
        }

        public static SqlConnection GetSqlConnection(stringconnect strconnect)
        {
            string strcon = Utils.GetStrConnectCSDL(strconnect);
            SqlConnection sqlConnection1 = new SqlConnection(strcon);
            return sqlConnection1;
        }



        #endregion stringconnect

        public static int CountTable(stringconnect strconnect)
        {
            SqlConnection sqlConnection1 = Utils.GetSqlConnection(strconnect);
            SqlCommand cmd = new SqlCommand();
            Object returnValue;

            //cmd.CommandText = "SELECT COUNT(*) FROM Customers";
            cmd.CommandText = SQLQuery.countTableInDatabase;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            returnValue = cmd.ExecuteScalar();

            sqlConnection1.Close();
            return System.Convert.ToInt32(returnValue);
        }

        /// <summary>
        /// chi dem cac truong trong database nguon
        /// </summary>
        /// <param name="table"></param>
        /// <param name="strconnectnguon"></param>
        /// <returns></returns>
        public static int CountFields(string table, stringconnect strconnectnguon)
        {
            //string strcon = SQLQuery.GetStrConnectCSDLNguon();
            SqlConnection sqlConnection1 = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            int count = 0;

            //cmd.CommandText = "SELECT COUNT(*) FROM Customers";
            cmd.CommandText = SQLQuery.countFieldsInTable(table);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            reader = cmd.ExecuteReader();

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
        public static int CountRows(string table, stringconnect strconnectnguon)
        {
            SqlConnection sqlConnection1 = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            Object returnValue;

            //cmd.CommandText = "SELECT COUNT(*) FROM Customers";
            cmd.CommandText = SQLQuery.CountRows(table);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            returnValue = cmd.ExecuteScalar();

            sqlConnection1.Close();

            return System.Convert.ToInt32(returnValue);
        }


        public static void RunQuery(string sqlquery, stringconnect strconnect)
        {
            SqlConnection sqlConnection1 = GetSqlConnection(strconnect);
            SqlCommand cmd = new SqlCommand();
            Int32 rowsAffected;

            //cmd.CommandText = "UPDATE Customers SET ContactTitle = 'Sales Manager' WHERE CustomerID = 'ALFKI'";
            cmd.CommandText = sqlquery;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();
            try
            {
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.Error(sqlquery);
                _logger.Error(ex.Message);
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
            SqlCommand cmd = new SqlCommand();
            Int32 rowsAffected;

            //cmd.CommandText = "UPDATE Customers SET ContactTitle = 'Sales Manager' WHERE CustomerID = 'ALFKI'";
            cmd.CommandText = sqlquery;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;
            //sqlConnection.Open();

            rowsAffected = cmd.ExecuteNonQuery();
        }

        public static void UpdateQuery(string sqlquery, SqlConnection sqlConnection)
        {
            SqlCommand cmd = new SqlCommand();
            Int32 rowsAffected;

            //cmd.CommandText = "UPDATE Customers SET ContactTitle = 'Sales Manager' WHERE CustomerID = 'ALFKI'";
            cmd.CommandText = sqlquery;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;
            //sqlConnection.Open();
            rowsAffected = cmd.ExecuteNonQuery();
        }

        #region GetValue
        public static string GetStringNullCheck(this IDataReader reader, string columnname)
        {
            if (HasColumn(reader, columnname))
            {
                int ordinal = reader.GetOrdinal(columnname);
                string value = reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                else
                {
                    value = ConvertUnicode.Utf8ToUnicode(value);
                    value = value.Replace("\"", "");
                    value = value.Replace("'", "");
                    return value;
                }
            }
            else
            {
                _logger.Error("Not Found column: " + columnname);
                return null;
            }
        }

        public static string GetStringNullCheck_Unicode(this IDataReader reader, string columnname)
        {
            if (HasColumn(reader, columnname))
            {
                int ordinal = reader.GetOrdinal(columnname);
                string value = reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }
            else
            {
                _logger.Error("Not Found column: " + columnname);
                return null;
            }
        }

        public static int? GetIntNullCheck(this IDataReader reader, string columnname)
        {
            try
            {
                if (HasColumn(reader, columnname))
                {
                    int ordinal = reader.GetOrdinal(columnname);
                    int? temp = null;
                    try
                    {
                        int? kq = reader.IsDBNull(ordinal) ? temp : reader.GetInt32(ordinal);
                        //kq = (kq == 0) ? null : kq;
                        return kq;
                    }
                    catch
                    {
                        bool? kq = GetBitNullCheck(reader, columnname);
                        int? intkq = (kq == true) ? 1 : temp;
                        return intkq;
                    }
                }
                else
                {
                    _logger.Error("Not Found column: " + columnname);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Errror Column " + columnname + ". " + ex.Message);
                return null;
            }

        }
        public static int? GetIntNullCheck_NotLog(this IDataReader reader, string columnname)
        {
            try
            {
                if (HasColumn(reader, columnname))
                {
                    int ordinal = reader.GetOrdinal(columnname);
                    int? temp = null;
                    try
                    {
                        int? kq = reader.IsDBNull(ordinal) ? temp : reader.GetInt32(ordinal);
                        //kq = (kq == 0) ? null : kq;
                        return kq;
                    }
                    catch
                    {
                        bool? kq = GetBitNullCheck(reader, columnname);
                        int? intkq = (kq == true) ? 1 : temp;
                        return intkq;
                    }
                }
                else
                {
                    //_logger.Error("Not Found column: " + columnname);
                    return null;
                }
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
                int ordinal = reader.GetOrdinal(columnname);
                int? temp = null;
                int? kq = reader.IsDBNull(ordinal) ? temp : reader.GetInt16(ordinal);
                return kq;
            }
            else
            {
                _logger.Error("Not Found column: " + columnname);
                return null;
            }
        }

        public static DateTime? GetDateTimeNullCheck(this IDataReader reader, string columnname)
        {
            if (HasColumn(reader, columnname))
            {
                int ordinal = reader.GetOrdinal(columnname);
                DateTime? thoigian = null;
                return reader.IsDBNull(ordinal) ? thoigian : reader.GetDateTime(ordinal);
            }
            else
            {
                _logger.Error("Not Found column: " + columnname);
                return null;
            }
        }

        public static bool? GetBitNullCheck(this IDataReader reader, string columnname)
        {
            if (HasColumn(reader, columnname))
            {
                int ordinal = reader.GetOrdinal(columnname);
                bool? kq = null;
                kq = reader.IsDBNull(ordinal) ? kq : reader.GetBoolean(ordinal);
                return kq;
            }
            else
            {
                _logger.Error("Not Found column: " + columnname);
                return null;
            }
        }

        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        #endregion GetValue




    }
}
