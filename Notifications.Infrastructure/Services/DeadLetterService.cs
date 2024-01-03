using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Data.Models;
using Shared.Data.Repository;
using Shared.ExternalServices.APIServices;
using Shared.ExternalServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Notifications.Application.Constants;
using Notifications.Application.DTOs;
using Notifications.Application.DTOs.APIDataFormatters;
using Notifications.Application.DTOs.Events;
using Notifications.Application.Exceptions;
using Notifications.Application.Interfaces.Services;
using Notifications.Application.ViewModels.Responses;
using Azure;
using Notifications.Application.Enums;
using Shared.Utilities.Helpers;

namespace Notifications.Infrastructure.Services
{
    public class DeadLetterService : IDeadLetterService
    {
        private readonly IAsyncRepository<User> _userRepository;
        private readonly ILogger<DeadLetterService> _notificationLogger;
        public readonly IServiceScopeFactory _scopeFactory;
        public readonly IMessagingService _messageBus;

        public DeadLetterService(
            IAsyncRepository<User> userRepository, IServiceScopeFactory scopeFactory, IMessagingService messageBus, ILogger<DeadLetterService> notificationLogger)
        {
            _userRepository = userRepository;
            _notificationLogger = notificationLogger;

            _messageBus = messageBus;
            _scopeFactory = scopeFactory;
        }

        public async Task<ApiResponse> ProcessAccountDeadLetterMessages(CancellationToken cancellationToken = default)
        {
            _notificationLogger.LogInformation("About to start dead-letter service");
            var messages = await _messageBus.GetDeadLetterMessages(EventMessages.ACCOUNT_TOPIC, EventMessagesSubscription.NOTIFICATIONS);

            int processedMessagesCount = 0;

            if (messages == null)
                _notificationLogger.LogInformation($"No dead-letter service messages found.");
            else
            {
                _notificationLogger.LogInformation($"{messages.Count} dead-letter service messages found.");

                var validMessages = messages.Where(x => x.Label.ToLower() == EventMessages.ACCOUNTS_USER_CREATED.ToLower() || x.Label.ToLower() == EventMessages.ACCOUNTS_USER_UPDATED.ToLower()).ToList();

                if (validMessages.Any())
                {
                    List<DeadLetterDto> queues = new();

                    for (int count = 0; count < validMessages.Count; count++)
                    {
                        var body = Encoding.UTF8.GetString(validMessages[count].Body);
                        if (validMessages[count].Label.ToLower() == EventMessages.ACCOUNTS_USER_CREATED.ToLower())
                        {
                            var userAccount = JsonConvert.DeserializeObject<AccountsUserCreatedMessage>(body);

                            queues.Add(new DeadLetterDto
                            {
                                Message = validMessages[count],
                                UserId = userAccount.UserId,
                                Body = (object)userAccount,
                                EventMessages = EventMessages.ACCOUNTS_USER_CREATED.ToLower()
                            });
                        }
                        else if (validMessages[count].Label.ToLower() == EventMessages.ACCOUNTS_USER_UPDATED.ToLower())
                        {
                            var userAccount = JsonConvert.DeserializeObject<AccountsUserUpdatedMessage>(body);

                            queues.Add(new DeadLetterDto
                            {
                                Message = validMessages[count],
                                UserId = userAccount.UserId,
                                Body = (object)userAccount,
                                EventMessages = EventMessages.ACCOUNTS_USER_UPDATED.ToLower()
                            });
                        }
                    }

                    var userAccountIds = queues.Select(x => x.UserId).ToList();

                    var userAccounts = await _userRepository.Table.Where(x => userAccountIds.Contains(x.UserId)).ToListAsync(cancellationToken: cancellationToken);

                    List<User> newUserAccounts = new();
                    List<User> updatedUserAccounts = new();
                    List<string> messageTokens = new();

                    for (int count = 0; count < queues.Count; count++)
                    {
                        if (queues[count].EventMessages == EventMessages.ACCOUNTS_USER_CREATED.ToLower() && !userAccounts.Any(x => x.UserId == queues[count].UserId))
                        {
                            var messageBody = (AccountsUserCreatedMessage)queues[count].Body;
                            var newUserAccount = new User
                            {
                                EmailAddress = messageBody.EmailAddress,
                                FirstName = messageBody.FirstName,
                                LastName = messageBody.LastName,
                                Roles = messageBody.Roles.First(),
                                IsDeleted = false,
                                DateUpdated = messageBody.DateCreated,
                                UserId = messageBody.UserId
                            };

                            try
                            {
                                await _userRepository.AddAsync(newUserAccount, cancellationToken);
                                await _userRepository.CommitAsync(cancellationToken);
                                await _messageBus.DeleteDeadLetterMessage(EventMessages.ACCOUNT_TOPIC, EventMessagesSubscription.NOTIFICATIONS, queues[count].Message.SystemProperties.LockToken).ConfigureAwait(false);
                                processedMessagesCount++;
                            }
                            catch (Exception ex)
                            {
                                _notificationLogger.LogError(ex, $"Account-Notification Dead-Letter Subscription with Message Sequence Number {queues[count].Message.SystemProperties.SequenceNumber} failed to update.");
                            }
                        }
                        else if (queues[count].EventMessages == EventMessages.ACCOUNTS_USER_CREATED.ToLower() && userAccounts.Any(x => x.UserId == queues[count].UserId))
                        {
                            await _messageBus.DeleteDeadLetterMessage(EventMessages.ACCOUNT_TOPIC, EventMessagesSubscription.NOTIFICATIONS, queues[count].Message.SystemProperties.LockToken).ConfigureAwait(false);
                            processedMessagesCount++;
                        }
                        else if (queues[count].EventMessages == EventMessages.ACCOUNTS_USER_UPDATED.ToLower() && userAccounts.Any(x => x.UserId == queues[count].UserId))
                        {
                            var messageBody = (AccountsUserUpdatedMessage)queues[count].Body;
                            var userAccount = userAccounts.FirstOrDefault(x => x.UserId == queues[count].UserId);

                            userAccount.EmailAddress = messageBody.NewEmailAddress;
                            userAccount.FirstName = messageBody.NewFirstName;
                            userAccount.LastName = messageBody.NewLastName;
                            if (messageBody.DateModified.HasValue)
                                userAccount.DateUpdated = messageBody.DateModified.Value;
                            //updatedUserAccounts.Add(userAccount);

                            try
                            {
                                _userRepository.Update(userAccount);
                                await _userRepository.CommitAsync(cancellationToken);
                                await _messageBus.DeleteDeadLetterMessage(EventMessages.ACCOUNT_TOPIC, EventMessagesSubscription.NOTIFICATIONS, queues[count].Message.SystemProperties.LockToken).ConfigureAwait(false);
                                processedMessagesCount++;
                            }
                            catch (Exception ex)
                            {
                                _notificationLogger.LogError(ex, $"Account-Notification Dead-Letter Subscription with Message Sequence Number {queues[count].Message.SystemProperties.SequenceNumber} failed to update.");
                            }

                            //messageTokens.Add(queues[count].Message.SystemProperties.LockToken);
                        }
                    }
                }
            }

            _notificationLogger.LogInformation($"dead-letter service ended with {processedMessagesCount} processed messages.");

            return await Task.FromResult(new ApiResponse(SuccessCodes.DEFAULT_SUCCESS_CODE, ResponseStatusEnum.Successful.ToDescription(), SuccessMessages.SUCCESSFUL_OPERATION));
        }

    }
}
