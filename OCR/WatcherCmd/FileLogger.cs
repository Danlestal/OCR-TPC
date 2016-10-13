using System;
using System.IO;

namespace WatcherCmd
{
    public class FileLogger : ILogger
    {
        private string file;
        private StreamWriter writer;

        public FileLogger(string filePath)
        {
            file = filePath;
        }

        public void Log(string message)
        {
            string prefix = "LOG (" + System.DateTime.Now.ToString() + "):";
            using (StreamWriter sw = File.AppendText(file))
            {
                sw.WriteLine(prefix + message);
            }
        }
    }
}
