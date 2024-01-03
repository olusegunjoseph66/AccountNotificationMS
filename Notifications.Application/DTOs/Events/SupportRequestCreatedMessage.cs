using Notifications.Application.DTOs.Sortings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class SupportRequestCreatedMessage
    {
        public string Id { get; set; }
        public int RequestId { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByName { get; set; } = "";
        public RequestCategory RequestCategory { get; set; } = new RequestCategory();
        public RequestStatus RequestStatus { get; set; } = new RequestStatus();

        public string Subject { get; set; } = "";

        public string Description { get; set; } = "";

        public string ChannelCode { get; set; } = "";
        public string CompanyCode { get; set; }
        public string DistributorSapNumber { get; set; }
        public string ReferenceId { get; set; }

    }
}
