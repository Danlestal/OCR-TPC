using Common.Logging;
using Newtonsoft.Json;
using OCR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel;
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
            string fileOutputPath = Path.Combine(Path.GetTempPath(), outputFile);

            PDFToImageConverter.ConvertToImage(inputPath, fileOutputPath, 512, 512);
            OCR.OcrTextReader reader = new OCR.OcrTextReader();
            OcrData data = reader.Read(fileOutputPath);

            ContributionPeriodsParser parser = new ContributionPeriodsParser();
            List<ContributionPeriod> results = parser.Parse(data.Text);

            if (results.Count == 0)
                return;


            string fileId = UploadFile(fileOutputPath);
            string contributorId = HealthCareContributionIdParser.Parse(data.Text);

            ContributionPeriodDataDTO dataToSend = new ContributionPeriodDataDTO();
            dataToSend.ContributorId = contributorId;
            foreach (ContributionPeriod period in results)
            {
                dataToSend.ContributionPeriodsDTO.Add(new ContributionPeriodDTO() { MoneyContribution = period.MoneyContribution,
                                                                                    PeriodEnd = period.PeriodEnd,
                                                                                    PeriodStart = period.PeriodStart,
                                                                                    HighResFileId = fileId
                });
            }

           string jsonDataToSend = JsonConvert.SerializeObject(dataToSend);

            APIClient client = new APIClient("http://localhost:58869/");
            client.Post("ContributionPeriod", dataToSend);

        }

        private static string UploadFile(string outputPath)
        {
            StreamReader sr = new StreamReader(outputPath, System.Text.Encoding.UTF8, true, 128);
            byte[] fileStream = ReadFully(sr.BaseStream);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri("http://localhost:58869/UploadFile"));
            request.Method = "POST";
            request.ContentType = "application/octet-stream";

            Stream serverStream = request.GetRequestStream();
            serverStream.Write(fileStream, 0, fileStream.Length);
            serverStream.Close();

            WebResponse response =  request.GetResponse();
            string result = string.Empty;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd(); // do something fun...
            }

            return result;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                if (input != null)
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                }
                return ms.ToArray();
            }
        }



    }
}
