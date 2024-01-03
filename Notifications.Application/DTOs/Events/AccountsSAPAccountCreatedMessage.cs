using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
   public class AccountsSAPAccountCreatedMessage
    {
        public int DistributorSapAccountId { get; set; }
        public int UserId { get; set; }
        public string DistributorSapNumber { get; set; } = "";
        public DateTime DateCreated { get; set; }

        public string CompanyCode { get; set; } = "";

        public string CountryCode { get; set; } = "";

        public string DistributorName { get; set; } = "";

        public string FriendlyName { get; set; } = "";
        public bool? IsMessageRequired { get; set; }

        public SapAccountType AccountType { get; set; } = new SapAccountType();


    }
}
