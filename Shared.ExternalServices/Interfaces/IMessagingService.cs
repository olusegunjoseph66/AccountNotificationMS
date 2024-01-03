using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.Interfaces
{
    public interface IMessagingService
    {
        Task PublishTopicMessage(dynamic message, string label);
        Task PublishTopicMessage(List<string> messages, string label);
        ServiceBusProcessor ConsumeMessage(string topicName, string subscriptionName);
        Task<IList<Message>> GetDeadLetterMessages(string topicName, string subscription);
        Task DeleteDeadLetterMessage(string topicName, string subscription, string lockToken);
        Task DeleteDeadLetterMessages(string topicName, string subscription, List<string> lockTokens);
    }
}
