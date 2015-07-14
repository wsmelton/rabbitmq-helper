﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using NSubstitute;
using NUnit.Framework;
using Thycotic.Utility.Testing.BDD;

namespace Thycotic.WindowsService.Bootstraper.Tests
{
    [TestFixture]
    class ServiceUpdaterTests : BehaviorTestBase<ServiceUpdater>
    {
        private CancellationTokenSource _cts;
        private IServiceManagerInteractor _serviceManagerInteractor;
        private IProcessRunner _processRunner;
        private string _workingPath;
        private string _backupPath;
        private string _serviceName;
        private string _msiPath;

        [TestFixtureSetUp]
        public override void SetUp()
        {
           _cts = new CancellationTokenSource();
           _serviceManagerInteractor = Substitute.For<IServiceManagerInteractor>();
           _processRunner = Substitute.For<IProcessRunner>();
          
           _workingPath = Path.Combine(Path.GetTempPath(), "updateTests");
           Directory.CreateDirectory(_workingPath);

           _backupPath = Path.Combine(_workingPath, ServiceUpdater.BackupDirectoryName);
           Directory.CreateDirectory(_backupPath);

           _serviceName = Guid.NewGuid().ToString();

           _msiPath = Path.Combine(Path.GetTempPath(), string.Format("fake-{0}.msi", Guid.NewGuid()));
           using (File.Create(_msiPath))
           {
                //create and dispose
           }


           Sut = new ServiceUpdater(_cts, _serviceManagerInteractor, _processRunner, _workingPath, _backupPath, _serviceName, _msiPath, false);

        }

        [TestFixtureTearDown]
        public override void TearDown()
        {

            if (Directory.Exists(_workingPath))
            {
                Directory.Delete(_workingPath, true);
            }

            if (Directory.Exists(_backupPath))
            {
                Directory.Delete(_backupPath, true);
            }


            if (File.Exists(_msiPath))
            {
                File.Delete(_msiPath);
            }
        }

        [Test]
        public override void ConstructorParametersDoNotExceptInvalidParameters()
        {
            this.ShouldFail<ArgumentNullException>("Precondition failed: cts != null", () => new ServiceUpdater(null, _serviceManagerInteractor, _processRunner, _workingPath, _backupPath, _serviceName, _msiPath, false));
            this.ShouldFail<ArgumentNullException>("Precondition failed: serviceManagerInteractor != null", () => new ServiceUpdater(_cts, null, _processRunner, _workingPath, _backupPath, _serviceName, _msiPath, false));
            this.ShouldFail<ArgumentNullException>("Precondition failed: processRunner != null", () => new ServiceUpdater(_cts, _serviceManagerInteractor, null, _workingPath, _backupPath, _serviceName, _msiPath, false));
            this.ShouldFail<ArgumentNullException>("Precondition failed: !string.IsNullOrWhiteSpace(workingPath)", () => new ServiceUpdater(_cts, _serviceManagerInteractor, _processRunner, null, _backupPath, _serviceName, _msiPath, false));
            this.ShouldFail<ArgumentNullException>("Precondition failed: !string.IsNullOrWhiteSpace(backupPath)", () => new ServiceUpdater(_cts, _serviceManagerInteractor, _processRunner, _workingPath, null, _serviceName, _msiPath, false));
            this.ShouldFail<ArgumentNullException>("Precondition failed: !string.IsNullOrWhiteSpace(serviceName)", () => new ServiceUpdater(_cts, _serviceManagerInteractor, _processRunner, _workingPath, _backupPath, null, _msiPath, false));
            this.ShouldFail<ArgumentNullException>("Precondition failed: !string.IsNullOrWhiteSpace(updatePath)", () => new ServiceUpdater(_cts, _serviceManagerInteractor, _processRunner, _workingPath, _backupPath, _serviceName, null, false));
        }

        [Test]
        public void RunUpdate()
        {

            Given(() =>
            {
                //hijack the process to start
                _processRunner.Start(Arg.Any<ProcessStartInfo>()).Returns(info =>
                {
                    var startInfo = (ProcessStartInfo)info.Args()[0];

                    startInfo.FileName = "cmd";
                    //print and exit
                    startInfo.Arguments = "/c echo no-op";

                    return Process.Start(startInfo);
                });
            });

            When(() =>
            {
                Sut.Update();
            });

            Then(() =>
            {
              //TODO: Flesh out -dkk
            });
        }
    }
}
