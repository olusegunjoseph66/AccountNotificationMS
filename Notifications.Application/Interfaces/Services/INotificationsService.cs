using Notifications.Application.DTOs.APIDataFormatters;
using Notifications.Application.ViewModels.QueryFilters;
using Notifications.Application.ViewModels.Requests;
using Notifications.Application.ViewModels.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.Interfaces.Services
{
    public interface INotificationsService
    {
        Task<ApiResponse> SaveNotificationSettings(IEnumerable<NoticationSettingsSaveRequest> noticationSettings, CancellationToken cancellationToken = default);
        Task<ApiResponse> GetMyNotificationSettings(CancellationToken cancellationToken);
        Task<ApiResponse> GetMyNotifications(NotificationQueryFilter filter, CancellationToken cancellationToken);
        Task<ApiResponse> AcknowledgeNotification(AcknowledgeNotificationRequest request, CancellationToken cancellationToken);
    }
}
