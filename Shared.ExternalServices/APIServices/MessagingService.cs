using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.ExternalServices.Configurations;
using Shared.ExternalServices.DTOs;
using Shared.ExternalServices.Interfaces;
using System.Text;

namespace Shared.ExternalServices.APIServices
{
    public class MessagingService : IMessagingService
    {
        private readonly MessagingServiceSetting _messagingSetting;
        private readonly ServiceBusClient _client;
        public readonly IConfiguration _config;

        public MessagingService(IOptions<MessagingServiceSetting> messagingSetting, IConfiguration _config)
        {
            this._config = _config;
            //_messagingSetting = messagingSetting.Value;

            _client = new ServiceBusClient(_config["MessagingServiceSetting:ConnectionString"]);
            //_client = new ServiceBusClient(_messagingSetting.ConnectionString);
        }

        public async Task PublishTopicMessage(dynamic message, string label)
        {
            message.Id = Guid.NewGuid();

            var jsonMessage = JsonConvert.SerializeObject(message);
            var busMessage = new Message(Encoding.UTF8.GetBytes(jsonMessage))
            {
                PartitionKey = Guid.NewGuid().ToString(),
                Label = label
            };

            //ISenderClient topicClient = new TopicClient(_messagingSetting.ConnectionString, _messagingSetting.TopicName);

            ISenderClient topicClient = new TopicClient(_config["MessagingServiceSetting:ConnectionString"], _config["MessagingServiceSetting:Orders:TopicName"]);
            await topicClient.SendAsync(busMessage);
            Console.WriteLine($"Sent message to {topicClient.Path}");
            await topicClient.CloseAsync();

        }

        public async Task PublishTopicMessage(List<string> messages, string label)
        {
            List<Message> busMessages = new();
            var partitionId = Guid.NewGuid().ToString();
            messages.ForEach(x =>
            {
                var busMessage = new Message(Encoding.UTF8.GetBytes(x))
                {
                    PartitionKey = partitionId,
                    Label = label
                };
                busMessages.Add(busMessage);
            });

            //ISenderClient topicClient = new TopicClient(_messagingSetting.ConnectionString, _messagingSetting.TopicName);

            ISenderClient topicClient = new TopicClient(_config["MessagingServiceSetting:ConnectionString"], _config["MessagingServiceSetting:Orders:TopicName"]);
            await topicClient.SendAsync(busMessages);
            Console.WriteLine($"Sent message to {topicClient.Path}");
            await topicClient.CloseAsync();

        }

        public ServiceBusProcessor ConsumeMessage(string topicName, string subscriptionName)
        {
            ServiceBusProcessorOptions options = new()
            {
                AutoCompleteMessages = false,
                PrefetchCount = 0,
                MaxConcurrentCalls = 1,
                ReceiveMode = ServiceBusReceiveMode.PeekLock,
                MaxAutoLockRenewalDuration = TimeSpan.FromSeconds(31)
            };
            return _client.CreateProcessor(topicName, subscriptionName, options);
        }

        public async Task<IList<Message>> GetDeadLetterMessages(string topicName, string subscription)
        {
            var subPath = EntityNameHelper.FormatSubscriptionPath(topicName, subscription);

            var deadLetterPath = EntityNameHelper.FormatDeadLetterPath(subPath);

            //var receiver = new MessageReceiver(_messagingSetting.ConnectionString, deadLetterPath, ReceiveMode.PeekLock);

            var receiver = new MessageReceiver(_config["MessagingServiceSetting:ConnectionString"], deadLetterPath, ReceiveMode.PeekLock);
            return await receiver.ReceiveAsync(100);
        }

        public async Task DeleteDeadLetterMessages(string topicName, string subscription, List<string> lockTokens)
        {
            var subPath = EntityNameHelper.FormatSubscriptionPath(topicName, subscription);

            var deadLetterPath = EntityNameHelper.FormatDeadLetterPath(subPath);
            //var receiver = new MessageReceiver(_messagingSetting.ConnectionString, deadLetterPath, ReceiveMode.ReceiveAndDelete);

            var receiver = new MessageReceiver(_config["MessagingServiceSetting:ConnectionString"], deadLetterPath, ReceiveMode.ReceiveAndDelete);
            await receiver.CompleteAsync(lockTokens);
        }

        public async Task DeleteDeadLetterMessage(string topicName, string subscription, string lockToken)
        {
            var subPath = EntityNameHelper.FormatSubscriptionPath(topicName, subscription);

            var deadLetterPath = EntityNameHelper.FormatDeadLetterPath(subPath);
            //var receiver = new MessageReceiver(_messagingSetting.ConnectionString, deadLetterPath, ReceiveMode.ReceiveAndDelete);

            var receiver = new MessageReceiver(_config["MessagingServiceSetting:ConnectionString"], deadLetterPath, ReceiveMode.ReceiveAndDelete);
            await receiver.CompleteAsync(lockToken);
        }
    }
}
