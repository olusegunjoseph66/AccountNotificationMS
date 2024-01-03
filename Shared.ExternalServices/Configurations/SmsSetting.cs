using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.Configurations
{
    public class SmsSetting
    {
        public string MessagingServiceSid { get; set; }
        public string AuthToken { get; set; }
        public string AccountSid { get; set; }
    }
}
