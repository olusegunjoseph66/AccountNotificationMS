using Notifications.Application.DTOs.APIDataFormatters;

namespace Notifications.Application.Interfaces.Services
{
    public interface IDeadLetterService
    {
        Task<ApiResponse> ProcessAccountDeadLetterMessages(CancellationToken cancellationToken = default);
    }
}