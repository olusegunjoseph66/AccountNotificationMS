namespace Notifications.Application.DTOs.Events
{
    public class DmsOrderItemMessage
    {
        public int DmsOrderItemId { get; set; }
        public ProductMessage Product { get; set; } = new ProductMessage();
        public decimal Quantity { get; set; }
        public string SalesUnitOfMeasureCode { get; set; } = "";
        public DateTime DateCreated { get; set; }
    }
}