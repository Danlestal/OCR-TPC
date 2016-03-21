using Common.Logging;
using System;
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

            _watcher.Init();
            _watcher.FileDetected += onFileDetected;
            

        }

        private void onFileDetected(object sender, System.IO.FileSystemEventArgs e)
        {
            int retries = 3;

            throw new NotImplementedException();
        }

        

    }
}
