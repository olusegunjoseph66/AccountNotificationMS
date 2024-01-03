using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.Configurations
{
    public class EmailSetting
    {
        public string BaseUrl { get; set; }
        public string EmailEndpoint { get; set; }
        public string Sender { get; set; }
        public string DisplayName { get; set; }
        public string ApiKey { get; set; }
    }
}
