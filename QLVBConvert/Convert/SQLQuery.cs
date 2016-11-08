using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Convert
{
    public class SQLQuery
    {

        public static string GetAllTableInDatabase = "SELECT * from information_schema.tables WHERE table_type = 'base table' order by table_name ";

        // dem so table co trong database
        public static string countTableInDatabase = "SELECT COUNT(*) from information_schema.tables WHERE table_type = 'base table' ";

        public static string countFieldsInTable(string table)
        {
            string sql = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = '" + table + "'";
            return sql;
        }

        public static string CountRows(string table)
        {
            string sql = "select count(*) from " + table;
            return sql;
        }

        //alter table vanbanden add id1 int

        //alter table vanbanden drop column id1
        public static string AddColumn(string table, string columnName, string columnType)
        {
            string sql = "if not exists (select * from sys.columns where Name = N'" + columnName
                                        + "' and Object_ID = Object_ID(N'" + table + "') ) "
                + " begin alter table " + table + " add " + columnName + " " + columnType + " null end ";
            return sql;
        }

        public static string DropColumn(string table, string columnName)
        {
            string sql = "if exists (select * from sys.columns where Name = N'" + columnName
                                        + "' and Object_ID = Object_ID(N'" + table + "') ) "
                + " begin alter table " + table + " drop column " + columnName + " end ";
            return sql;
        }


        public static string UpdateId1(string table, string value, string whereid)
        {
            string sql = "update " + table + " set id1='" + value + "' where intid='" + whereid + "'";
            return sql;
        }

        #region CheckValue

        public static string FixDateTime(string table, string column)
        {
            string value = "1900 - 01 - 01";
            string sql = "update " + table + " set " + column + " =null where " + column + " = '" + value + "'";
            return sql;
        }


        #endregion CheckValue

    }
}
