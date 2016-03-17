using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OCRTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class OCRTextReaderTests
    {
        [TestMethod]
        [DeploymentItem(@"C:\programacion\PecanSoft\OCR-TPC\OCRTests\Resources\Cert.png")]
        public void Read_Success()
        {
            OCR.OCRTextReader reader = new OCR.OCRTextReader();
            reader.Read("Cert.png");
        }
    }
}
