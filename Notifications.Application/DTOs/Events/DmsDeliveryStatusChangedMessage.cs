using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class DmsDeliveryStatusChangedMessage
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public int DmsOrderId { get; set; }
        public string OrderSapNumber { get; set; }
        public string ParentOrderSapNumber { get; set; }
        public string CompanyCode { get; set; }
        public string CountryCode { get; set; }
        public DistributorSapAccount DistributorSapAccount { get; set; }
        public decimal EstimatedNetValue { get; set; }
        public decimal? OrderSapNetValue { get; set; }
        public OrderGenericStatus OrderStatus { get; set; }
        public OrderType OrderType { get; set; }
        public OrderGenericStatus OldDeliveryStatus { get; set; }
        public OrderGenericStatus NewDeliveryStatus { get; set; }
    }
}
