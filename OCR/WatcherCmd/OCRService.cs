
using System;
using System.ServiceProcess;
using WatcherCmd.Jobs;

namespace OCR
{
    public partial class OCRService : ServiceBase
    {


        static void Main(string[] args)
        {
            Run(args);
        }

        private static void Run(string[] args)
        {
            var service = new OCRService();
            if (Environment.UserInteractive)
            {
                service.OnStart(args);
                Console.WriteLine("Press any key to stop program");
                Console.ReadLine();
                service.OnStop();
            }
            else
            {
                Run(service);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                InitDispatcher();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while launching dispatcher: {0}\nStackTrace: \n{1}\n", e.Message, e.StackTrace);
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        private void InitDispatcher()
        {

            WatcherCmdJob _watcherCmdJob = new WatcherCmdJob();
            _watcherCmdJob.Execute();
        }
    }
}
