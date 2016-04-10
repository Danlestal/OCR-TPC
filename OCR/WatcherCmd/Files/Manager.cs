using Common.Logging;
using Newtonsoft.Json;
using OCR;
using System.Collections.Generic;
using System.IO;
using System.Net;
using WatcherCmd.Files.Interface;

namespace WatcherCmd.Files
{
    public class Manager : IManager
    {
        private readonly ILog _logger;
        private readonly IWatcher _watcher;
        private APIClient _apiClient;

        
        public Manager(ILog logger, IWatcher watcher, APIClient client)
        {
            _logger = logger;
            _watcher = watcher;
            _apiClient = client;

        }

        public void InitializeSystem()
        {

            _watcher.FileDetected += OnFileDetected;
            _watcher.Init();

        }

        private void OnFileDetected(object sender, FileSystemEventArgs e)
        {

            string inputPath = e.FullPath;
            string inputFile = Path.GetFileName(inputPath);
            string outputFile = Path.GetFileNameWithoutExtension(inputPath) + ".tiff";
            string outputPath = Path.Combine(Path.GetTempPath(), outputFile);

            PDFToImageConverter.ConvertToImage(inputPath, outputPath, 512, 512);
            OCR.OcrTextReader reader = new OCR.OcrTextReader();
            OcrData data = reader.Read(outputPath);

            ContributionPeriodsParser parser = new ContributionPeriodsParser();
            List<ContributionPeriod> results = parser.Parse(data.Text);

            if (results.Count == 0)
                return;

            APIClient client = new APIClient("http://localhost:58869/");

            var filestream = new System.IO.FileStream(outputPath,
                                          System.IO.FileMode.Open,
                                          System.IO.FileAccess.Read,
                                          System.IO.FileShare.ReadWrite);
            var fileStreamReader = new System.IO.StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);



            APIResponse imagePostingResponse =  client.PostStream("Image", fileStreamReader.BaseStream);
            //----------------

            /*
            WebRequest request = WebRequest.Create("http://localhost:58869/image");
            // Set the Method property of the request to POST.
            request.Method = "POST";
            byte[] byteArray = File.ReadAllBytes(outputPath);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            string responseFromServer = new StreamReader(dataStream).ReadToEnd();^*/


            //-----------------




            string id = HealthCareContributionIdParser.Parse(data.Text);

            ContributionPeriodDataDTO dataToSend = new ContributionPeriodDataDTO();
            dataToSend.ContributorId = id;
            foreach (ContributionPeriod period in results)
            {
                dataToSend.ContributionPeriodsDTO.Add(new ContributionPeriodDTO() { MoneyContribution = period.MoneyContribution, PeriodEnd = period.PeriodEnd, PeriodStart = period.PeriodStart });
            }

            string jsonDataToSend = JsonConvert.SerializeObject(dataToSend);

        }

        

    }
}
