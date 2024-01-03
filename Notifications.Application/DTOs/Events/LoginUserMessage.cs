using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class LoginUserMessage
    {
        public int UserId { get; set; }

        public DateTime LoginDate { get; set; }

        public string IpAddress { get; set; } = "";

        public string DeviceId { get; set; } = "";

        public string ChannelCode { get; set; } = "";
    }
}
