using Azure.Core;
using Azure.Messaging.ServiceBus;
using KissLog;
using KissLog.RestClient.Requests.CreateRequestLog;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Notifications.Application.Configurations;
using Notifications.Application.Constants;
using Notifications.Application.Constants.NotificationMessages;
using Notifications.Application.DTOs;
using Notifications.Application.DTOs.Events;
using Notifications.Application.Interfaces.Services;
using Notifications.Application.Interfaces.Messaging;
using Shared.Data.Models;
using Shared.Data.Repository;
using Shared.ExternalServices.Configurations;
using Shared.ExternalServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;
using static System.Net.WebRequestMethods;
using User = Shared.Data.Models.User;
using Shared.Utilities.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Notifications.Infrastructure.Services.Messaging
{
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
    public class AzureServiceBusConsumer:BaseService, IAzureServiceBusConsumer
    {
        private readonly ServiceBusProcessor accountProcessor;

        public readonly IMessagingService _messageBus;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        public readonly IServiceScopeFactory _scopeFactory;
        public readonly IServiceScopeFactory _accountScopeFactory;

        public IAsyncRepository<Notification> _accountNotificationRepository;
        public IAsyncRepository<UserNotification> _accountUserNotificationRepository;
        public IAsyncRepository<User> _accountUserRepository;
        private IAsyncRepository<Unsubscribe> _accountUnsubscribeRepository;

        private readonly IConfiguration _configuration;


        public AzureServiceBusConsumer(IAuthenticatedUserService authenticatedUserService, IConfiguration configuration, IMessagingService messageBus, IEmailService emailService, ISmsService smsService, IServiceScopeFactory scopeFactory, IServiceScopeFactory accountScopeFactory, 

            IAsyncRepository<Notification> accountNotificationRepository,
            IAsyncRepository<UserNotification> accountUserNotificationRepository,            
            IAsyncRepository<User> accountUserRepository, 
            
            IAsyncRepository<Unsubscribe> accountUnsubscribeRepository, ILogger<AzureServiceBusConsumer> logger) : base(authenticatedUserService)
        {
            _messageBus = messageBus;
            _emailService = emailService;
            _smsService = smsService;
            _configuration = configuration;

            accountProcessor = _messageBus.ConsumeMessage(EventMessages.ACCOUNT_TOPIC, EventMessagesSubscription.NOTIFICATIONS);

            _scopeFactory = scopeFactory;
            _accountScopeFactory = accountScopeFactory;

            _accountNotificationRepository = accountNotificationRepository;

            _accountUserNotificationRepository = accountUserNotificationRepository;

            _accountUserRepository = accountUserRepository;
            _accountUnsubscribeRepository = accountUnsubscribeRepository;
        }

        #region Event Triggers
        
        public async Task StartAccountMsg()
        {
            var kissLogger = new Logger(url: "account/notification/start");
            kissLogger.Info("SUBSCRIBER- ACCOUNT NOTIFICATION: This is the starting point of the Account event subscriptions.");
            Logger.NotifyListeners(kissLogger);

            accountProcessor.ProcessMessageAsync += OnAutoSendAccountNotification;
            accountProcessor.ProcessErrorAsync += ErrorHanler;
            await accountProcessor.StartProcessingAsync();
        }
        
        Task ErrorHanler(ProcessErrorEventArgs args)
        {
            var kissLogger = new Logger(url: "account/notification");
            kissLogger.Error(args.Exception, "SUBSCRIBER- ACCOUNT NOTIFICATION: Sorry, the following issue occurred.");
            Logger.NotifyListeners(kissLogger);

            return Task.CompletedTask;
        }
        #endregion

        #region Actions
        private async Task OnAutoSendAccountNotification(ProcessMessageEventArgs args)
        {
            var kissLogger = new Logger(url: "account/notification");
            try
            {
                if (args.Message.DeliveryCount > 0)
                {
                    kissLogger.Info($"SUBSCRIBER- ACCOUNT NOTIFICATION: Entry-- {args.Message.Subject} ---The message with the MessageId: {args.Message.MessageId},Subscription Number:  {args.Message.SequenceNumber}, Subject: {args.Message.Subject}, at Enqueued Time {args.Message.EnqueuedTime}, being read now at {DateTimeOffset.UtcNow}.");
                }

                var subject = args.Message.Subject;
                using var scope = _accountScopeFactory.CreateScope();

                _accountUserRepository = scope.ServiceProvider.GetRequiredService<IAsyncRepository<User>>();
                _accountUnsubscribeRepository = scope.ServiceProvider.GetRequiredService<IAsyncRepository<Unsubscribe>>();

                if (subject.ToLower() == EventMessages.ACCOUNTS_USER_UPDATED.ToLower())
                {
                    var message = args.Message;
                    string updatedAccount = Encoding.UTF8.GetString(message.Body);
                    var updatedUser = JsonConvert.DeserializeObject<AccountsUserUpdatedMessage>(updatedAccount);
                    if (updatedUser != null)
                    {
                        User? user = await _accountUserRepository.Table.FirstOrDefaultAsync(p => p.UserId == updatedUser.UserId);
                        if (user != null)
                        {
                            user.DateUpdated = DateTime.UtcNow;
                            user.DeviceId = updatedUser.NewDeviceId;
                            user.EmailAddress = updatedUser.NewEmailAddress;
                            user.IsDeleted = false;
                            user.FirstName = updatedUser.NewFirstName;
                            user.LastName = updatedUser.NewLastName;
                            user.PhoneNumber = updatedUser.NewPhoneNumber;
                            _accountUserRepository.Update(user);
                            await _accountUserRepository.CommitAsync();

                            await args.CompleteMessageAsync(args.Message);
                        }
                    }

                }
                else if (subject.ToLower() == EventMessages.ACCOUNTS_USER_DELETED.ToLower())
                {
                    var message = args.Message;
                    string deletedAccount = Encoding.UTF8.GetString(message.Body);
                    var deletedUser = JsonConvert.DeserializeObject<AccountsUserDeletedMessage>(deletedAccount);
                    if (deletedUser != null)
                    {
                        User? user = await _accountUserRepository.Table.FirstOrDefaultAsync(p => p.UserId == deletedUser.UserId);
                        if (user != null)
                        {
                            user.IsDeleted = true;
                            user.DateUpdated = DateTime.UtcNow;
                            _accountUserRepository.Update(user);
                            await _accountUserRepository.CommitAsync();

                            await args.CompleteMessageAsync(args.Message);
                        }
                    }

                }
                else if (subject.ToLower() == EventMessages.ACCOUNTS_USER_LOGIN.ToLower())
                {
                    await args.CompleteMessageAsync(args.Message);
                }
                else
                {
                    _accountNotificationRepository = scope.ServiceProvider.GetRequiredService<IAsyncRepository<Notification>>();
                    _accountUserNotificationRepository = scope.ServiceProvider.GetRequiredService<IAsyncRepository<UserNotification>>();

                    var users = new List<Shared.Data.Models.User>();
                    var notifications = await _accountNotificationRepository.Table.Where(p => p.EventTriggerName.ToLower().Contains(EventMessages.ACCOUNT_TOPIC)).ToListAsync();

                    List<Dictionary<string, string>> placeholderValues = new();
                    List<PlaceholderArray> placeholderArray = new();
                    Dictionary<string, string> smsPlaceholders = new();
                    string messageKey = "";

                    Notification? notification = new();
                    bool isNotificationAllowed = false;
                    bool isSmsNotificationAllowed = false;
                    bool isEmailNotificationAllowed = false;
                    bool isOtpNotification = false;

                    if (subject.ToLower() == EventMessages.ACCOUNTS_USER_CREATED.ToLower())
                    {
                        var message = args.Message;
                        var body = Encoding.UTF8.GetString(message.Body);
                        var account = JsonConvert.DeserializeObject<AccountsUserCreatedMessage>(body);

                        if (account is not null)
                        {
                            var existingUser = await _accountUserRepository.Table.FirstOrDefaultAsync(x => x.UserId == account.UserId);
                            if (existingUser is null)
                            {
                                var user = new User
                                {
                                    DateUpdated = DateTime.UtcNow,
                                    IsDeleted = false,
                                    FirstName = account.FirstName,
                                    LastName = account.LastName,
                                    UserId = account.UserId,
                                    EmailAddress = account.EmailAddress,
                                    PhoneNumber = account.PhoneNumber,
                                    DeviceId = account.DeviceId
                                };

                                if (account.Roles.Any())
                                {
                                    user.Roles = account.Roles[0];
                                }
                                await _accountUserRepository.AddAsync(user);
                                await _accountUserRepository.CommitAsync();

                                users.Add(user);
                            }
                            else
                                users.Add(existingUser);

                            if (account.Roles.Contains("Distributor"))
                            {
                                notification = notifications.FirstOrDefault(p => p.EventTriggerName.ToLower() == subject.ToLower() && p.IsDistributorNotification);
                                messageKey = NotificationMessagesKey.DISTRIBUTOR_WELCOME_NOTIFICATION;

                                placeholderValues = new()
                            {
                                new Dictionary<string, string>
                                {
                                    { "firstName", account.FirstName },
                                    { "recipientEmail", account.EmailAddress }
                                }
                            };
                            }
                            else
                            {
                                notification = notifications.FirstOrDefault(p => p.EventTriggerName == subject && !p.IsDistributorNotification);
                                messageKey = NotificationMessagesKey.ADMIN_WELCOME_NOTIFICATION;

                                placeholderValues = new()
                            {
                                new Dictionary<string, string>
                                {
                                    { "firstName", account.FirstName },
                                    { "userName", account.Username },
                                    { "password", account.Password },
                                    { "adminPortalUrl", "https://admin.dms.com" },
                                    { "recipientEmail", account.EmailAddress }
                                }
                            };
                            }

                            isNotificationAllowed = true;
                            isEmailNotificationAllowed = true;
                        }
                    }
                    else if (subject.ToLower() == EventMessages.ACCOUNTS_OTP_GENERATED.ToLower())
                    {

                        var message = args.Message;
                        var body = Encoding.UTF8.GetString(message.Body);
                        var otp = JsonConvert.DeserializeObject<OtpGeneratedMessage>(body);

                        if (otp != null)
                        {
                            notification = notifications.FirstOrDefault(p => p.EventTriggerName.ToLower() == subject.ToLower());
                            users.Add(new User { EmailAddress = otp.EmailAddress, PhoneNumber = otp.PhoneNumber });

                            placeholderValues = new()
                        {
                            new Dictionary<string, string>
                            {
                                { "otpCode", otp.OtpCode },
                                { "dateExpiry", otp.DateExpiry.ConvertToLocal().ToString() },
                                { "recipientEmail", otp.EmailAddress }
                            }
                        };

                            smsPlaceholders = new()
                        {
                            { "{otpCode}", otp.OtpCode }
                        };

                            isNotificationAllowed = true;
                            isOtpNotification = true;
                            isEmailNotificationAllowed = true;
                            isSmsNotificationAllowed = true;
                        }
                    }
                    else if (subject.ToLower() == EventMessages.ACCOUNTS_PASSWORD_UPDATED)
                    {
                        var message = args.Message;
                        var body = Encoding.UTF8.GetString(message.Body);

                        messageKey = NotificationMessagesKey.PASSWORD_CHANGE_CONFIRMATION_NOTIFICATION;

                        var passwordUpdateAccount = JsonConvert.DeserializeObject<AccountsPasswordUpdatedMessage>(body);
                        if (passwordUpdateAccount is not null)
                        {
                            User? user = await _accountUserRepository.Table.FirstOrDefaultAsync(p => p.UserId == passwordUpdateAccount.UserId);
                            notification = notifications.FirstOrDefault(p => p.EventTriggerName.ToLower() == subject.ToLower());

                            if (notification is not null && user is not null)
                            {
                                List<Unsubscribe> unsubscribeEntry = await _accountUnsubscribeRepository.Table.Where(p => p.UserId == user.UserId && p.NotificationId == notification.Id).ToListAsync();

                                if (!unsubscribeEntry.Any())
                                {
                                    if (user != null)
                                    {
                                        if (!string.IsNullOrEmpty(user.EmailAddress))
                                        {
                                            users.Add(user);
                                            placeholderValues = new()
                                        {
                                            new Dictionary<string, string>
                                            {
                                                { "recipientEmail", user.EmailAddress }
                                            }
                                        };
                                            isNotificationAllowed = true;
                                            isEmailNotificationAllowed = true;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else if (subject.ToLower() == EventMessages.ACCOUNTS_SAPACCOUNT_CREATED.ToLower())
                    {
                        messageKey = NotificationMessagesKey.ACCOUNT_LINKED_NOTIFICATION;
                        var message = args.Message;
                        var body = Encoding.UTF8.GetString(message.Body);
                        var account = JsonConvert.DeserializeObject<AccountsSAPAccountCreatedMessage>(body);

                        if (account is not null)
                        {
                            if (account.IsMessageRequired.HasValue && !account.IsMessageRequired.Value)
                            {
                                await args.CompleteMessageAsync(args.Message);
                            }
                            else
                            {
                                User? user = await _accountUserRepository.Table.FirstOrDefaultAsync(p => p.UserId == account.UserId);
                                notification = notifications.FirstOrDefault(p => p.EventTriggerName.ToLower() == subject.ToLower());

                                if (notification is not null && user is not null)
                                {
                                    List<Unsubscribe> unsubscribeEntry = await _accountUnsubscribeRepository.Table.Where(p => p.UserId == user.UserId && p.NotificationId == notification.Id).ToListAsync();

                                    if (!unsubscribeEntry.Any())
                                    {
                                        if (user is not null)
                                        {
                                            if (!string.IsNullOrEmpty(user.EmailAddress))
                                            {
                                                users.Add(user);
                                                placeholderValues = new()
                                        {
                                            new Dictionary<string, string>
                                            {
                                                { "firstName", user.FirstName },
                                                { "distributorSapNumber", account.DistributorSapNumber },
                                                { "recipientEmail", user.EmailAddress }
                                            }
                                        };
                                                isNotificationAllowed = true;
                                                isEmailNotificationAllowed = true;
                                            }
                                        }

                                    }
                                }
                            }
                        }

                    }
                    else if (subject.ToLower() == EventMessages.ACCOUNTS_DELETION_REQUEST_CREATED.ToLower())
                    {
                        messageKey = NotificationMessagesKey.ACCOUNT_DELETION_REQUEST_NOTIFICATION;
                        var message = args.Message;
                        var body = Encoding.UTF8.GetString(message.Body);
                        var account = JsonConvert.DeserializeObject<AccountsDeletionRequestMessage>(body);
                        var adminRoleName = "administrator";

                        var adminEmails = new List<string>();
                        var adminUserRoles = await _accountUserRepository.Table.Where(p => p.Roles.ToLower().Contains(adminRoleName)).ToListAsync();
                        if (adminUserRoles.Any())
                        {
                            notification = notifications.FirstOrDefault(p => p.EventTriggerName.ToLower() == subject.ToLower());
                            if (notification != null)
                            {
                                foreach (var admin in adminUserRoles)
                                {
                                    var unsubsubscribes = _accountUnsubscribeRepository.Table.ToList();
                                    if (!unsubsubscribes.Any(p => p.UserId == admin.UserId && p.NotificationId == notification.Id))
                                    {
                                        var adminUser = _accountUserRepository.Table.FirstOrDefault(p => p.UserId == admin.UserId);
                                        if (adminUser != null)
                                        {
                                            if (!string.IsNullOrEmpty(adminUser.EmailAddress))
                                            {
                                                users.Add(adminUser);
                                                List<Dictionary<string, string>> placeholderValue = new()
                                            {
                                                new Dictionary<string, string>
                                                {
                                                    { "recipientEmail", adminUser.EmailAddress }
                                                }
                                            };
                                                placeholderArray.Add(new PlaceholderArray
                                                {
                                                    Email = adminUser.EmailAddress,
                                                    UserId = adminUser.UserId,
                                                    PlaceholderValues = placeholderValue
                                                });
                                            }
                                        }
                                    }
                                }
                                isNotificationAllowed = true;
                                isEmailNotificationAllowed = true;
                            }
                        }
                    }
                    else
                    {
                        await args.CompleteMessageAsync(args.Message);
                    }

                    if (isNotificationAllowed)
                    {
                        if (notification != null && users.Any())
                        {
                            List<UserNotification> userNotifications = new();
                            if (isOtpNotification)
                            {
                                if (isEmailNotificationAllowed)
                                {
                                    string[] recipients = users.Select(x => x.EmailAddress).ToArray();
                                    await Task.Run(() => _emailService.SendMessage(notification.EmailTemplateId, recipients, placeholderValues)).ConfigureAwait(false);

                                    for (int i = 0; i < recipients.Length; i++)
                                    {
                                        kissLogger.Info($"SUBSCRIBER- ACCOUNT NOTIFICATION: OTP Email Sending-- Email sent to {recipients[i]} via SendGrid at {DateTimeOffset.UtcNow}.");
                                    }
                                }

                                if (isSmsNotificationAllowed && !string.IsNullOrWhiteSpace(notification.SmsMessageTemplate))
                                {
                                    string[] smsRecipients = users.Select(x => x.PhoneNumber).ToArray();
                                    string messageContent = notification.SmsMessageTemplate;
                                    foreach (var placeholder in smsPlaceholders)
                                    {
                                        messageContent = messageContent.Replace(placeholder.Key, placeholder.Value);
                                    }
                                    await Task.Run(() => _smsService.SendMessage(messageContent, smsRecipients));

                                    for (int i = 0; i < smsRecipients.Length; i++)
                                    {
                                        kissLogger.Info($"SUBSCRIBER- ACCOUNT NOTIFICATION: OTP SMS Sending-- SMS sent to {smsRecipients[i]} via Twilio at {DateTimeOffset.UtcNow}.");
                                    }
                                }

                                await args.CompleteMessageAsync(args.Message);
                            }
                            else
                            {
                                var message = AccountMessages.GetMessage(messageKey);

                                if (!string.IsNullOrWhiteSpace(message))
                                {
                                    users.ForEach(x =>
                                    {
                                        UserNotification userNotification = new()
                                        {
                                            DateCreated = DateTime.UtcNow,
                                            NotificationId = notification.Id,
                                            ReadStatus = false,
                                            NotificationMessage = message,
                                            UserId = x.UserId
                                        };
                                        userNotifications.Add(userNotification);
                                    });
                                    await _accountUserNotificationRepository.AddRangeAsync(userNotifications);
                                    await _accountUserNotificationRepository.CommitAsync();

                                    if (isEmailNotificationAllowed)
                                    {
                                        if (placeholderArray.Any())
                                        {
                                            foreach (var array in placeholderArray)
                                            {
                                                string[] recipients = new string[] { array.Email };
                                                await Task.Run(() => _emailService.SendMessage(notification.EmailTemplateId, recipients, placeholderValues)).ConfigureAwait(false);

                                                kissLogger.Info($"SUBSCRIBER- ACCOUNT NOTIFICATION: Email Sending-- Email sent to {array.Email} via SendGrid at {DateTimeOffset.UtcNow}.");
                                            };
                                        }
                                        else
                                        {
                                            string[] recipients = users.Select(x => x.EmailAddress).ToArray();
                                            await Task.Run(() => _emailService.SendMessage(notification.EmailTemplateId, recipients, placeholderValues)).ConfigureAwait(false);

                                            for (int i = 0; i < recipients.Length; i++)
                                            {
                                                kissLogger.Info($"SUBSCRIBER- ACCOUNT NOTIFICATION: Email Sending-- Email sent to {recipients[i]} via SendGrid at {DateTimeOffset.UtcNow}.");
                                            }
                                        }
                                    }

                                    if (isSmsNotificationAllowed && !string.IsNullOrWhiteSpace(notification.SmsMessageTemplate))
                                    {
                                        string[] smsRecipients = users.Select(x => x.PhoneNumber).ToArray();
                                        string messageContent = notification.SmsMessageTemplate;
                                        foreach (var placeholder in smsPlaceholders)
                                        {
                                            messageContent = messageContent.Replace(placeholder.Key, placeholder.Value);
                                        }
                                        await Task.Run(() => _smsService.SendMessage(messageContent, smsRecipients));

                                        for (int i = 0; i < smsRecipients.Length; i++)
                                        {
                                            kissLogger.Info($"SUBSCRIBER- ACCOUNT NOTIFICATION: SMS Sending-- SMS sent to {smsRecipients[i]} via Twilio at {DateTimeOffset.UtcNow}.");
                                        }
                                    }
                                    await args.CompleteMessageAsync(args.Message);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                kissLogger.Error(ex, $"SUBSCRIBER- ACCOUNT NOTIFICATION: Sorry, the following issue occurred. -- Subject: {args.Message.Subject}, with Message ID: {args.Message.MessageId} created at {args.Message.EnqueuedTime} with current Delivery count: {args.Message.DeliveryCount}");
            }
            finally
            {
                Logger.NotifyListeners(kissLogger);
            }
        }

        #endregion
    }
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
}
