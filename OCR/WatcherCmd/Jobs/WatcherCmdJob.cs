using WatcherCmd.Files;
using System;
using WatcherCmd.Files.Interface;
using Common.Logging;

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
            try
            {
                _logger.Info("FileProcesserJob started");
                execute();
                _logger.Info("FileProcesserJob ended");
            }
            catch (Exception e)
            {
                _logger.ErrorFormat("Exception while executing FileProcesserJob: {0}\nStackTrace: \n{1}\n", e.Message, e.StackTrace);
            }
        }

        private void execute()
        {
                        
            Watcher watcher = new Watcher();

            IManager manager = new Manager(_logger, watcher);
            manager.InitializeSystem();
        
        }
 
    }

}
