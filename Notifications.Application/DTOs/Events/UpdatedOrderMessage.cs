using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class UpdatedOrderMessage
    {
        public int DmsOrderId { get; set; }

        public DateTime? DateModified { get; set; }

        public int? ModifiedByUserId { get; set; }

        public int UserId { get; set; }
        public string CompanyCode { get; set; } = "";

        public string CountryCode { get; set; } = "";
        public string OrderSapNumber { get; set; } = "";

        public DistributorSapAccount DistributorSapAccount { get; set; } = new DistributorSapAccount();

        public decimal EstimatedNetValue { get; set; }

        public OrderGenericStatus OldOrderStatus { get; set; } = new OrderGenericStatus();

        public OrderGenericStatus NewOrderStatus { get; set; } = new OrderGenericStatus();

        public OrderType OrderType { get; set; } = new OrderType();

        public string OldTruckSizeCode { get; set; } = "";

        public string NewTruckSizeCode { get; set; } = "";

        public string OldDeliveryMethodCode { get; set; } = "";
        public string NewDeliveryMethodCode { get; set; } = "";

        public Plant OldPlant { get; set; } = new Plant();

        public Plant NewPlant { get; set; } = new Plant();

        public DateTime? OldDeliveryDate { get; set; }

        public DateTime? NewDeliveryDate { get; set; }

        public string OldDeliveryAddress { get; set; } = "";

        public string NewDeliveryAddress { get; set; } = "";

        public string OldDeliveryCity { get; set; } = "";

        public string NewDeliveryCity { get; set; } = "";

        public string OldDeliveryStateCode { get; set; } = "";

        public string NewDeliveryStateCode { get; set; } = "";

        public string OldDeliveryCountryCode { get; set; } = "";

        public string NewDeliveryCountryCode { get; set; } = "";

        public DmsOrderItems DmsOrderItems { get; set; } = new DmsOrderItems();
        
        
    }
}
