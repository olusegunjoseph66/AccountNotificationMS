using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class DmsOrderAtcAvailableMessage
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public int DmsOrderId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModifed { get; set; }
        public string OrderSapNumber { get; set; }
        public string CompanyCode { get; set; }
        public string CountryCode { get; set; }
        public decimal EstimatedNetValue { get; set; }
        public decimal? OrderSapNetValue { get; set; }
        public int NumberOfChildAtc { get; set; }
        public OrderType OrderType { get; set; }
        public DistributorSapAccount DistributorSapAccount { get; set; }
        public OrderGenericStatus NewOrderStatus { get; set; }
    }
}
