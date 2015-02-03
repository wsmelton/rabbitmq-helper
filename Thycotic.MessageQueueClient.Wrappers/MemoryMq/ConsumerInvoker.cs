﻿using System;
using Autofac;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueueClient.Wrappers.MemoryMq
{
    /// <summary>
    /// Consumer wrapper factory
    /// </summary>
    [Obsolete("Do not use")]
    public class ConsumerInvoker : IConsumerInvoker
    {
        private readonly IComponentContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumerInvoker"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ConsumerInvoker(IComponentContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Consumes the specified consumable.
        /// </summary>
        /// <param name="consumable">The consumable.</param>
        public void Consume(IConsumable consumable)
        {
            var requestType = consumable.GetType();
            var baseConsumerType = typeof (IBasicConsumer<>);
            var consumerType = baseConsumerType.MakeGenericType(requestType);

            //HACK: I don't like this -dkk
            dynamic consumer = _context.Resolve(consumerType);

            //HACK: I don't like this -dkk
            consumer.Consume((dynamic)consumable);

        }

        /// <summary>
        /// Consumes the specified consumable.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="consumable">The consumable.</param>
        /// <returns></returns>
        public TResponse Consume<TResponse>(IConsumable consumable)
        {
            var requestType = consumable.GetType();
            var responseType = typeof (TResponse);
            var baseConsumerType = typeof(IBlockingConsumer<,>);
            var consumerType = baseConsumerType.MakeGenericType(requestType, responseType);

            //HACK: I don't like this -dkk
            dynamic consumer = _context.Resolve(consumerType);

            //HACK: I don't like this -dkk
            return consumer.Consume((dynamic)consumable);
        }
    }
}