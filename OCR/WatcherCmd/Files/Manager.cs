using Common.Logging;
using System;
using System.IO;
using WatcherCmd.Files.Interface;

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

            _watcher.FileDetected += onFileDetected;
            _watcher.Init();
            
        }

        private void onFileDetected(object sender, FileSystemEventArgs e)
        {
            int retries = 3;

            throw new NotImplementedException();
        }

        

    }
}
