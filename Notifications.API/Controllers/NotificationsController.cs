using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notifications.Application.DTOs.APIDataFormatters;
using Notifications.Application.Interfaces.Services;
using Notifications.Application.ViewModels.QueryFilters;
using Notifications.Application.ViewModels.Requests;
using Notifications.Application.ViewModels.Responses;
using Notifications.Application.ViewModels.Responses.ResponseDto;
using Shared.Utilities.DTO.Pagination;

namespace Notifications.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;
        public NotificationsController(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

    }
}
