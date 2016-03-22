using Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using WatcherCmd.Files.Interface;

namespace WatcherCmd.Files.Tests
{
    [TestClass()]
    public class WatcherWatcherTests
    {

        private static ILog _logger;
        private Mock<Watcher> mockWatcher;
        private Manager _manager;

        [TestMethod()]
        public void Init_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Manager_InitializeSystem_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void MyEventIsRaised_Test()
        {
            List<string> receivedEvents = new List<string>();

            var mockWatcher = new Mock<IWatcher>();
            //var manager = new Manager(mockWatcher.Object);

            // arrange
            //this.mockWatcher = new Mock<Watcher>(MockBehavior.Strict);
            //this._manager = new Manager(this.mockWatcher.Object);

            // act
            this.mockWatcher.Raise(mock => mock.FileDetected += null, new EventArgs()); // this does not fire

            Watcher _watcher = new Watcher(_logger);

            //this.mockWatcher.Verify(mock => mock.FileDetected(It.IsAny<string>()));
            

            Assert.Fail();
        }
    }
}