using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mq.Shared.Messages;
using Mq.Shared.Handlers;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;
using ServiceStack.Messaging;

namespace Mq.Shared.Services
{
    public class MessageQueueService: IMessageQueueService
    {
        private readonly ILogger<MessageQueueService> _logger;
        private readonly List<string> _handlers;
        private readonly RedisMqServer _mqServer;
        private readonly IMessageQueueClient _mqClient;
        private readonly IServiceProvider _provider;

        public MessageQueueService(IServiceProvider provider, ILogger<MessageQueueService> logger)
        {
            _handlers = new List<string>();
            _logger = logger;
            var redisFactory = new PooledRedisClientManager("localhost:6379");
            _mqServer = new RedisMqServer(redisFactory, retryCount:2);
            _mqClient = _mqServer.CreateMessageQueueClient();
            _provider = provider;
        }
        
        public void StartListening()
        {   
            _mqServer.Start(); //Starts listening for messages
        }

        public TR Publish<T, TR>(T message)
        {   
            var replyToMq = _mqClient.GetTempQueueName();
            
            var clientMsg = new Message<T>(message) {
                ReplyTo =  replyToMq
            };

            _mqClient.Publish(clientMsg);
            var responseMsg = _mqClient.Get<TR>(replyToMq, TimeSpan.FromSeconds(10));
            _mqClient.Ack(responseMsg);

            if (responseMsg == null)
                throw new TimeoutException();
            
            return responseMsg.GetBody();
        }

        public void Subscribe<T, TR, TH>() 
            where T : BaseMessage 
            where TR : BaseMessage 
            where TH : IBaseMessageHandler<T, TR>
        {
            var messageName = typeof(T).Name;
            _logger.LogInformation("Subscribing to message {MessageName} with {EventHandler}", messageName, nameof(TH));
            
            if (_handlers.Contains(messageName))
            {
                _logger.LogError("Message {MessageName} has been registered already.", messageName);
            }
            else
            {
                _logger.LogInformation("Message {MessageName} subscribed with {EventHandler}", messageName, nameof(TH));
                var handler = (TH) ActivatorUtilities.CreateInstance(_provider, typeof(TH));
                _mqServer.RegisterHandler<T>(m => handler.Handle(m.GetBody()));
                _handlers.Add(messageName);
            }   
        }
    }
}