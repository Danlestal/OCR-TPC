
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCR_API.InternalService;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRTests
{

    [TestClass]
    public class ImageUploadTest
    {

        [TestMethod]
        [TestCategory("UnitTest")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCRTests\Resources\Cert.tiff")]
        public void Upload_Success()
        {

            string moveFile = "Cert.tiff";
            string url = @"C:\Users\Jorge\Documents\programacion\Cert.tiff";
            if (File.Exists(url))
            {
                File.Delete(url);
            }

            var uploader = new ImageUploadService();
            uploader.UploadImage(moveFile, url);

            Assert.IsTrue(File.Exists(url));
        }

    }
}
