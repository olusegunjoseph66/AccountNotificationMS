using Notifications.Application.Constants;
using Shared.ExternalServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class DmsOrderCreatedMessage : IntegrationBaseMessage
    {
        public int DmsOrderId { get; set; }
        public int CreatedByUserId { get; set; }
        public int UserId { get; set; }
        public string CompanyCode { get; set; }
        public string CountryCode { get; set; }
        public DistributorSapAccountMessage DistributorSapAccount { get; set; }
        public decimal EstimatedNetValue { get; set; }
        public string SalesUnitOfMeasureCode { get; set; }
        public Status OrderStatus { get; set; }
        public Status OrderType { get; set; }
        public string ChannelCode { get; set; }
        public List<DmsOrderItemMessage> DmsOrderItems { get; set; }
    }
}
