using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Moq;
using WatcherCmd.Files.Interface;

namespace OCRTests
{
    [TestClass]
    public class PDFWatcherTest
    {

        public event FileSystemEventHandler FileCreated;
        private IWatcher _watcher;

        public PDFWatcherTest(IWatcher watcher)
        {
            this._watcher = watcher;
            this._watcher.FileDetected += this.FileCreated;
        }

        //[TestMethod]
        //public void DetectorTestMethod()
        //{
        //    string directoryPathToWatch = @"C:\Users\Jorge\Documents\Visual Studio 2015\Projects\OCR-TPC-master\OCRTests\Destination";

        //    if (!Directory.Exists(directoryPathToWatch))
        //    {
        //        string error = string.Format("Directory '{0}' not found", directoryPathToWatch);

        //        //_logger.Fatal(error);
        //        return;
        //    }
        //    //_logger.Info(string.Format("Directory '{0}' found", directoryPathToWatch));

        //    _watcher = new FileSystemWatcher();

        //    _watcher.Path = directoryPathToWatch;
        //    _watcher.IncludeSubdirectories = true;
        //    _watcher.InternalBufferSize = 4 * 1024;
        //    //_watcher.Filter = ".csv";
        //    _watcher.Created += FileCreated;
        //    _watcher.Error += watcherError;
        //    _watcher.IncludeSubdirectories = true;

        //    _watcher.EnableRaisingEvents = true;

        //}

        private void watcherError(object sender, ErrorEventArgs e)
        {
            //_logger.Warn(String.Format("Error in FileSystemWatcher. Reinitializing watcher. Error {0} {1}", e.GetException().Message, e.ToString()));
            //prepareWatcherInExceptionSafeMode();
        }

    }



}
