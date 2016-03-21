using System.Collections.Generic;
using WatcherCmd.Configuration;
using WatcherCmd.Jobs.Interfaces;
using WatcherCmd.Ninject;

namespace WatcherCmd.Jobs
{

    class OCRJobFactory : IOCRJobFactory
    {
        private static IConfigurationProvider _configProvider;

        public OCRJobFactory(IConfigurationProvider configProvider)
        {
            _configProvider = configProvider;
        }

        public IList<OCRJob> GetOCRJobs()
        {
            var result = new List<OCRJob>();

            foreach (var jobHelper in getJobHelpers())
            {
                var job = jobHelper.GetJob();
                result.Add(job);
            }

            return result;
        }

        private IList<IOCRJobHelper> getJobHelpers()
        {
            var result = new List<IOCRJobHelper>();
            result.Add(WatcherCmdNinjectModule.GetDependency<WatcherCmdJobHelper>());
            return result;
        }
    }
}
