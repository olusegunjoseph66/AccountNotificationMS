namespace Notifications.Application.DTOs.Events
{
    public class DmsOrderItems
    {
        public List<DmsOrderItemMessage> Items { get; set; } = new List<DmsOrderItemMessage>();
    }
}