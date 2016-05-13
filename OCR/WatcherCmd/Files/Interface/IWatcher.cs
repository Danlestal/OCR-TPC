using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatcherCmd.Files.Interface
{
    public interface IWatcher
    {

        event FileSystemEventHandler FileDetected;
        void Init(string folderToWath);
        bool EnableRaisingEvents { get; set; }

    }
}
