using Common.Logging;
using Newtonsoft.Json;
using OCR;
using OCR_API.Controllers;
using OCR_API.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using WatcherCmd.Files.Interface;
using YFO.Testing.Infrastructure.APITesting;

namespace WatcherCmd.Files
{
    public class Manager : IManager
    {
        private readonly ILog _logger;
        private readonly IWatcher _watcher;
        
        public Manager(ILog logger, IWatcher watcher)
        {
            _logger = logger;
            _watcher = watcher;
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
            string outputFile = inputFile.Split('.')[0] + ".tiff";
            string outputPath = Path.Combine(Path.GetTempPath(), outputFile);

            PDFToImageConverter.ConvertToImage(inputPath, outputPath, 512, 512);

            OCR.OcrTextReader reader = new OCR.OcrTextReader();
            OcrData data = reader.Read(outputPath);
            
            string jsonData = ParseData(data);

            SendDataObject(jsonData);
        }

        private string ParseData(OcrData data)
        {
            ContributionPeriodsParser parser = new ContributionPeriodsParser();
            List<ContributionPeriod> results = parser.Parse(data.Text);

            string id = HealthCareContributionIdParser.Parse(data.Text);

            OCR_API.DTOs.ContributionPeriodDataDTO dataToSend = new OCR_API.DTOs.ContributionPeriodDataDTO();
            dataToSend.ContributorId = id;
            foreach (ContributionPeriod period in results)
            {
                dataToSend.ContributionPeriodsDTO.Add(
                    new OCR_API.DTOs.ContributionPeriodDTO()
                    {
                        MoneyContribution = period.MoneyContribution,
                        PeriodEnd = period.PeriodEnd,
                        PeriodStart = period.PeriodStart
                    }
                );
            }

            string jsonDataToSend = JsonConvert.SerializeObject(dataToSend);

            return jsonDataToSend;
        }

        private void SendDataObject (string jsonData)
        { 
            var appSettings = ConfigurationManager.AppSettings;
            string HostPath = appSettings["HostPath"];

            APIClient client = new APIClient(HostPath);

            client.Post(HostPath, jsonData);


        }

        

    }
}
