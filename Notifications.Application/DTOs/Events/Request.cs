namespace Notifications.Application.DTOs.Events
{
    public class Request
    {
        public int UserId { get; set; }
        public int RequestId { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }
    }
}