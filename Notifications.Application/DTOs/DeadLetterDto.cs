using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs
{
    public class DeadLetterDto
    {
        public int UserId { get; set; }
        public Message Message { get; set; }
        public string EventMessages { get; set; }
        public object Body { get; set; }
    }
}
