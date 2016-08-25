
using Newtonsoft.Json;
using OCR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using WatcherCmd.Files.Interface;

namespace WatcherCmd.Files
{
    public class VidaLaboralManager : IManager
    {
        private readonly IWatcher _watcher;
        private ILogger _logger;
        private APIClient _apiClient;
        private string _apiUrl;


        public VidaLaboralManager(ILogger logger, IWatcher watcher, APIClient client)
        {
            _logger = logger;
            _watcher = watcher;
            _apiClient = client;
            _apiUrl = ConfigurationManager.AppSettings["ApiURL"];

        }

        public void InitializeSystem()
        {

            _watcher.FileDetected += OnFileDetected;
            _watcher.Init(ConfigurationManager.AppSettings["VidaLaboralFolder"]);

        }

        private void OnFileDetected(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(500);
            ProcLaboralLife(e.FullPath);
        }

        private void ProcLaboralLife(string inputPath)
        {
            _logger.Log("PROCESANDO ARCHIVO: " + inputPath);
            LaboralLifeParser parser = new LaboralLifeParser();

            LaboralLifeData data = null;
            try
            {
                data = parser.Parse(inputPath);
                if (data != null)
                {
                    _apiClient.Post("LaboralLife", data);
                }
            }
            catch (Exception a)
            {
                _logger.Log("error en archivo: " + inputPath +"\n Excepcion: " + a.Message);
            }

            _logger.Log("FIN PROCESO");
            File.Delete(inputPath);
        }
    }
}
