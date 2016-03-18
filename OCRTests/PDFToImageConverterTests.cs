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
            PDFToImageConverter.ConvertToImage("lol.pdf", "outputlol.png",72,72);
        }

        [TestMethod]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCRTests\Resources\Cert.pdf")]
        public void PDFToImageConverter_Read_Success()
        {
            PDFToImageConverter.ConvertToImage("Cert.pdf", "format2.tiff",72,72);
            Assert.IsTrue(File.Exists("format2.tiff"));
        }
    }
}
