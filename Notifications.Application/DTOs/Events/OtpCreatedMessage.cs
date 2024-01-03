using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class OtpGeneratedMessage
    {
        public int OtpId { get; set; }
        public string OtpCode { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public DateTime DateExpiry { get; set; }
    }
}
