using System;
using Quartz;
using Quartz.Impl;
using log4net;
using WatcherCmd.Jobs.Interfaces;
using WatcherCmd.Configuration;

namespace WatcherCmd.Jobs
{
    public interface ICronDispatcher : IDisposable
    {
        void Init();
    }

    public class Dispatcher : ICronDispatcher
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Dispatcher));

        private static IScheduler _scheduler;
        private static IOCRJobFactory _jobFactory;
        private static IConfigurationProvider _confProvider;

        public Dispatcher(IOCRJobFactory jobFactory)
        {
            _confProvider = new ConfigurationProvider();
            _scheduler = new StdSchedulerFactory().GetScheduler();
            _jobFactory = jobFactory;
        }

        public void Init()
        {
            try
            {
                _logger.Info("Starting Dispatcher");
                init();
            }
            catch (Exception e)
            {
                _logger.ErrorFormat("Exception while preparing jobs: {0}\nStackTrace: \n{1}\n", e.Message, e.StackTrace);
            }
        }

        private void init()
        {
            IOCRJobFactory _jobFactory = new OCRJobFactory(_confProvider);
            foreach (var OCRJob in _jobFactory.GetOCRJobs())
            {
                scheduleOCRJob(OCRJob);
            }
            _scheduler.Start();
        }

        private void scheduleOCRJob(OCRJob OCRJob)
        {
            _logger.InfoFormat("Scheduling job {0}", OCRJob.Name);
            _scheduler.ScheduleJob(OCRJob.JobDetail, OCRJob.Trigger);
        }

        public void Dispose()
        {
            _logger.Info("Disposing Dispatcher");
            if (_scheduler != null)
            {
                _scheduler.Shutdown(false);
                _scheduler = null;
            }
        }
    }
}
