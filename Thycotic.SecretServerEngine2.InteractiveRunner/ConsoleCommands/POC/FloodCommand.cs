﻿using System;
using System.Linq;
using Thycotic.Logging;
using Thycotic.MessageQueueClient;
using Thycotic.Messages.Areas.POC.Request;

namespace Thycotic.SecretServerEngine2.InteractiveRunner.ConsoleCommands.POC
{
    class FloodCommand : ConsoleCommandBase
    {
        private readonly IRequestBus _bus;
        private readonly ILogWriter _log = Log.Get(typeof(FloodCommand));

        public override string Name
        {
            get { return "flood"; }
        }

        public override string Description
        {
            get { return "Floods the exchange with messages"; }
        }

        public FloodCommand(IRequestBus bus)
        {
            _bus = bus;

            Action = parameters =>
            {
                string countString;
                if (!parameters.TryGet("count", out countString)) return;

                var count = Convert.ToInt32(countString);

                _log.Info(string.Format("Flooding exchange with {0} request(s). Please wait...", count));

                Enumerable.Range(0, count).AsParallel().ForAll(i =>
                {
                    var message = new PingMessage();

                    _bus.BasicPublish(message);
                });

                _log.Info("Flooding completed");
            };
        }
    }
}