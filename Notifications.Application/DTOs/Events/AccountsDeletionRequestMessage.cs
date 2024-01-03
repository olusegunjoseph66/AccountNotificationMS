using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class AccountsDeletionRequestMessage
    {
        public int DeletionRequestId { get; set; }
        public DateTime DateCreated { get; set; }

        public int UserId { get; set; }

        public string Reason { get; set; } = "";
    }
}
