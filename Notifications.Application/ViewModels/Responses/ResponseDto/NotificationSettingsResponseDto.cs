namespace Notifications.Application.ViewModels.Responses.ResponseDto
{
    public class NotificationSettingsResponseDto
    {
        public int NotificationId { get; set; }

        public string Name { get; set; } 

        public bool Subscribed { get; set; }
    }
}