using WatcherCmd.Configuration;
using log4net;
using Quartz;


namespace WatcherCmd.Jobs
{
    class WatcherCmdJobHelper : IOCRJobHelper
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(WatcherCmdJobHelper));

        private static IConfigurationProvider _configProvider;


        public WatcherCmdJobHelper(IConfigurationProvider configProvider)
        {

            _configProvider = configProvider;
            
        }

         
        public OCRJob GetJob()
        {
            var result = new OCRJob();

            result.Name = "Watcher Cmd Job";
            result.JobDetail = createJobDetail();
            result.Trigger = createTrigger();

            return result;
        }

        private ITrigger createTrigger()
        {
            _logger.Info("Preparing  Watcher Trigger");
            var jobDataMap = getJobData();
            var cronExpression = _configProvider.GetHourlyCronExpression();
            
            var result = getTrigger(jobDataMap, cronExpression);
            _logger.Info("Watcher Trigger prepared");
            return result;
        }

        private JobDataMap getJobData()
        {
            _logger.Info("Preparing Watcher Job Data");

            JobDataMap result = new JobDataMap();
            result.Put("ConfigurationProvider", _configProvider);

            _logger.Info("Done preparing Watcher Job Data");
            return result;
        }

        private ITrigger getTrigger(JobDataMap jobDataMap, string cronExpression)
        {
            if(cronExpression == "NOW")
            {
                return TriggerBuilder.Create()
                    .StartNow()
                    //.WithCronSchedule(cronExpression, x => x.WithMisfireHandlingInstructionDoNothing())
                    .UsingJobData(jobDataMap)
                    .Build();
            } else
            {
                return TriggerBuilder.Create()
                    .WithCronSchedule(cronExpression, x => x.WithMisfireHandlingInstructionDoNothing())
                    .UsingJobData(jobDataMap)
                    .Build();
            }
            
        }

        private IJobDetail createJobDetail()
        {
            _logger.Info("Preparing WatcherCmd JobDetail");

            var result = JobBuilder.Create()
                .OfType(typeof(WatcherCmdJob))
                .Build();

            _logger.Info("JobDetail WatcherCmd prepared");
            return result;
        }
    }
}
