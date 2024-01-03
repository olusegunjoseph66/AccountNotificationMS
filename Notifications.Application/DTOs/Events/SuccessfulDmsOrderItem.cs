namespace Notifications.Application.DTOs.Events
{
    public class SuccessfulDmsOrderItem
    {
        public int DmsOrderItemId { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string OrderItemSapNumber { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public ProductMessage Product { get; set; } = new ProductMessage();

        public decimal Quantity { get; set; }

        public string SalesUnitOfMeasureCode { get; set; } = "";
    }
}