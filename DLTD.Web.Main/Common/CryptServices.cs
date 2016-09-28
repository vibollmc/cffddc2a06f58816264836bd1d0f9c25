using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace DLTD.Web.Main.Common
{
    public class CryptServices
    {
        #region Password
        public static string HashMD5(String str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            // Convert the input string to a byte array and compute the hash.
            byte[] b = System.Text.Encoding.UTF8.GetBytes(str);
            b = md5.ComputeHash(b);
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder s = new StringBuilder();
            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            foreach (byte by in b)
            {
                s.Append(by.ToString("x2").ToLower());
            }
            // Return the hexadecimal string.
            return s.ToString();
        }

        // Verify a hash against a string.
        public static bool verifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = HashMD5(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string HashSHA1(string value)
        {
            //var sha1 = System.Security.Cryptography.SHA1.Create();
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            var inputBytes = Encoding.UTF8.GetBytes(value);
            //var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2").ToLower());
            }
            return sb.ToString();
        }

        #endregion Password
    }
}