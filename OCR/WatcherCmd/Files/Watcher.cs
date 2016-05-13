using Common.Logging;
using Common.Logging.Configuration;
using System;
using System.Configuration;
using System.IO;
using WatcherCmd.Files.Interface;

namespace WatcherCmd.Files
{

    public class Watcher : IWatcher
    {

        public event FileSystemEventHandler FileDetected;
        private static FileSystemWatcher _watcher;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Watcher));

        public bool EnableRaisingEvents
        {
            get { return _watcher.EnableRaisingEvents; }
            set { _watcher.EnableRaisingEvents = value; }
        }

        public void Init(string folder)
        {
            PrepareWatcherInExceptionSafeMode(folder);
        }

        private void PrepareWatcherInExceptionSafeMode(string folderToWatch)
        {
            _logger.Debug("Preparing FileSystemWatcher");

            try
            {
                PrepareWatcher(folderToWatch);
            }
            catch (Exception e)
            {
                _logger.Error("Exception while preparing FileSystemWatcher", e);
                disposeWatcher();
            }
        }

        private void PrepareWatcher(string directoryPathToWatch)
        {

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
            _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            _watcher.Created += FileDetected; 
            //_watcher.Renamed += this.?;
            _watcher.Error += WatcherError;
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;

            _logger.Info(String.Format("Watcher configured for folder {0}", _watcher.Path));
        }

        private void WatcherError(object sender, ErrorEventArgs e)
        {
            _logger.Warn(String.Format("Error in FileSystemWatcher. Reinitializing watcher. Error {0} {1}", e.GetException().Message, e.ToString()));
            //prepareWatcherInExceptionSafeMode();
        }

        private void disposeWatcher()
        {
            if (_watcher != null)
            {
                _logger.Debug("Disposing FileSystemWatcher");
                _watcher.Dispose();
            }
        }

    }
}
