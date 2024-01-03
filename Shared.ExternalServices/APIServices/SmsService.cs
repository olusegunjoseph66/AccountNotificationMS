using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using Shared.ExternalServices.Configurations;
using Shared.ExternalServices.DTOs.EmailConfiguration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Azure.Amqp.CbsConstants;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Shared.ExternalServices.Interfaces;
using Shared.Utilities.Helpers;
using Microsoft.Extensions.Configuration;

namespace Shared.ExternalServices.APIServices
{
    public class SmsService : ISmsService
    {
        private readonly SmsSetting _smsSetting;
        public readonly IConfiguration _config;

        public SmsService(IOptions<SmsSetting> smsSetting, IConfiguration _config)
        {
            _smsSetting = smsSetting.Value;
            this._config = _config;
        }

        public async Task<bool> SendMessage(string message, string[] recipients)
        {
            try
            {
                //TwilioClient.Init(_smsSetting.AccountSid, _smsSetting.AuthToken);
                TwilioClient.Init(_config["SmsSetting:AccountSid"], _config["SmsSetting:AuthToken"]);

                if (recipients.Length > 0)
                {
                    for (int i = 0; i < recipients.Length; i++)
                    {
                        var messageOptions = new CreateMessageOptions(new PhoneNumber(recipients[i].ToPhoneNumberFormat()))
                        {
                            MessagingServiceSid = _config["SmsSetting:MessagingServiceSid"],//_smsSetting.MessagingServiceSid,
                            Body = message
                        };
                        var response = await MessageResource.CreateAsync(messageOptions);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
