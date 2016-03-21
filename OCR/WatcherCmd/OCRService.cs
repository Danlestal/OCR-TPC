
using Common.Logging;
using System;
using System.ServiceProcess;
using WatcherCmd.Configuration;
using WatcherCmd.Jobs;
using WatcherCmd.Jobs.Interfaces;

namespace OCR
{
    public partial class OCRService : ServiceBase
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(OCRService));
        private static IOCRJobFactory _jobFactory;
        private ICronDispatcher _dispatcher;

        public OCRService()
        {
               //InitializeComponent();
        }

        static void Main(string[] args)
        {
            try
            {
                run(args);
            }
            catch (Exception e)
            {
                _logger.Error("Exception while running service", e);
            }
        }

        private static void run(string[] args)
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
                initDispatcher();
            }
            catch (Exception e)
            {
                _logger.ErrorFormat("Exception while launching dispatcher: {0}\nStackTrace: \n{1}\n", e.Message, e.StackTrace);
            }
        }

        protected override void OnStop()
        {
            //_logger.Info("STOPPED");
            //if (_dispatcher != null)
            //    _dispatcher.Dispose();

            //base.OnStop();
        }

        private void initDispatcher()
        {
            _jobFactory = new OCRJobFactory(new ConfigurationProvider());
            _dispatcher = new Dispatcher(_jobFactory);
            _dispatcher.Init();
        }
    }
}
