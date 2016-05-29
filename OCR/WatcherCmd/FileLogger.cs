using System;
using System.IO;

namespace WatcherCmd
{
    public class FileLogger : ILogger
    {
        private StreamWriter writer;

        public FileLogger(string filePath)
        {
            writer = File.AppendText(filePath);
        }

        public void Log(string message)
        {
            string prefix = "LOG (" + System.DateTime.Now.ToString() + "):";
            writer.WriteLine(prefix + message);
        }
    }
}
