﻿using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.MessageQueue.Client.Wrappers;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;
using Thycotic.Utility.Testing.BDD;
using Thycotic.Utility.Testing.DataGeneration;
using Thycotic.Utility.Testing.TestChain;

namespace Thycotic.MessageQueue.Client.Tests.Wrappers
{
    [TestFixture]
    public class BlockingConsumerWrapperTests : BehaviorTestBase<BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>>
    {

        private CancellationTokenSource _cts = new CancellationTokenSource();
        private string _exchangeName;
        private ICommonModel _model;
        private ICommonConnection _commonConnection;
        private IExchangeNameProvider _exchangeNameProvider;
        private IObjectSerializer _objectSerializer;
        private IMessageEncryptor _messageEncryptor;
        private IPrioritySchedulerProvider _prioritySchedulerProvider;
        private Func<Owned<IBlockingConsumer<IBlockingConsumable, object>>> _consumerFactory;
        private IBlockingConsumer<IBlockingConsumable, object> _consumer;

        private void WaitToOpenChannel()
        {
            //wait for the connection to fire and assign the command model
            while (Sut.CommonModel == null)
            {
                Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
            }
        }

        private void WaitToForAsyncComplete()
        {
            //wait for the model to receive and ack or nack, effectively signling completion
            _cts.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(30));
        }

        [SetUp]
        public override void SetUp()
        {
            _cts = new CancellationTokenSource();
            _model = TestedSubstitute.For<ICommonModel>();

            //since we don't have access to the inner task used the model to know when consumption is done.
            _model.When(m => m.BasicAck(Arg.Any<DeliveryTagWrapper>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>())).Do(info => _cts.Cancel());
            _model.When(m => m.BasicNack(Arg.Any<DeliveryTagWrapper>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())).Do(info => _cts.Cancel());

            _commonConnection = new TestConnection(_model);
            _exchangeNameProvider = TestedSubstitute.For<IExchangeNameProvider>();
            _exchangeName = this.GenerateUniqueDummyName();
            _exchangeNameProvider.GetCurrentExchange().Returns(_exchangeName);


            _objectSerializer = new JsonObjectSerializer();
            _messageEncryptor = TestedSubstitute.For<IMessageEncryptor>();

            //just return what is sent (skip the first argument - exchange - and return the bytes)
            _messageEncryptor.Encrypt(Arg.Any<string>(), Arg.Any<byte[]>()).Returns(info => info.Args().Skip(1).First());

            //just return what is sent (skip the first argument - exchange - and return the bytes)
            _messageEncryptor.Decrypt(Arg.Any<string>(), Arg.Any<byte[]>()).Returns(info => info.Args().Skip(1).First());

            _prioritySchedulerProvider = TestedSubstitute.For<IPrioritySchedulerProvider>();
            var testContext = new SynchronizationContext();
            _prioritySchedulerProvider.Lowest.Returns(new PriorityScheduler(testContext, ThreadPriority.Lowest));
            _prioritySchedulerProvider.BelowNormal.Returns(new PriorityScheduler(testContext, ThreadPriority.BelowNormal));
            _prioritySchedulerProvider.Normal.Returns(new PriorityScheduler(testContext, ThreadPriority.Normal));
            _prioritySchedulerProvider.AboveNormal.Returns(new PriorityScheduler(testContext, ThreadPriority.AboveNormal));
            _prioritySchedulerProvider.Highest.Returns(new PriorityScheduler(testContext, ThreadPriority.Highest));

            _consumer = TestedSubstitute.For<IBlockingConsumer<IBlockingConsumable, object>>();

            _consumerFactory =
                () =>
                    new LeakyOwned<IBlockingConsumer<IBlockingConsumable, object>>(
                        _consumer, new LifetimeDummy());

            Sut = new BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>(_commonConnection, _exchangeNameProvider, _objectSerializer, _messageEncryptor, _prioritySchedulerProvider, _consumerFactory);
        }

