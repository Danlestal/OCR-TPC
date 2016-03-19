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
        [TestCategory("UnitTest")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCRTests\Resources\format2.tiff")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\x86", "x86")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\tessdata", "tessdata")]
        public void Read_Success()
        {
            OCR.OCRTextReader reader = new OCR.OCRTextReader();
            reader.Read("format2.tiff");
        }
    }
}
