using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

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
        public void ParseTable_Success()
        {
            PDFToImageConverter converter = new PDFToImageConverter();
            converter.AddFilter(new BrightnessFilter(0.35f));
            converter.ConvertToImage(1, "20160428114735.pdf", "20160428114735-1.png", 512, 512, System.Drawing.Imaging.ImageFormat.Png);

            OCR.OcrTextReader reader = new OCR.OcrTextReader();
            var tablesPageData = reader.Read("20160428114735-1.png");

            LaboralLifeParser parser = new LaboralLifeParser();
            parser.ParseTable(tablesPageData.Text);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCRTests\Resources\vlaboral\20160428114735.pdf")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\x86", "x86")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\tessdata", "tessdata")]
        public void Parse_Success()
        {

            LaboralLifeParser parser = new LaboralLifeParser();
            LaboralLifeData result = parser.Parse("20160428114735.pdf");

        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCRTests\Resources\vlaboral", "vlaboral")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\x86", "x86")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\tessdata", "tessdata")]
        public void Parse_All_Success()
        {
            List<LaboralLifeData> allLifes = new List<LaboralLifeData>();
            foreach (string file in Directory.GetFiles("vlaboral"))
            {
                LaboralLifeParser parser = new LaboralLifeParser();
                allLifes.Add(parser.Parse(file));
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCRTests\Resources\vlaboral\20160428114735.pdf")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\x86", "x86")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\tessdata", "tessdata")]
        public void Parse_JustOneFile_Success()
        {

            LaboralLifeParser parser = new LaboralLifeParser();
            LaboralLifeData data = parser.Parse("20160428114735.pdf");

            string jsonDataToSend = JsonConvert.SerializeObject(data);

            APIClient client = new APIClient("http://localhost:58869/");
            client.Post("LaboralLife", jsonDataToSend);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCRTests\Resources\vlaboral\20160428114735.pdf")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\x86", "x86")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\tessdata", "tessdata")]
        public void Parse_PersonalData_Success()
        {

            PDFToImageConverter converter = new PDFToImageConverter();
            converter.ConvertToImage(0, "20160428114735.pdf", "20160428114735-0.png", 512, 512, System.Drawing.Imaging.ImageFormat.Png);

            OCR.OcrTextReader reader = new OCR.OcrTextReader();
            var personalData = reader.Read("20160428114735-0.png");

            LaboralLifeParser parser = new LaboralLifeParser();
            parser.ParsePersonalData(personalData.Text);
        }


        [TestMethod]
        [TestCategory("UnitTest")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCRTests\Resources\vlaboral","vlaboral")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\x86", "x86")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCR\tessdata", "tessdata")]
        public void Parse_AllPersonalData_Success()
        {
            PDFToImageConverter converter = new PDFToImageConverter();
            OCR.OcrTextReader reader = new OCR.OcrTextReader();
            LaboralLifeParser parser = new LaboralLifeParser();

            List<PersonalData> personalDataList = new List<PersonalData>();
            foreach (string file in Directory.GetFiles("vlaboral"))
            {
                string outputFileName = Path.GetFileNameWithoutExtension(file) + ".png";

                converter.ConvertToImage(0, file, outputFileName, 512, 512, System.Drawing.Imaging.ImageFormat.Png);
                var personalOCRData = reader.Read(outputFileName);

                PersonalData data = null;
                try
                {
                    data = parser.ParsePersonalData(personalOCRData.Text);
                }
                catch (Exception)
                {
                    Console.Write("Problems parsing file " + file);
                }
                
                if (data != null)
                    personalDataList.Add(data);
            }

        }


    }
}
