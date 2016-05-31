using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dynamo.Extensions;
using Dynamo.Models;
using Dynamo.Scheduler;
using Moq;
using NUnit.Framework;

namespace Dynamo.Tests
{
    class ExtensionTests
    {
        string extensionsPath;
        Mock<IExtension> extMock;
        DynamoModel model;

        [SetUp]
        public void Init()
        {
            extensionsPath = Path.Combine(Directory.GetCurrentDirectory(), "extensions");
            extMock = new Mock<IExtension>();

            model = DynamoModel.Start(
                new DynamoModel.DefaultStartConfiguration()
                {
                    StartInTestMode = true,
                    Extensions = new List<IExtension> { extMock.Object },
                    ProcessMode = TaskProcessMode.Synchronous
                });
        }

        [Test]
        public void DirectoryExists()
        {
            Assert.IsTrue(Directory.Exists(extensionsPath));
        }

        [Test]
        public void ExtensionIsStarted()
        {
            extMock.Verify(ext => ext.Startup(It.IsAny<StartupParams>()));
        }

        [Test]
        public void ExtensionIsAdded()
        {
            Assert.IsTrue(model.ExtensionManager.Extensions.Contains(extMock.Object));
        }

        [Test]
        public void ExtensionIsReady()
        {
            extMock.Verify(ext => ext.Ready(It.IsAny<ReadyParams>()));
        }

        [Test]
        public void ExtensionIsDisposed()
        {
            model.Dispose();
            extMock.Verify(ext => ext.Dispose());
        }
    }
}
