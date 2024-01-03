namespace Notifications.Application.ViewModels.Responses.ResponseDto
{
    public class UserNotificationResponseDto
    {
        public int UserNotificationId { get; set; }
        public DateTime DateCreated { get; set; }
        public string NotificationMessage { get; set; } 
        public bool ReadStatus { get; set; }

    }
}