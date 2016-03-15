using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using OCR;

namespace OCRTests
{
    [TestClass]
    public class PDFToImageConverterTests
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void PDFToImageConverter_Read_Error_FileNotFound()
        {
            PDFToImageConverter.ConvertToImage("lol.pdf", "outputlol.png");
        }

        [TestMethod]
        [DeploymentItem(@"C:\programacion\PecanSoft\OCR-TPC\OCRTests\Resources\Cert.pdf")]
        public void PDFToImageConverter_Read_Success()
        {
            PDFToImageConverter.ConvertToImage("Cert.pdf", "outputlol.png");
            Assert.IsTrue(File.Exists("Cert.pdf"));
        }
    }
}
