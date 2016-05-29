using WatcherCmd.Files;
using System;
using WatcherCmd.Files.Interface;
using OCR;
using System.Configuration;

namespace WatcherCmd.Jobs
{
    public class WatcherCmdJob
    {
        
        public WatcherCmdJob()
        {
        
        }

        public string Id { get { return "ManagerProcess"; } }

        public void Execute()
        {
            var client = new APIClient(ConfigurationManager.AppSettings["ApiURL"]);

            var certWatcher = new Watcher();
            IManager certManager = new CertManager(new ConsoleLogger(), certWatcher, client);
            certManager.InitializeSystem();

            var laboralWatcher = new Watcher();
            IManager vidaLaboralManager = new VidaLaboralManager(new ConsoleLogger(), laboralWatcher, client);
            vidaLaboralManager.InitializeSystem();
        }
    }
}
