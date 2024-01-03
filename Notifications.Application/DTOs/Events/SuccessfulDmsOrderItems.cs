namespace Notifications.Application.DTOs.Events
{
    public class SuccessfulDmsOrderItems
    {
        public List<SuccessfulDmsOrderItem> items { get; set; } = new List<SuccessfulDmsOrderItem>();
    }
}