        [Test]
        public override void ConstructorParametersDoNotExceptInvalidParameters()
        {
            this.ShouldFail<ArgumentNullException>("Precondition failed: connection != null", () => new BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>(null, _exchangeNameProvider, _objectSerializer, _messageEncryptor, _prioritySchedulerProvider, _consumerFactory));
            this.ShouldFail<ArgumentNullException>("Precondition failed: exchangeNameProvider != null", () => new BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>(_commonConnection, null, _objectSerializer, _messageEncryptor, _prioritySchedulerProvider, _consumerFactory));
            this.ShouldFail<ArgumentNullException>("Precondition failed: objectSerializer != null", () => new BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>(_commonConnection, _exchangeNameProvider, null, _messageEncryptor, _prioritySchedulerProvider, _consumerFactory));
            this.ShouldFail<ArgumentNullException>("Precondition failed: messageEncryptor != null", () => new BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>(_commonConnection, _exchangeNameProvider, _objectSerializer, null, _prioritySchedulerProvider, _consumerFactory));
            this.ShouldFail<ArgumentNullException>("Precondition failed: prioritySchedulerProvider != null", () => new BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>(_commonConnection, _exchangeNameProvider, _objectSerializer, _messageEncryptor, null, _consumerFactory));
            this.ShouldFail<ArgumentNullException>("Precondition failed: consumerFactory != null", () => new BlockingConsumerWrapper<IBlockingConsumable, object, IBlockingConsumer<IBlockingConsumable, object>>(_commonConnection, _exchangeNameProvider, _objectSerializer, _messageEncryptor, _prioritySchedulerProvider, null));
        }

        /// <summary>
        /// Basic deliver should relay when appropriate.
        /// </summary>
        [Test]
        public void HandleBasicDeliverShouldRelayWhenAppropriate()
        {
            //TODO: Clean up this test and make BLocking publish

            var consumerTag = string.Empty;
            var deliveryTag = new DeliveryTagWrapper(0);
            var redelivered = false;
            var routingKey = string.Empty;
            ICommonModelProperties properties = null;
            TestBlockingConsumable consumable = null;
            byte[] body = null;

            Given(() =>
            {

                consumerTag = this.GenerateUniqueDummyName();
                deliveryTag = new DeliveryTagWrapper(1);
                redelivered = false;
                routingKey = this.GenerateUniqueDummyName();
                properties = TestedSubstitute.For<ICommonModelProperties>();
                consumable = new TestBlockingConsumable
                {
                    Content = this.GenerateUniqueDummyName(),

                };
                body = _objectSerializer.ToBytes(consumable);

                Sut.StartConsuming();

                _consumer.When(c => c.Consume(Arg.Any<CancellationToken>(), Arg.Any<IBlockingConsumable>())).Do(info =>
                {
                    var token = (CancellationToken)info.Args().First();
                    var consumable2 = (TestBlockingConsumable)info.Args().Skip(1).First();

                    consumable2.Content.Should().Be(consumable.Content);
                });
            });



            When(() =>
            {
                WaitToOpenChannel();

                Sut.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, _exchangeName, routingKey, properties, body);

                WaitToForAsyncComplete();
            });


            Then(() =>
            {
                Sut.CommonModel.Received().BasicConsume(Arg.Any<string>(), Arg.Any<bool>(), Sut);

                _consumer.Received().Consume(Arg.Any<CancellationToken>(), Arg.Any<TestBlockingConsumable>());

                Sut.CommonModel.Received().BasicAck(deliveryTag, _exchangeName, routingKey, false);

            });
        }

        /// <summary>
        /// Basic deliver should not relay corrupted.
        /// </summary>
        [Test]
        public void HandleBasicDeliverShouldNotRelayCorrupted()
        {
            //TODO: Clean up this test and make BLocking publish

            var consumerTag = string.Empty;
            var deliveryTag = new DeliveryTagWrapper(0);
            var redelivered = false;
            var routingKey = string.Empty;
            ICommonModelProperties properties = null;
            byte[] body = null;

            Given(() =>
            {

                consumerTag = this.GenerateUniqueDummyName();
                deliveryTag = new DeliveryTagWrapper(1);
                redelivered = false;
                routingKey = this.GenerateUniqueDummyName();
                properties = TestedSubstitute.For<ICommonModelProperties>();
                body = Encoding.UTF8.GetBytes(this.GenerateUniqueDummyName());

                Sut.StartConsuming();
            });



            When(() =>
            {
                WaitToOpenChannel();

                Sut.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, _exchangeName, routingKey, properties, body);

                WaitToForAsyncComplete();
            });


            Then(() =>
            {
                Sut.CommonModel.Received().BasicConsume(Arg.Any<string>(), Arg.Any<bool>(), Sut);

                _consumer.DidNotReceive().Consume(Arg.Any<CancellationToken>(), Arg.Any<IBlockingConsumable>());
                _consumer.DidNotReceive().Consume(Arg.Any<CancellationToken>(), Arg.Any<TestBlockingConsumable>());

                Sut.CommonModel.Received().BasicNack(deliveryTag, _exchangeName, routingKey, false, false);

            });
        }
    }
}