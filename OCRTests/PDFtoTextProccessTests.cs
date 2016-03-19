using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using OCR;

namespace OCRTests
{
    [TestClass]
    public class PDFtoTextProccessTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCRTests\Resources\Cert.pdf")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\x86", "x86")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\tessdata", "tessdata")]
        public void WholeProc_Success()
        {
            PDFToImageConverter.ConvertToImage("Cert.pdf", "format2.tiff", 512, 512);
            OCR.OCRTextReader reader = new OCR.OCRTextReader();
            OcrData data = reader.Read("format2.tiff");
        }
    }
}
