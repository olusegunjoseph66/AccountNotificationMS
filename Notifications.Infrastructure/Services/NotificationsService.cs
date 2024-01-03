using Azure.Core;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Notifications.Application.Constants;
using Notifications.Application.DTOs.APIDataFormatters;
using Notifications.Application.DTOs.Events;
using Notifications.Application.DTOs.Filters;
using Notifications.Application.DTOs.Sortings;
using Notifications.Application.Enums;
using Notifications.Application.Exceptions;
using Notifications.Application.Interfaces.Services;
using Notifications.Application.ViewModels.QueryFilters;
using Notifications.Application.ViewModels.Requests;
using Notifications.Application.ViewModels.Responses;
using Notifications.Application.ViewModels.Responses.ResponseDto;
using Notifications.Infrastructure.QueryObjects;
using Shared.Data.Extensions;
using Shared.Data.Models;
using Shared.Data.Repository;
using Shared.ExternalServices.Interfaces;
using Shared.Utilities.DTO.Pagination;
using Shared.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Infrastructure.Services
{
    public class NotificationsService : BaseService, INotificationsService
    {
        private readonly IAsyncRepository<Notification> _notificationRepository;
        private readonly IAsyncRepository<Unsubscribe> _unsubsribeRepository;
        private readonly IAsyncRepository<UserNotification> _userNotificationRepository;


        public readonly IMessagingService _messageBus;
        public NotificationsService(IAuthenticatedUserService authenticatedUserService, IAsyncRepository<Unsubscribe> unsubsribeRepository, IAsyncRepository<Notification> notificationRepository, IAsyncRepository<UserNotification> userNotificationRepository , IMessagingService messageBus) : base(authenticatedUserService)
        {
            _unsubsribeRepository = unsubsribeRepository;
            _notificationRepository = notificationRepository;
            _messageBus = messageBus;
            _userNotificationRepository = userNotificationRepository;
        }

        public async Task<ApiResponse> SaveNotificationSettings(IEnumerable<NoticationSettingsSaveRequest> noticationSettings, CancellationToken cancellationToken = default)
        {
            GetUserId();

            var selectedUnsubsubscribes = await _unsubsribeRepository.Table.Where(p => p.UserId == LoggedInUserId).ToListAsync(cancellationToken);

            if (selectedUnsubsubscribes.Any())
                await _unsubsribeRepository.DeleteRange(selectedUnsubsubscribes);

            List<Unsubscribe> unsubscribes = new();
            noticationSettings.ForEach(x =>
            {
                if (x.Subscribed == false)
                {
                    Unsubscribe unsubscribe = new()
                    {
                        DateUnsubscribed = DateTime.UtcNow,
                        NotificationId = x.NotificationId,
                        UserId = LoggedInUserId
                    };
                    unsubscribes.Add(unsubscribe);
                }
            });
            await _unsubsribeRepository.AddRangeAsync(unsubscribes, cancellationToken);
            await _unsubsribeRepository.CommitAsync(cancellationToken);

            var messageObject = new NotificationPublishMessage
            {
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                UserId = LoggedInUserId
            };
            await _messageBus.PublishTopicMessage(messageObject, EventMessages.NOTIFICATION_SETTINGS_UPDATED);

            return ResponseHandler.SuccessResponse(SuccessMessages.SUCCESSFUL_NOTIFICATION_SETTINGS_ADDITION);
        }

        public async Task<ApiResponse> GetMyNotificationSettings(CancellationToken cancellationToken)
        {
            GetUserId();

            var notifications = await _notificationRepository.Table.Where(p => p.IsDistributorNotification).Select(x => new Notification
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync(cancellationToken);

            var unsubscribedNotifications = await _unsubsribeRepository.Table.Where(p => p.UserId == LoggedInUserId).Select(x => new Unsubscribe
            {
                NotificationId = x.NotificationId
            }).ToListAsync(cancellationToken);

            List<NotificationSettingsResponseDto> settings = new();

            if (notifications.Any())
            {
                if (unsubscribedNotifications.Any())
                {
                    notifications.ForEach(x =>
                    {
                        if (unsubscribedNotifications.Any(u => u.NotificationId == x.Id))
                            settings.Add(new NotificationSettingsResponseDto
                            {
                                Name = x.Name,
                                NotificationId = x.Id,
                                Subscribed = false
                            });
                        else
                            settings.Add(new NotificationSettingsResponseDto
                            {
                                Name = x.Name,
                                NotificationId = x.Id,
                                Subscribed = true
                            });
                    }); 
                }
                else
                {
                    notifications.ForEach(x =>
                    {
                        settings.Add(new NotificationSettingsResponseDto
                        {
                            Name = x.Name,
                            NotificationId = x.Id,
                            Subscribed = true
                        });
                    });
                }
            }

            var response = new NotificationSettingsResponse(settings);
            return ResponseHandler.SuccessResponse(SuccessMessages.SUCCESSFUL_DEFAULT, response);
        }

        public async  Task<ApiResponse> GetMyNotifications(NotificationQueryFilter filter, CancellationToken cancellationToken)
        {
            GetUserId();

            BasePageFilter pageFilter = new(filter.PageSize, filter.PageIndex);

            NotificationFilterDto notificationFilter = new()
            {
                ReadStatus = filter.ReadStatus,
                UserId = LoggedInUserId
            };

            var expression = new NotificationQueryObject(notificationFilter).Expression;
            var orderExpression = ProcessOrderFunc();
            var query = _userNotificationRepository.Table.AsNoTrackingWithIdentityResolution().Select(x => new UserNotification
            {
                Id = x.Id,
                DateCreated = x.DateCreated,
                NotificationMessage = x.NotificationMessage,
                ReadStatus = x.ReadStatus, 
                UserId = x.UserId
            }).OrderByWhere(expression, orderExpression);

            var totalCount = await query.CountAsync(cancellationToken);
            var queryResult = query.Paginate(pageFilter.PageNumber, pageFilter.PageSize);
            var userNotifications = await queryResult.ToListAsync(cancellationToken);
            var totalPages = NumberManipulator.PageCountConverter(totalCount, pageFilter.PageSize);
            var response = new PaginatedList<UserNotificationResponseDto>(ProcessQuery(userNotifications), new PaginationMetaData(filter.PageIndex, filter.PageSize, totalPages, totalCount));

            return ResponseHandler.SuccessResponse(SuccessMessages.SUCCESSFUL_DEFAULT, response);
        }

        public async Task<ApiResponse> AcknowledgeNotification(AcknowledgeNotificationRequest request, CancellationToken cancellationToken)
        {
            var userNotification = await _userNotificationRepository.Table.Where(x => x.Id == request.UserNotificationId && x.UserId == LoggedInUserId).FirstOrDefaultAsync(cancellationToken);

            if (userNotification == null)
                throw new ValidationException();

            userNotification.ReadStatus = true;
            userNotification.DateRead = DateTime.UtcNow;
            _userNotificationRepository.Update(userNotification);

            return ResponseHandler.SuccessResponse(SuccessMessages.SUCCESSFUL_NOTIFICATION_ACKNOWLEDGEMENT);
        }

        #region Private Methods
        private static Func<IQueryable<UserNotification>, IOrderedQueryable<UserNotification>> ProcessOrderFunc()
        {
            static IOrderedQueryable<UserNotification> orderFunction(IQueryable<UserNotification> queryable)
            {
               return queryable.OrderByDescending(p => p.DateCreated);
            }
            return orderFunction;
        }

        private static IReadOnlyList<UserNotificationResponseDto> ProcessQuery(IReadOnlyList<UserNotification> userNotifications)
        {
            return userNotifications.Select(p =>
            {
                var item = new UserNotificationResponseDto
                {
                    DateCreated = p.DateCreated,
                    ReadStatus = p.ReadStatus,
                    NotificationMessage = p.NotificationMessage,
                    UserNotificationId = p.Id
                };
                return item;
            }).ToList();
        }
        #endregion
    }
}
