using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using System.Text;

namespace OCRTests
{
    [TestClass]
    public class Password_Test
    {

        [TestMethod]
        public void Encrypt()
        {
            string plainText = "hola";

            if (plainText == null) throw new ArgumentNullException("plainText");

            //encrypt data
            var data = Encoding.Unicode.GetBytes(plainText);
            byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);

            //return as base64 string
            string result = Convert.ToBase64String(encrypted);

         //   public string Decrypt(string cipher)
        //{
            if (result == null) throw new ArgumentNullException("cipher");

            //parse base64 string
            byte[] data2 = Convert.FromBase64String(result);

            //decrypt data
            byte[] decrypted = ProtectedData.Unprotect(data2, null, DataProtectionScope.CurrentUser);
            Assert.AreEqual(Encoding.Unicode.GetString(decrypted), plainText);
       // }

        }



    }
}
