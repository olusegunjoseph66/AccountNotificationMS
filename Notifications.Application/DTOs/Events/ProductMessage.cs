namespace Notifications.Application.DTOs.Events
{
    public class ProductMessage
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
    }
}