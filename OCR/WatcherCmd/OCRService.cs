
using Common.Logging;
using System;
using System.ServiceProcess;
using WatcherCmd.Jobs;

namespace OCR
{
    public partial class OCRService : ServiceBase
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(OCRService));

        public OCRService()
        {
         
        }

        static void Main(string[] args)
        {
            try
            {
                Run(args);
            }
            catch (Exception e)
            {
                _logger.Error("Exception while running service", e);
            }
        }

        private static void Run(string[] args)
        {
            var service = new OCRService();
            if (Environment.UserInteractive)
            {
                _logger.Info("User Interactive detected");
                service.OnStart(args);
                Console.WriteLine("Press any key to stop program");
                Console.ReadLine();
                service.OnStop();
                _logger.Info("Exiting");
            }
            else
            {
                _logger.Info("Running as windows service");
                Run(service);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _logger.Info("STARTED");
                InitDispatcher();
            }
            catch (Exception e)
            {
                _logger.ErrorFormat("Exception while launching dispatcher: {0}\nStackTrace: \n{1}\n", e.Message, e.StackTrace);
            }
        }

        protected override void OnStop()
        {
            _logger.Info("STOPPED");
            
            base.OnStop();
        }

        private void InitDispatcher()
        {

            WatcherCmdJob _watcherCmdJob = new WatcherCmdJob();
            _watcherCmdJob.Execute();
        }
    }
}
