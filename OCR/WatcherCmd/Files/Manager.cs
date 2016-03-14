using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatcherCmd.Files.Interface;

namespace WatcherCmd.Files
{
    public class Manager
    {

        private readonly IWatcher fileDetecteted;

        public Manager (IWatcher fileDetecteted)
        {
            this.fileDetecteted = fileDetecteted;

            fileDetecteted.FileDetected += (sender, args) =>
            {

                //Do Something
            };

        }


    }
}
