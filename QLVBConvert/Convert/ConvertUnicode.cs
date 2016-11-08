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
                String raw_db_string = utf8String;
                Encoding sourceEncoding = Encoding.GetEncoding(1252);
                Encoding targetEncoding = Encoding.GetEncoding(65001);

                byte[] encodeTheseChars = sourceEncoding.GetBytes(raw_db_string);

                String recoded_string = targetEncoding.GetString(encodeTheseChars);

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
                String raw_db_string = unicodeString;
                Encoding sourceEncoding = Encoding.GetEncoding(65001);
                Encoding targetEncoding = Encoding.GetEncoding(1252);

                byte[] encodeTheseChars = sourceEncoding.GetBytes(raw_db_string);

                String recoded_string = targetEncoding.GetString(encodeTheseChars);

                return recoded_string;
            }

        }
    }
}
