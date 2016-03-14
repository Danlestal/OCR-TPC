using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using OCR;

namespace OCRTests
{
    [TestClass]
    public class PDFReaderTests
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void PDFReader_Read_Error_FileNotFound()
        {
            var reader = new PDFReader();
            reader.Read("lol.pdf");
        }
    }
}
