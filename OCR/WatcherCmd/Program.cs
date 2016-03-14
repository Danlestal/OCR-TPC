using Common.Logging;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var kernel = new StandardKernel(new EDPItalyNijectModule());


                var service = kernel.Get<EDPItalyService>();
                ServiceBase.Run(service);
            }
            catch (Exception e)
            {
                _logger.Error("Exception while launching EDPItalyService", e);
            }
        }






        private static void initLogger()
        {
            throw new NotImplementedException();
        }


    
    }
}
