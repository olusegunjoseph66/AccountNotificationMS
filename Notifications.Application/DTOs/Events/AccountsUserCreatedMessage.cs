using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class AccountsUserCreatedMessage
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string EmailAddress { get; set; } = "";

        public string PhoneNumber { get; set; } = "";

        public string Username { get; set; } = "";
        public string Password { get; set; }

        public DateTime DateCreated { get; set; }

        public string DeviceId { get; set; } = "";

        public AccountStatusMessage AccountStatus = new();

        public List<string> Roles { get; set; } = new List<string>();

        public string ChannelCode { get; set; } = "";



    }
}
