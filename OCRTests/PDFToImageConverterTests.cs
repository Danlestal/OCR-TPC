using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using OCR;
using System.Drawing.Imaging;

namespace OCRTests
{
    [TestClass]
    public class PDFToImageConverterTests
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void PDFToImageConverter_Read_Error_FileNotFound()
        {
            PDFToImageConverter.ConvertToImage("lol.pdf", "outputlol.png",72,72, ImageFormat.Tiff);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCRTests\Resources\Cert.pdf")]
        public void PDFToImageConverter_Read_Success()
        {
            PDFToImageConverter.ConvertToImage("Cert.pdf", "format2.tiff",72,72, ImageFormat.Tiff);
            Assert.IsTrue(File.Exists("format2.tiff"));
        }
    }
}
