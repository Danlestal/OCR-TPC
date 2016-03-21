using Quartz;


namespace WatcherCmd.Jobs
{
    public class OCRJob
    {
        public string Name { get; set; }
        public ITrigger Trigger { get; set; }
        public IJobDetail JobDetail { get; set; }
    }
}
