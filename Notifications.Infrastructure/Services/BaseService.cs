using Notifications.Application.Interfaces.Services;


namespace Notifications.Infrastructure.Services
{
    public class BaseService
    {
        internal readonly IAuthenticatedUserService _authenticatedUserService;

        public BaseService(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        public int LoggedInUserId { get; set; }

        public void GetUserId()
        {
            if (_authenticatedUserService.UserId == 0) throw new UnauthorizedAccessException($"Access Denied. Kindly login to continue with this request.");
            LoggedInUserId = _authenticatedUserService.UserId;
        }
    }
}
