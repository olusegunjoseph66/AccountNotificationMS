using Notifications.Application.ViewModels.Responses.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.ViewModels.Responses
{
    public record NotificationSettingsResponse(List<NotificationSettingsResponseDto> NotificationSettings);
}
