using Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatcherCmd.Files.Interface;

namespace WatcherCmd.Files 
{

    public class Watcher : IWatcher
    {

        public event FileSystemEventHandler FileDetected;
        private static FileSystemWatcher _watcher;
        
        
        private readonly ILog _logger;

        public Watcher(ILog logger)
        {
            _logger = logger;
        }

        public bool EnableRaisingEvents
        {
            get { return _watcher.EnableRaisingEvents; }
            set { _watcher.EnableRaisingEvents = value; }
        }

        public void Init()
        {
            _logger.Info("Initialize Watcher");
            prepareWatcherInExceptionSafeMode();
        }

        private void prepareWatcherInExceptionSafeMode()
        {
            try
            {
                _logger.Debug("Preparing FileSystemWatcher");
                prepareWatcher();
                _logger.Debug("Done preparing FileSystemWatcher");
            }
            catch (Exception e)
            {
                _logger.Error("Exception while preparing FileSystemWatcher", e);
                //disposeWatcher();
            }
        }

        private void prepareWatcher()
        {
            string directoryPathToWatch = @"C:\Users\Jorge\Documents\Visual Studio 2015\Projects\OCR-TPC-master\OCRTests\Destination";


            if (!Directory.Exists(directoryPathToWatch))
            {
                string error = string.Format("Directory '{0}' not found", directoryPathToWatch);

                _logger.Fatal(error);
                return;
            }
            _logger.Info(string.Format("Directory '{0}' found", directoryPathToWatch));


            _watcher = new FileSystemWatcher();

            _watcher.Path = directoryPathToWatch;
            _watcher.IncludeSubdirectories = true;
            _watcher.InternalBufferSize = 4 * 1024;
            //_watcher.Filter = ".csv";
            _watcher.Created += this.FileDetected;
            //_watcher.Renamed += this.?;
            _watcher.Error += watcherError;
            _watcher.IncludeSubdirectories = true;

            _watcher.EnableRaisingEvents = true;

            _logger.Info(String.Format("Watcher configured for folder {0}", _watcher.Path));
        }

        private void watcherError(object sender, ErrorEventArgs e)
        {
            _logger.Warn(String.Format("Error in FileSystemWatcher. Reinitializing watcher. Error {0} {1}", e.GetException().Message, e.ToString()));
            //prepareWatcherInExceptionSafeMode();
        }

    }
}
