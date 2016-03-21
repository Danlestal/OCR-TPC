using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatcherCmd.Files.Interface;

namespace WatcherCmd.Files
{
    public class Manager : IManager
    {

        private readonly IWatcher _watcher;

        public Manager(IWatcher watcher)
        {
            _watcher = watcher;
            _watcher.Init();
        }

        public void InitializeSystem()
        {

            _watcher.FileDetected += onFileDetected;
            _watcher.Init();

        }

        private void onFileDetected(object sender, System.IO.FileSystemEventArgs e)
        {
            int retries = 3;

            throw new NotImplementedException();
        }

        

    }
}
