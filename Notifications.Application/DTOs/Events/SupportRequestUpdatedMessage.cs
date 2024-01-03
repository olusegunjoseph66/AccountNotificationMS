using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class SupportRequestUpdatedMessage
    {
        public int RequestId { get; set; }
        public DateTime? DateModified { get; set; }
        public int ModifiedByUserId { get; set; }
        public string ModifiedByName { get; set; } = "";

        public RequestStatus OldRequestStatus { get; set; } = new RequestStatus();

        public RequestStatus NewRequestStatus { get; set; } = new RequestStatus();
        
        public string Subject { get; set; } = "";

        public string Description { get; set; } = "";
    }
}
