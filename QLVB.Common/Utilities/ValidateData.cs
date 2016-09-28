using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Common.Logging;

namespace QLVB.Common.Utilities
{
    public class ValidateData
    {
        //http://www.joellipman.com/articles/web-development/503-basic-tests-for-sql-injection-vulnerabilities.html
        public static string Escape(string data)
        {
            var builder = new StringBuilder();
            foreach (var ch in data)
            {
                switch (ch)
                {
                    case '\\': // Backslash
                        builder.AppendFormat("\\\\");
                        break;
                    case '\r': // Carriage return
                        builder.AppendFormat("\\r");
                        break;
                    case '\n': // New Line
                        builder.AppendFormat("\\n");
                        break;
                    case '\a': // Vertical tab
                        builder.AppendFormat("\\a");
                        break;
                    case '\b': // Backspace
                        builder.AppendFormat("\\b");
                        break;
                    case '\f': // Formfeed
                        builder.AppendFormat("\\f");
                        break;
                    case '\t': // Horizontal tab
                        builder.AppendFormat("\\t");
                        break;
                    case '\v': // Vertical tab
                        builder.AppendFormat("\\v");
                        break;
                    case '\"': // Double quotation mark
                        builder.AppendFormat("\\\"");
                        break;
                    case '\'': // Single quotation mark
                        builder.AppendFormat("\\\'");
                        break;
                    default:
                        builder.Append(ch);
                        break;
                }
            }
            return builder.ToString();
        }

        //http://forums.asp.net/t/1254125.aspx?Coding+techniques+for+protecting+against+Sql+injection

        public static string[] blackList = {
                                         "--", ";--", "/*", "*/", "@@","@", "'",
                                        "char", "nchar", "varchar", "nvarchar", 
                                        "alter", "begin", "cast", "create", "cursor", "declare", "delete", 
                                        "drop", "end", "exec", "execute", "fetch", "insert", "kill", "open", 
                                        "select", "sys", "sysobjects", "syscolumns", 
                                        "table", "update", 
                                        "xp_", "union", "char(0)"
                                    };

        public static string CheckInput(string parameter)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                var builder = new StringBuilder(parameter);
                for (int i = 0; i < blackList.Length; i++)
                {
                    if ((parameter.IndexOf(blackList[i], StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        //Handle the discovery of suspicious Sql characters here

                        builder.Replace(blackList[i], blackList[i] + "_");
                    }
                }
                return builder.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')
                    || (c == '.') || (c == '_'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// tim kiem chinh xac "abc"
        /// </summary>
        /// <param name="str">"abc"</param>
        /// <returns>abc</returns>
        public static Dictionary<bool, string> SearchExactly(string str)
        {
            var sb = new StringBuilder(str);
            Dictionary<bool, string> result = new Dictionary<bool, string>();
            bool flag = false;
            if (sb[0].ToString() == "\"")
            {
                if ((sb[sb.Length - 1].ToString() == "\""))
                {
                    sb = sb.Remove(0, 1);
                    sb = sb.Remove(sb.Length - 1, 1);
                    flag = true;
                }
            }
            result.Add(flag, sb.ToString());
            return result;
        }
        public enum enumFullTextSearch
        {
            AND = 1,
            OR = 2
        }

        /// <summary>
        /// ghep chuoi thanh __ and __ hoac __ or___
        /// dung trong full text search
        /// </summary>
        /// <param name="values"></param>
        /// <param name="SearchAndOr">1: and, 2: or</param>
        /// <returns></returns>
        public static string GhepChuoiFullTextSearch(string values, int SearchAndOr)
        {
            string strAndOr = string.Empty;
            strAndOr = (SearchAndOr == (int)enumFullTextSearch.AND) ? "AND" : "OR";
            var sb = new StringBuilder();
            string[] keywords = values.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in keywords)
            {
                sb.Append(item + " " + strAndOr + " ");
            }
            string result = sb.ToString(0, sb.Length - strAndOr.Length - 2); // tru and/or va 2 khoang trang
            return result;
        }
    }
}
