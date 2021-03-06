﻿using OCR;
using System;
using System.Configuration;
using System.IO;
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

            if (!Directory.Exists(ConfigurationManager.AppSettings["VidaLaboralFolder"]))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["VidaLaboralFolder"]);
            }

            if (!Directory.Exists(ConfigurationManager.AppSettings["VidaLaboralFolder"] + "\\ERROR\\"))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["VidaLaboralFolder"] + "\\ERROR\\");
            }

            if (!Directory.Exists(ConfigurationManager.AppSettings["DestinationPathVidaLaboral"]))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["DestinationPathVidaLaboral"]);
            }

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
                    //_logger.Log("archivo " + inputPath + "tiene "+ data.Rows.Count + " filas");
                    _apiClient.Post("LaboralLife", data);
                }
            }
            catch (Exception a)
            {
                _logger.Log("error en archivo: " + inputPath +"\n Excepcion: " + a.Message);
                string errorPath = ConfigurationManager.AppSettings["VidaLaboralFolder"] + "\\ERROR\\" + System.DateTime.Today.ToString("ddMMyyyy") + "_" + Path.GetFileName(inputPath);

                if (File.Exists(errorPath))
                {
                    File.Delete(errorPath);
                }

                File.Move(inputPath, errorPath);
            }

            string milisecond = DateTime.Now.Millisecond.ToString();
            string destinationPath = ConfigurationManager.AppSettings["DestinationPathVidaLaboral"] + "\\" + System.DateTime.Today.ToString("ddMMyyyy") + "_" + Path.GetFileName(inputPath);
            if (File.Exists(inputPath))
            {
                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                }

                File.Move(inputPath, destinationPath);
            }
            
            _logger.Log("FIN PROCESO");
        }
    }
}
