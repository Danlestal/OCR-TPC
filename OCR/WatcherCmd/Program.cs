using log4net;
using Ninject;
using OCR;
using System;
using System.ServiceProcess;
using WatcherCmd.Ninject;

namespace WatcherCmd
{
    static class Program
    {

        private static readonly ILog _logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            initLogger();

            try
            {
                var kernel = new StandardKernel(new WatcherCmdNinjectModule());


                var service = kernel.Get<OCRService>();
                //ServiceBase.Run(service);
            }
            catch (Exception e)
            {
                _logger.Error("Exception while launching OCRService", e);
            }
        }

        private static void initLogger()
        {
            throw new NotImplementedException();
        }


    
    }
}
