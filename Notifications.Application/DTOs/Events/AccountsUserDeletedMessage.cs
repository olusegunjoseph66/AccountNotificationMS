using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class AccountsUserDeletedMessage
    {
        public int UserId { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeletedByUserId { get; set; }
    }
}
