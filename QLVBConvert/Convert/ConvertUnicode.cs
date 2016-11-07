using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Convert
{
    public class ConvertUnicode
    {
        public static string Utf8ToUnicode(string utf8String)
        {

            if (String.IsNullOrEmpty(utf8String))
            {
                return "";
            }
            else
            {
                var raw_db_string = utf8String;
                var sourceEncoding = Encoding.GetEncoding(1252);
                var targetEncoding = Encoding.GetEncoding(65001);

                var encodeTheseChars = sourceEncoding.GetBytes(raw_db_string);

                var recoded_string = targetEncoding.GetString(encodeTheseChars);

                return recoded_string;
            }
        }
        public static string UnicodeToUtf8(string unicodeString)
        {
            if (String.IsNullOrEmpty(unicodeString))
            {
                return "";
            }
            else
            {
                var raw_db_string = unicodeString;
                var sourceEncoding = Encoding.GetEncoding(65001);
                var targetEncoding = Encoding.GetEncoding(1252);

                var encodeTheseChars = sourceEncoding.GetBytes(raw_db_string);

                var recoded_string = targetEncoding.GetString(encodeTheseChars);

                return recoded_string;
            }

        }
    }
}
