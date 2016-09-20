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
            try
            {
                PrepareWatcher(folderToWatch);
            }
            catch (Exception)
            {
                disposeWatcher();
            }
        }

        private void PrepareWatcher(string directoryPathToWatch)
        {

            if (!Directory.Exists(directoryPathToWatch))
            {
                string error = string.Format("Directory '{0}' not found", directoryPathToWatch);
                return;
            }

            _watcher = new FileSystemWatcher();
            _watcher.Path = directoryPathToWatch;
            _watcher.IncludeSubdirectories = false;
            _watcher.InternalBufferSize = 64 * 1024;
            _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            _watcher.Created += FileDetected; 
            _watcher.EnableRaisingEvents = true;
        }

        private void disposeWatcher()
        {
            if (_watcher != null)
            {
                _watcher.Dispose();
            }
        }

    }
}
