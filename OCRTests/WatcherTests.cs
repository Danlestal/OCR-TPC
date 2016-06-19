using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace WatcherCmd.Files.Tests
{
    [TestClass()]
    public class WatcherWatcherTests
    {

        [TestMethod()]
        public void CreateTempPath_Test()
        {
            string result = Path.GetTempPath();


            var appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            Assert.AreEqual(result, @"..\..\..\AppData\Local\Temp\");

        }
    }
}