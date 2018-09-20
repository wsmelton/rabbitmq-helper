﻿using System;
using Thycotic.RabbitMq.Helper.Logic.OS;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli
{
    /// <summary>
    /// CTL RabbitMqProcess interactor 
    /// </summary>
    /// <seealso cref="IProcessInteractor" />
    public class RabbitMqBatPluginClient : RabbitMqBatClient
    {
       
        /// <summary>
        ///     Gets the executable.
        /// </summary>
        /// <value>
        ///     The executable.
        /// </value>
        protected override string Executable => "rabbitmq-plugins.bat";


        /// <summary>
        /// Enables the management console.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public string EnableManagementConsole()
        {
            const string parameters2 = "enable rabbitmq_management";

            var output = Invoke(parameters2, TimeSpan.FromSeconds(60));

            ValidateOutput("Plugin configuration unchanged.", output, false);

            return output;
        }
    }
}
