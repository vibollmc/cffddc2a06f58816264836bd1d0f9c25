using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DBTek.Crypto;

//[assembly: System.Security.AllowPartiallyTrustedCallers]

namespace QLVB.Common.Crypt
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

        #region Ma hoa van ban dien tu

        // ma hoa van ban dien tu theo dinh dang cu
        // tuong thich voi phien ban asp

        private static string Space(int n)
        {
            string str = "";
            for (int i = 0; i < n; i++)
            {
                str += " ";
            }
            //return String.Empty.PadLeft(n);
            return str;
        }

        public static string Mahoa(string strtext)
        {
            int nC, lI, lJ = 0, nK = 0, lA;
            string sB;
            lA = strtext.Length;
            int temp = (lA + (lA + 2) / 3);
            sB = Space(temp);
            //string smod = "lA=" + lA + ";";
            for (lI = 1; lI <= lA; lI++)
            {
                //char t = strtext.Substring(lI, 1);  == mid
                // mang trong vbs tu 1 .. n
                // c# : 1 .. n-1
                char c = strtext[lI - 1];  //== Asc
                nC = (int)(c);
                lJ = lJ + 1;
                sB = sB.Substring(0, lJ - 1) + ((char)((nC & 63) + 59)).ToString() + Space(sB.Length - lJ);
                switch (lI % 3)
                {
                    case 1:
                        nK = (nK | (int)((nC / 64) * 16));
                        break;

                    case 2:
                        nK = (nK | (int)((nC / 64) * 4));
                        break;

                    case 0:
                        nK = (nK | (int)(nC / 64));
                        lJ = lJ + 1;
                        sB = sB.Substring(0, lJ - 1) + ((char)(nK + 59)).ToString() + Space(sB.Length - lJ);
                        nK = 0;
                        break;
                }
            }
            //smod += "lI=" + lI + ";";
            int t2 = lA % 3;
            if (t2 == 1)
            {
                lJ = lJ + 1;
                sB = sB.Substring(0, lJ - 1) + ((char)(nK + 59)).ToString() + Space(sB.Length - lJ);
            }
            return sB;
        }

        public static string Giaima(string strtext)
        {
            strtext = strtext.Replace("\r", "");
            strtext = strtext.Replace("\n", "");
            int nC, nD = 0, nE = 0, lA, lB, lI, lJ = 0, lK = 0;
            string sB;
            lA = strtext.Length;
            lB = lA - 1 - ((lA - 1) / 4);
            sB = Space(lB);
            //string smod = "lB=" + lB + ";";
            for (lI = 1; lI <= lB; lI++)
            {
                lJ = lJ + 1;
                char c = strtext[lJ - 1];  //== Asc
                nC = (int)c - 59;
                int mod = lI % 3;
                switch (mod)
                {
                    case 1:
                        lK = lK + 4;
                        if (lK > lA) { lK = lA; }
                        char e = strtext[lK - 1];
                        nE = (int)e - 59;
                        nD = ((nE / 16) & 3) * 64;
                        break;

                    case 2:
                        nD = ((nE / 4) & 3) * 64;
                        break;

                    case 0:
                        nD = (nE & 3) * 64;
                        lJ = lJ + 1;
                        break;
                }
                sB = sB.Substring(0, lI - 1) + (char)(nC | nD) + Space(sB.Length - lI);
            }
            //smod += "lI=" + lI + ";";
            return sB;
        }

        //    Function ToGiaiMa(msText)
        //    msText=replace(msText,Chr(13),"")
        //    msText=replace(msText,Chr(10),"")
        //    Dim nC
        //    Dim nD
        //    Dim nE
        //    Dim lA
        //    Dim lB
        //    Dim lI
        //    Dim lJ
        //    Dim lK
        //    Dim sB
        //    lA = Len(msText)
        //    lB = lA - 1 - (lA - 1) \ 4
        //    sB = Space(lB)
        //    For lI = 1 To lB
        //        lJ = lJ + 1
        //        nC = Asc(Mid(msText, lJ, 1)) - 59
        //        Select Case lI Mod 3
        //        Case 1
        //            lK = lK + 4
        //            If lK > lA Then lK = lA end if
        //            nE = Asc(Mid(msText, lK, 1)) - 59
        //            nD = ((nE \ 16) And 3) * 64
        //        Case 2
        //            nD = ((nE \ 4) And 3) * 64
        //        Case 0
        //            nD = (nE And 3) * 64
        //            lJ = lJ + 1
        //        End Select
        //        sB = Mid(sB, 1, lI - 1) & Chr(nC Or nD) & Mid(sB, lI + 1, Len(sB) - lI)
        //    Next
        //    ToGiaiMa = sB
        //End Function
        //'---------------------------------------------------------------
        //Function ToMahoa(msText)
        //    Dim nC
        //    Dim lI
        //    Dim lJ
        //    Dim nK
        //    Dim lA
        //    Dim sB
        //    lA = Len(msText)
        //    sB = Space(lA + (lA + 2) \ 3)
        //    For lI = 1 To lA
        //        nC = Asc(Mid(msText, lI, 1))
        //        lJ = lJ + 1
        //        sB = Mid(sB, 1, lJ - 1) & Chr((nC And 63) + 59) & Mid(sB, lJ + 1, Len(sB) - lJ)
        //        Select Case lI Mod 3
        //        Case 1
        //            nK = nK Or ((nC \ 64) * 16)
        //        Case 2
        //            nK = nK Or ((nC \ 64) * 4)
        //        Case 0
        //            nK = nK Or (nC \ 64)
        //            lJ = lJ + 1
        //            sB = Mid(sB, 1, lJ - 1) & Chr(nK + 59) & Mid(sB, lJ + 1, Len(sB) - lJ)
        //            nK = 0
        //        End Select
        //    Next
        //    If lA Mod 3 Then
        //        lJ = lJ + 1
        //        sB = Mid(sB, 1, lJ - 1) & Chr(nK + 59) & Mid(sB, lJ + 1, Len(sB) - lJ)
        //    End If
        //    ToMahoa = sB
        //End Function

        #endregion Ma hoa van ban dien tu


        #region Base64

        /// <summary>
        /// The method create a Base64 encoded string from a normal string.
        /// </summary>
        /// <param name="toEncode">The String containing the characters to encode.</param>
        /// <returns>The Base64 encoded string.</returns>
        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.Unicode.GetBytes(toEncode);

            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }

        /// <summary>
        /// The method to Decode your Base64 strings.
        /// </summary>
        /// <param name="encodedData">The String containing the characters to decode.</param>
        /// <returns>A String containing the results of decoding the specified sequence of bytes.</returns>
        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);

            string returnValue = System.Text.Encoding.Unicode.GetString(encodedDataAsBytes);

            return returnValue;
        }


        public void EncodeWithString(string inputFileName)
        {
            System.IO.FileStream inFile;
            byte[] binaryData;
            try
            {
                inFile = new System.IO.FileStream(inputFileName,
                                          System.IO.FileMode.Open,
                                          System.IO.FileAccess.Read);
                binaryData = new Byte[inFile.Length];
                long bytesRead = inFile.Read(binaryData, 0,
                                     (int)inFile.Length);
                inFile.Close();
            }
            catch (Exception ex)
            {
                // Error creating stream or reading from it.
                //System.Console.WriteLine("{0}", exp.Message);
                string error = ex.Message;
                return;
            }

            // Convert the binary input into Base64 UUEncoded output.
            string base64String;
            try
            {
                base64String =
                  System.Convert.ToBase64String(binaryData,
                                         0,
                                         binaryData.Length);
            }
            catch (System.ArgumentNullException)
            {
                System.Console.WriteLine("Binary data array is null.");
                return;
            }

            // Write the UUEncoded version to the output file.
            //System.IO.StreamWriter outFile;
            //try
            //{
            //    outFile = new System.IO.StreamWriter(outputFileName,
            //                         false,
            //                         System.Text.Encoding.ASCII);
            //    outFile.Write(base64String);
            //    outFile.Close();
            //}
            //catch (System.Exception exp)
            //{
            //    // Error creating stream or writing to it.
            //    System.Console.WriteLine("{0}", exp.Message);
            //}
        }

        public static string EncodeFileToBase64(string fileName)
        {
            FileStream fs = new FileStream(fileName,
                                   FileMode.Open,
                                   FileAccess.Read);
            byte[] filebytes = new byte[fs.Length];
            fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
            string encodedData =
                Convert.ToBase64String(filebytes);
            //Base64FormattingOptions.InsertLineBreaks);
            return encodedData;
        }

        public static void DecodeFileFromBase64(string encodeData, string fileName)
        {

            byte[] filebytes = Convert.FromBase64String(encodeData);
            FileStream fs = new FileStream(fileName,
                                           FileMode.CreateNew,
                                           FileAccess.Write,
                                           FileShare.None);
            fs.Write(filebytes, 0, filebytes.Length);
            fs.Close();

            byte[] file = System.Convert.FromBase64String(encodeData);
            //File.WriteAllBytes(fileName + ".zip", file);
            File.WriteAllBytes(fileName, file);
        }

        //http://msdn.microsoft.com/en-us/library/system.security.cryptography.frombase64transform.aspx

        public static void DecodeFromFile(string inFileName, string outFileName)
        {
            using (FromBase64Transform myTransform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces))
            {

                byte[] myOutputBytes = new byte[myTransform.OutputBlockSize];

                //Open the input and output files. 
                using (FileStream myInputFile = new FileStream(inFileName, FileMode.Open, FileAccess.Read))
                {
                    using (FileStream myOutputFile = new FileStream(outFileName, FileMode.Create, FileAccess.Write))
                    {

                        //Retrieve the file contents into a byte array.  
                        byte[] myInputBytes = new byte[myInputFile.Length];
                        myInputFile.Read(myInputBytes, 0, myInputBytes.Length);

                        //Transform the data in chunks the size of InputBlockSize.  
                        int i = 0;
                        while (myInputBytes.Length - i > 4/*myTransform.InputBlockSize*/)
                        {
                            int bytesWritten = myTransform.TransformBlock(myInputBytes, i, 4/*myTransform.InputBlockSize*/, myOutputBytes, 0);
                            i += 4/*myTransform.InputBlockSize*/;
                            myOutputFile.Write(myOutputBytes, 0, bytesWritten);
                        }

                        //Transform the final block of data.
                        myOutputBytes = myTransform.TransformFinalBlock(myInputBytes, i, myInputBytes.Length - i);
                        myOutputFile.Write(myOutputBytes, 0, myOutputBytes.Length);

                        //Free up any used resources.
                        myTransform.Clear();
                    }
                }
            }

        }

        // Read in the specified source file and write out an encoded target file. 
        public static void EncodeFromFile(string sourceFile, string targetFile)
        {
            // Verify members.cs exists at the specified directory. 
            if (!File.Exists(sourceFile))
            {
                //Console.Write("Unable to locate source file located at ");
                //Console.WriteLine(sourceFile + ".");
                //Console.Write("Please correct the path and run the ");
                //Console.WriteLine("sample again.");
                return;
            }

            // Retrieve the input and output file streams. 
            using (FileStream inputFileStream =
                new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            {
                using (FileStream outputFileStream =
                    new FileStream(targetFile, FileMode.Create, FileAccess.Write))
                {

                    // Create a new ToBase64Transform object to convert to base 64.
                    ToBase64Transform base64Transform = new ToBase64Transform();

                    // Create a new byte array with the size of the output block size. 
                    byte[] outputBytes = new byte[base64Transform.OutputBlockSize];

                    // Retrieve the file contents into a byte array. 
                    byte[] inputBytes = new byte[inputFileStream.Length];
                    inputFileStream.Read(inputBytes, 0, inputBytes.Length);

                    // Verify that multiple blocks can not be transformed. 
                    if (!base64Transform.CanTransformMultipleBlocks)
                    {
                        // Initializie the offset size. 
                        int inputOffset = 0;

                        // Iterate through inputBytes transforming by blockSize. 
                        int inputBlockSize = base64Transform.InputBlockSize;

                        while (inputBytes.Length - inputOffset > inputBlockSize)
                        {
                            base64Transform.TransformBlock(
                                inputBytes,
                                inputOffset,
                                inputBytes.Length - inputOffset,
                                outputBytes,
                                0);

                            inputOffset += base64Transform.InputBlockSize;
                            outputFileStream.Write(
                                outputBytes,
                                0,
                                base64Transform.OutputBlockSize);
                        }

                        // Transform the final block of data.
                        outputBytes = base64Transform.TransformFinalBlock(
                            inputBytes,
                            inputOffset,
                            inputBytes.Length - inputOffset);

                        outputFileStream.Write(outputBytes, 0, outputBytes.Length);
                        //Console.WriteLine("Created encoded file at " + targetFile);
                    }

                    // Determine if the current transform can be reused. 
                    if (!base64Transform.CanReuseTransform)
                    {
                        // Free up any used resources.
                        base64Transform.Clear();
                    }
                }
            }

        }

        #endregion Base64

        #region AES

        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                //aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                //aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        public static string EncryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = EncryptStringToBytes_Aes(input, passwordBytes, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }
        public static string DecryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            //byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            string result = DecryptStringFromBytes_Aes(bytesToBeDecrypted, passwordBytes, passwordBytes);

            return result;
        }

        #endregion AES

        #region DBTek

        public static string EncryptString_AES(string pass, string text)
        {
            try
            {
                DBTek.Crypto.Rijndael crypt = new DBTek.Crypto.Rijndael();
                string encryptext = crypt.EncodeString(text, pass, pass);
                return encryptext;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static string DeCryptString_AES(string pass, string text)
        {
            try
            {
                DBTek.Crypto.Rijndael crypt = new DBTek.Crypto.Rijndael();
                string detext = crypt.DecodeString(text, pass, pass);
                return detext;
            }
            catch 
            {
                //throw ex;
                return "Error!";
            }

        }

        public static void EncodeFile_Base64(string sourceFile, string destFile)
        {
            try
            {
                DBTek.Crypto.Base64 crypt = new DBTek.Crypto.Base64();
                crypt.EncodeFile(sourceFile, destFile);
            }
            catch 
            {

            }

        }
        public static void DecodeFile_Base64(string sourceFile, string destFile)
        {
            try
            {
                DBTek.Crypto.Base64 crypt = new DBTek.Crypto.Base64();
                crypt.DecodeFile(sourceFile, destFile);
            }
            catch 
            {

            }

        }

        #endregion DBTek

        #region AES1

        //http://www.c-sharpcorner.com/UploadFile/a85b23/text-encrypt-and-decrypt-with-a-specified-key/

        private static byte[] _salt = Encoding.ASCII.GetBytes("uxyotp873@$%");

        public static string EncryptStringAES(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr = null;                 // Encrypted string to return  
            RijndaelManaged aesAlg = null;        // RijndaelManaged object used to encrypt the data.  

            try
            {
                // generate the key from the shared secret and the salt  
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create a RijndaelManaged object  
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.  
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.  
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV  
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt =
                       new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // Clear the RijndaelManaged object.  
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.  
            return outStr;
        }

        public static string DecryptStringAES(string cipherText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            // Declare the RijndaelManaged object  
            // used to decrypt the data.  
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt  
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create the streams used for decryption.  
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object  
                    // with the specified key and IV.  
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream  
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.  
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt =
                        new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream  
                            // and place them in a string.  
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                // Clear the RijndaelManaged object.  
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }
        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }


        #endregion AES1

    }
}
