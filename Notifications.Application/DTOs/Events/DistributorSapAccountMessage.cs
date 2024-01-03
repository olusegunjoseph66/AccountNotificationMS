using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class DistributorSapAccountMessage
    {
        public int DistributorSapAccountId { get; set; }
        public string DistributorSapNumber { get; set; }
        public string DistributorName { get; set; }
    }
}
