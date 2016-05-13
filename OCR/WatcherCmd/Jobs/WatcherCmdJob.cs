using WatcherCmd.Files;
using System;
using WatcherCmd.Files.Interface;
using Common.Logging;
using OCR;
using System.Configuration;

namespace WatcherCmd.Jobs
{
    public class WatcherCmdJob
    {
        
        public WatcherCmdJob()
        {
        
        }

        private static readonly ILog _logger = LogManager.GetLogger(typeof(WatcherCmdJob));

        public string Id { get { return "ManagerProcess"; } }

        public void Execute()
        {
            _logger.Info("FileProcesserJob started");
            try
            {
                
                execute();
                
            }
            catch (Exception e)
            {
                _logger.ErrorFormat("Exception while executing FileProcesserJob: {0}\nStackTrace: \n{1}\n", e.Message, e.StackTrace);
            }

            _logger.Info("FileProcesserJob ended");
        }

        private void execute()
        {
                        
            
            var client = new APIClient(ConfigurationManager.AppSettings["ApiURL"]);

            var certWatcher = new Watcher();
            IManager certManager = new CertManager(_logger, certWatcher, client);
            certManager.InitializeSystem();

            var laboralWatcher = new Watcher();
            IManager vidaLaboralManager = new VidaLaboralManager(_logger, laboralWatcher, client);
            vidaLaboralManager.InitializeSystem();

        }
 
    }

}
