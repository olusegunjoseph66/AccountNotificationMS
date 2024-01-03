using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.Interfaces.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task StartAccountMsg();
    }
}
