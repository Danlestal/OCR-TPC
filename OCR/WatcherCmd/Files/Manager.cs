﻿using Common.Logging;
using Newtonsoft.Json;
using OCR;
using System.Collections.Generic;
using System.IO;
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
            string outputFile = inputFile.Split('.')[0] + ".tiff";
            string outputPath = Path.Combine(Path.GetTempPath(), outputFile);

            PDFToImageConverter.ConvertToImage(inputPath, outputPath, 512, 512);

            OCR.OcrTextReader reader = new OCR.OcrTextReader();
            OcrData data = reader.Read(outputPath);

            ContributionPeriodsParser parser = new ContributionPeriodsParser();
            List<ContributionPeriod> results = parser.Parse(data.Text);

            string id = HealthCareContributionIdParser.Parse(data.Text);

            OCR_API.DTOs.ContributionPeriodDataDTO dataToSend = new OCR_API.DTOs.ContributionPeriodDataDTO();
            dataToSend.ContributorId = id;
            dataToSend.ContributionPeriodsDTO = new List<OCR_API.DTOs.ContributionPeriodDTO>();

            foreach (ContributionPeriod period in results)
            {
                dataToSend.ContributionPeriodsDTO.Add(new OCR_API.DTOs.ContributionPeriodDTO() { MoneyContribution = period.MoneyContribution, PeriodEnd = period.PeriodEnd, PeriodStart = period.PeriodStart });
            }

            _apiClient.Post("ContributionPeriod", dataToSend);

        }

        

    }
}
