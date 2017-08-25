using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Common.Utilities
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
                //byte[] utf8Bytes = Encoding.UTF8.GetBytes(utf8String);
                //byte[] isoBytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utf8Bytes);

                //string uf8converted = Encoding.Unicode.GetString(isoBytes);
                //======================================================

                String raw_db_string = utf8String;
                // encoding : 1252 , 1258
                // Windows-1252 (Western Europe)
                // Windows-1258 (Vietnam)
                Encoding sourceEncoding = Encoding.GetEncoding(1252);
                Encoding targetEncoding = Encoding.GetEncoding(65001);

                byte[] encodeTheseChars = sourceEncoding.GetBytes(raw_db_string);

                String recoded_string = targetEncoding.GetString(encodeTheseChars);

                return recoded_string;
                //===============================================
                //return uf8converted;
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

        //==========================================
        public static string AsciiToUnicode(string asciiString)
        {
            if (String.IsNullOrEmpty(asciiString))
            {
                return "";
            }
            else
            {
                byte[] utf8Bytes = Encoding.ASCII.GetBytes(asciiString);
                byte[] isoBytes = Encoding.Convert(Encoding.ASCII, Encoding.Unicode, utf8Bytes);

                string uf8converted = Encoding.Unicode.GetString(isoBytes);
                //======================================================

                String raw_db_string = asciiString;
                // encoding : 1252 , 1258
                // Windows-1252 (Western Europe)
                // Windows-1258 (Vietnam)
                Encoding sourceEncoding = Encoding.GetEncoding(1252);
                Encoding targetEncoding = Encoding.GetEncoding(65001);

                byte[] encodeTheseChars = sourceEncoding.GetBytes(raw_db_string);

                String recoded_string = targetEncoding.GetString(encodeTheseChars);

                // return recoded_string;
                //===============================================
                return uf8converted;
            }
        }

        //Public Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" ( _
        //lpvDest As Any, lpvSource As Any, ByVal cbCopy As Long)

        // Public Function UnicodeToUTF8(UniString As String) As String
        //Dim bArray() As Byte, TempB() As Byte, i As Long, k As Long
        //Dim TLen As Long, b1 As Byte, b2 As Byte, UTF16 As Long
        //Dim byte1 As Byte, byte2 As Byte, byte3 As Byte

        //TLen = Len(UniString)
        //If TLen = 0 Then Exit Function
        //k = 0

        //For i = 1 To TLen
        //CopyMemory b1, ByVal StrPtr(UniString) + ((i - 1) * 2), 1
        //CopyMemory b2, ByVal StrPtr(UniString) + ((i - 1) * 2) + 1, 1

        //UTF16 = b2
        //UTF16 = UTF16 * 256 + b1

        //If UTF16 < &H80 Then
        //UnicodeToUTF8 = UnicodeToUTF8 & Chr$(UTF16)
        //ElseIf UTF16 < &H800 Then
        //byte2 = &H80 + (UTF16 And &H3F)
        //UTF16 = UTF16 \ &H40
        //byte1 = &HC0 + (UTF16 And &H1F)
        //UnicodeToUTF8 = UnicodeToUTF8 & Chr$(byte1) & Chr$(byte2)
        //Else
        //byte3 = &H80 + (UTF16 And &H3F)
        //UTF16 = UTF16 \ &H40
        //byte2 = &H80 + (UTF16 And &H3F)
        //UTF16 = UTF16 \ &H40
        //byte1 = &HE0 + (UTF16 And &HF)
        //UnicodeToUTF8 = UnicodeToUTF8 & Chr$(byte1) & Chr$(byte2) & Chr$(byte3)
        //End If
        //Next
        //End Function

        //Public Function UTF8ToUnicode(UTF8Str As String) As String
        //Dim indexS As Long, UTF8 As Long, U16 As Long, BArrayS() As Byte, lastbyte As Byte
        //BArrayS() = UTF8Str
        //indexS = LBound(BArrayS)
        //On Error Resume Next
        //While indexS <= UBound(BArrayS)
        //UTF8 = BArrayS(indexS)
        //If UTF8 = &HE1 Or UTF8 = &HE2 Then ' 3 bytes
        //lastbyte = Merge(BArrayS(indexS + 4), BArrayS(indexS + 5))
        //U16 = (BArrayS(indexS) And &HF) * &H1000 + (BArrayS(indexS + 2) And &H3F) * &H40 + (lastbyte And &H3F)

        //UTF8ToUnicode = UTF8ToUnicode & ChrW(U16)

        //indexS = indexS + 6
        //ElseIf ((UTF8 >= &HC3) And (UTF8 <= &HC6)) Or UTF8 = &HCB Then '2 bytes
        //lastbyte = Merge(BArrayS(indexS + 2), BArrayS(indexS + 3))
        //U16 = (BArrayS(indexS) And &H1F) * &H40 + (lastbyte And &H3F)

        //UTF8ToUnicode = UTF8ToUnicode & ChrW(U16)

        //indexS = indexS + 4
        //Else ' 1 byte
        //UTF8ToUnicode = UTF8ToUnicode & Chr(UTF8)
        //indexS = indexS + 2
        //End If
        //Wend
        //End Function

        //Private Function Merge(ByVal LoByte As Byte, ByVal HiByte As Byte) As Byte
        //If HiByte = 0 Then
        //Merge = LoByte
        //Exit Function
        //End If

        //Select Case HiByte
        //Case 32
        //Select Case LoByte
        //Case 26: Merge = &H82
        //Case 30: Merge = &H84
        //Case 38: Merge = &H85
        //Case 32: Merge = &H86
        //Case 33: Merge = &H87
        //Case 48: Merge = &H89
        //Case 57: Merge = &H8B
        //Case 24: Merge = &H91
        //Case 25: Merge = &H92
        //Case 28: Merge = &H93
        //Case 29: Merge = &H94
        //Case 34: Merge = &H95
        //Case 19: Merge = &H96
        //Case 20: Merge = &H97
        //Case 58: Merge = &H9B
        //End Select
        //Case 33
        //Select Case LoByte
        //Case 34: Merge = &H99
        //End Select
        //Case 1
        //Select Case LoByte
        //Case 146: Merge = &H83
        //Case 96: Merge = &H8A
        //Case 82: Merge = &H8C
        //Case 125: Merge = &H8E
        //Case 97: Merge = &H9A
        //Case 83: Merge = &H9C
        //Case 126: Merge = &H9E
        //Case 120: Merge = &H9F
        //End Select
        //Case 2
        //Select Case LoByte
        //Case 198: Merge = &H88
        //Case 220: Merge = &H98
        //End Select
        //End Select
        //End Function

        //Link: http://www.ddth.com/showthread.php/214703-Chuyển-qua-lại-giữa-UTF-8-literal-và-Unicode#ixzz2HRPoRq20
    }
}
