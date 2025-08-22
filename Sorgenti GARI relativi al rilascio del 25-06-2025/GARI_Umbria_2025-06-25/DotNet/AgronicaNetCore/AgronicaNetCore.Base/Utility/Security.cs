using AgronicaDataProvider6.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AgronicaNetCore.Base.Utility
{
    public static class Security
    {
        private const string ENCRYPTION_KEY = "13422245-16FA-4712-8BEF-26A32DC979BB";

        private static string Agronica_Url_Decode(string sInput, string sSeparator = "G")
        {
            var sOutput = "";
            var v = sInput.Split(sSeparator);
            foreach (var row in v)
            {

                var chr = Convert.ToChar(Convert.ToUInt32(row, 16)).ToString();
                sOutput += chr;
            }

            return sOutput;
        }

        private static bool UsaEncodingSemplice(IOptions<SecuritySettings> options)
        {
            if (options is null || options.Value is null)
                return false;

            return options.Value.UsaEncodingSemplice;
        }

        public static string Stringa_Decodifica_LANCompatibile(string sText,
                                                               string key,
                                                               IOptions<SecuritySettings> options,
                                                               bool Esci_Se_Vuoto = false)
        {
            if (Esci_Se_Vuoto && sText == "")
                return "";

            if (UsaEncodingSemplice(options))
            {
                var testoDecodificato = System.Web.HttpUtility.HtmlDecode(sText);
                return testoDecodificato.Replace("@", "\\");
            }

            var t = Agronica_Url_Decode(sText);
            var strEncrypted = "";
            byte pos = 0;

            var defEncoder = Encoding.Default;
            var _1252Encoder = CodePagesEncodingProvider.Instance.GetEncoding(1252);

            var tn = _1252Encoder!.GetString(Encoding.Convert(defEncoder, _1252Encoder, defEncoder.GetBytes(t)));
            var kC = _1252Encoder.GetString(Encoding.Convert(defEncoder, _1252Encoder, defEncoder.GetBytes(key)));

            for (var i = 0; i < tn.Length; i++)
            {

                var A1 = (int)(Convert.ToChar(_1252Encoder.GetBytes(tn.Substring(i, 1))[0]));
                var A2 = (int)(Convert.ToChar(_1252Encoder.GetBytes(kC.Substring(pos, 1))[0]));
                strEncrypted += Convert.ToChar(A1 - A2);
                pos++;
                if (pos >= key.Length) pos = 0;
            }
            return strEncrypted;
        }

        public static string DecryptString(string? encryptedText, string? encryptKey)
        {
            if (!string.IsNullOrEmpty(encryptedText) && !string.IsNullOrEmpty(encryptKey)) {
                try
                {
                    return DecryptString(encryptedText, ENCRYPTION_KEY, encryptKey);
                }
                catch (Exception) { }
            }
            return encryptedText;
        }

        public static string DecryptString(string encryptedText, string cr, string cr2)
        {
            if (string.IsNullOrEmpty(encryptedText))
                throw new ArgumentNullException("encryptedText");

            if (string.IsNullOrEmpty(cr))
                throw new ArgumentNullException("cr");

            if (string.IsNullOrEmpty(cr2))
                throw new ArgumentNullException("cr2");

            byte[] key;
            byte[] iv;

            var keyStr = cr + cr2;
            key = CalculateSHA256(keyStr);
            var iv_32 = CalculateSHA256(cr);
            iv = iv_32.Take(16).ToArray();

            string decrypted = DecryptStringWithAes(encryptedText, key, iv);
            return decrypted;
        }

        private static string DecryptStringWithAes(string cipherText, byte[] key, byte[] iv)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");

            if (Information.IsNothing(key) || key.Length <= 0)
                throw new ArgumentNullException("key");

            if (Information.IsNothing(iv) || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            var cipherByteArray = Convert.FromBase64String(cipherText);
            string plaintext = null;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                // Create a decryptor
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                // Create a memory stream with the encrypted data
                using (MemoryStream msDecrypt = new MemoryStream(cipherByteArray))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted data
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        public static string EncryptString(string plainText, string cr2)
        {
            return EncryptString(plainText, ENCRYPTION_KEY, cr2);
        }

        public static string EncryptString(string plainText, string cr, string cr2)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");

            if (string.IsNullOrEmpty(cr))
                throw new ArgumentNullException("cr");

            if (string.IsNullOrEmpty(cr2))
                throw new ArgumentNullException("cr2");

            byte[] key;
            byte[] iv;

            var keyStr = cr + cr2;
            key = CalculateSHA256(keyStr);
            var iv_32 = CalculateSHA256(cr);
            iv = iv_32.Take(16).ToArray();

            string encrypted = EncryptStringWithAes(plainText, key, iv);
            return encrypted;
        }

        private static string EncryptStringWithAes(string plainText, byte[] key, byte[] iv)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");

            if (Information.IsNothing(key) || key.Length <= 0)
                throw new ArgumentNullException("key");

            if (Information.IsNothing(iv) || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                // Create an encryptor
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Create a memory stream to hold the encrypted data
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write the data to be encrypted into the stream
                            swEncrypt.Write(plainText);
                        }
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public static byte[] CalculateSHA256(string str)
        {
            SHA256 sha256 = SHA256.Create();
            System.Text.UTF8Encoding objUtf8 = new System.Text.UTF8Encoding();
            byte[] hashValue = sha256.ComputeHash(objUtf8.GetBytes(str ?? ""));

            return hashValue;
        }

    }
}
