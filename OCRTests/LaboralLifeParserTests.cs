using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRTests
{
    [TestClass]
    public class LaboralLifeParserTests
    {

        [TestMethod]
        [TestCategory("UnitTest")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCRTests\Resources\vlaboral\20160428114735.pdf")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\x86", "x86")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\tessdata", "tessdata")]
        public void Parse_Success_EmptyText()
        {
            //PDFToImageConverter.ConvertToImage("20160428114735.pdf", "format2.png", 512,512,System.Drawing.Imaging.ImageFormat.Png);

            //PDFToImageConverter.ConvertToImage(, 512, 512, System.Drawing.Imaging.ImageFormat.Png);

            PDFToImageConverter.ConvertToImage(1, "20160428114735.pdf", "20160428114735-1.png", 512, 512, System.Drawing.Imaging.ImageFormat.Png);


            OCR.OcrTextReader reader = new OCR.OcrTextReader();
            var result = reader.Read("20160428114735-1.png");

            LaboralLifeParser parser = new LaboralLifeParser();
            parser.ParseTable(result.Text);

        }


}
}
