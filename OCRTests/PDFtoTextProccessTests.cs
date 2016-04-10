using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using OCR;
using System.Collections.Generic;
using Newtonsoft.Json;

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
            OCR.OcrTextReader reader = new OCR.OcrTextReader();
            OcrData data = reader.Read("format2.tiff");

            ContributionPeriodsParser parser = new ContributionPeriodsParser();
            List<ContributionPeriod> results = parser.Parse(data.Text);

            if (results.Count == 0)
                return;

            APIClient client = new APIClient("http://localhost:58869/");

            var filestream = new System.IO.FileStream("format2.tiff",
                                          System.IO.FileMode.Open,
                                          System.IO.FileAccess.Read,
                                          System.IO.FileShare.ReadWrite);
            var fileStreamReader = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);



            client.PostStream("Image", fileStreamReader.BaseStream);



            string id = HealthCareContributionIdParser.Parse(data.Text);

            ContributionPeriodDataDTO dataToSend = new ContributionPeriodDataDTO();
            dataToSend.ContributorId = id;
            foreach (ContributionPeriod period in results)
            {
                dataToSend.ContributionPeriodsDTO.Add(new ContributionPeriodDTO() { MoneyContribution = period.MoneyContribution, PeriodEnd = period.PeriodEnd, PeriodStart = period.PeriodStart });
            }

            string jsonDataToSend = JsonConvert.SerializeObject(dataToSend);

            Assert.IsTrue(results.Count == 5);
        }
    }
}
