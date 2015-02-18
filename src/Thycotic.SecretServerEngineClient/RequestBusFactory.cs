﻿using Thycotic.MessageQueueClient;
using Thycotic.MessageQueueClient.QueueClient;
using Thycotic.MessageQueueClient.QueueClient.MemoryMq;
using Thycotic.MessageQueueClient.QueueClient.RabbitMq;
using Thycotic.Utility.Serialization;

namespace Thycotic.SecretServerEngineClient
{
    /// <summary>
    /// Basic request bus factory
    /// </summary>
    public static class RequestBusFactory
    {
        /// <summary>
        /// Gets the Rabbit mq bus.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        /// <param name="messageEncryptor">The message encryptor.</param>
        /// <returns></returns>
        public static IRequestBus GetRabbitMqBus(string url, string userName, string password, bool useSsl, IMessageEncryptor messageEncryptor)
        {
            var connection = new RabbitMqConnection(url, userName, password, useSsl);

            var objectSerializer = new JsonObjectSerializer();

            return new RequestBus(connection, objectSerializer, messageEncryptor);
        }

        /// <summary>
        /// Gets the memory mq bus.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        /// <param name="messageEncryptor">The message encryptor.</param>
        /// <returns></returns>
        public static IRequestBus GetMemoryMqBus(string url, bool useSsl, IMessageEncryptor messageEncryptor)
        {
            var connection = new MemoryMqConnection(url, useSsl);

            var objectSerializer = new JsonObjectSerializer();

            return new RequestBus(connection, objectSerializer, messageEncryptor);
        }
    }
}
