using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class OrderSubmissionFailedMessage
    {
        public int DmsOrderId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? DateSubmittedToDMS { get; set; }

        public int UserId { get; set; }

        public string CompanyCode { get; set; } = "";

        public string CountryCode { get; set; } = "";

        public DistributorSapAccount DistributorSapAccount { get; set; } = new DistributorSapAccount();

        public decimal EstimatedNetValue { get; set; }

        public OrderGenericStatus OrderStatus { get; set; } = new OrderGenericStatus();

        public OrderType OrderType { get; set; } = new OrderType();

        public string TruckSizeCode { get; set; } = "";

        public string DeliveryMethodCode { get; set; } = "";

        public Plant Plant { get; set; } = new Plant();

        public DateTime? DeliveryDate { get; set; }

        public string DeliveryAddress { get; set; } = "";

        public string DeliveryCity { get; set; } = "";

        public string DeliveryStateCode { get; set; } = "";

        public string DeliveryCountryCode { get; set; } = "";

        public int NumberOfSubmissionAttempts { get; set; }

        public DmsOrderItems DmsOrderItems { get; set; } = new DmsOrderItems();

        


    }
}
