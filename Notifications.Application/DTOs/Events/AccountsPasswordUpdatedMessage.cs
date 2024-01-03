using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class AccountsPasswordUpdatedMessage
    {
        public int UserId { get; set; }
        public DateTime? DateModified { get; set; }

        public int? ModifiedByUserId { get; set; }

        public string ChannelCode { get; set; } = "";
    }
}
