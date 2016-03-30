using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace OCR_API.InternalService
{
    public class Encryption
    {

        public string Encrypt(string plainText)
        {
            //string plainText = "hola";

            if (plainText == null) throw new ArgumentNullException("plainText");

            //encrypt data
            var data = Encoding.Unicode.GetBytes(plainText);
            byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);

            //return as base64 string
            string result = Convert.ToBase64String(encrypted);

            return result;
        }


        public string Decrypt(string cipher)
        {
            if (cipher == null) throw new ArgumentNullException("cipher");

            //parse base64 string
            byte[] data2 = Convert.FromBase64String(cipher);

            //decrypt data
            byte[] decrypted = ProtectedData.Unprotect(data2, null, DataProtectionScope.CurrentUser);

            return Encoding.Unicode.GetString(decrypted);
        }

    }

}