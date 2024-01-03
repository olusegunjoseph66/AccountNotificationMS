﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class TripStatusChangedMessage
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
        public OrderType OrderType { get; set; }
        public DistributorSapAccount DistributorSapAccount { get; set; }
        public OrderGenericStatus OrderStatus { get; set; }
        public OrderGenericStatus DeliveryStatus { get; set; }
        public OrderGenericStatus OldTripStatus { get; set; }
        public OrderGenericStatus NewTripStatus { get; set; }
    }
}
