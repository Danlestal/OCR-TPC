using WatcherCmd.Configuration;
using WatcherCmd.Files;
//using EDPItalyWinService.Ftp;
using WatcherCmd.Ninject;
//using EDPItalyWinService.Services;
using log4net;
using Ninject;
using Quartz;
using System;
using WatcherCmd.Files.Interface;

namespace WatcherCmd.Jobs
{
    [DisallowConcurrentExecution]
    public class WatcherCmdJob : IJob
    {
        StandardKernel _ninjectKernel;

        public WatcherCmdJob()
        {
            _ninjectKernel = new StandardKernel(new WatcherCmdNinjectModule());
            _configProvider = _ninjectKernel.Get<IConfigurationProvider>();
        }

        private static readonly ILog _logger = LogManager.GetLogger(typeof(WatcherCmdJob));

        public string Id { get { return "ManagerProcess"; } }

        private static IConfigurationProvider _configProvider;

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("FileProcesserJob started");
                execute(context);
                _logger.Info("FileProcesserJob ended");
            }
            catch (Exception e)
            {
                _logger.ErrorFormat("Exception while executing FileProcesserJob: {0}\nStackTrace: \n{1}\n", e.Message, e.StackTrace);
            }
        }

        private void execute(IJobExecutionContext context)
        {
            //jvr breAKpoint
            prepareContextData(context);
            refreshConfigSettings();

            Watcher watcher = new Watcher(_logger);

            IManager manager = new Manager(_logger, watcher);
            manager.InitializeSystem();
        
        }
 
        private void prepareContextData(IJobExecutionContext context)
        {
            _logger.Info("Getting Job Data from context");

            var confProvider = context.Trigger.JobDataMap.Get("ConfigurationProvider") as IConfigurationProvider;
            if (confProvider == null)
                throw new ArgumentException("Cant get IConfigurationProvider from context");
            _configProvider = confProvider;

                    }
        
        private void refreshConfigSettings()
        {
            _logger.Debug("Refreshing config file cache");
            _configProvider.Refresh();
            _logger.Debug("Done refreshing config file cache");
        }





    }



}
