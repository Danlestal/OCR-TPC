using System;

namespace WatcherCmd
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            string prefix = "LOG (" + System.DateTime.Now.ToString() + "):";
            Console.WriteLine(prefix + message);
        }
    }
}
