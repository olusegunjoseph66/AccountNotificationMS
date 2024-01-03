using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using Shared.ExternalServices.Configurations;
using Shared.ExternalServices.DTOs.EmailConfiguration;
using Shared.ExternalServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.APIServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _emailSetting;
        public readonly IConfiguration _config;

        public EmailService(IOptions<EmailSetting> emailSetting, IConfiguration _config)
        {
            //_emailSetting = emailSetting.Value;
            this._config = _config;
        }

        public async Task<bool> SendMessage(string emailTemplateId, string[] recipients, List<Dictionary<string, string>> emailValues, CancellationToken cancellationToken = default)
        {
            bool isSuccessful = false;
            try
            {
                var sender = new EmailPersonnelDto()
                {
                    Email = _config["EmailSetting:Sender"] //_emailSetting.Sender
                };

                List<EmailPersonnelDto> emailRecipients = new();
                for (int index = 0; index < recipients.Length; index++)
                {
                    emailRecipients.Add(new EmailPersonnelDto
                    {
                        Email = recipients[index].Trim()
                    });
                }

                var emailValue = emailValues.ElementAt(0);
                dynamic expando = new ExpandoObject();

                for (int index = 0; index < emailValue.Count; index++)
                {
                    AddProperty(expando, emailValue.ElementAt(index).Key, emailValue.ElementAt(index).Value);
                }

                List<PersonalizationDto> Personalizations = new()
                {
                    new PersonalizationDto
                    {
                        DynamicTemplate = expando,
                        To = emailRecipients
                    }
                };

                var emailBody = new EmailValuesDto()
                {
                    Personalizations = Personalizations,
                    From = sender,
                    TemplateId = emailTemplateId
                };
                var requestBody = JsonConvert.SerializeObject(emailBody);

                //var client = new RestClient(_emailSetting.BaseUrl);
                //var request = new RestRequest(_emailSetting.EmailEndpoint, Method.Post);
                var client = new RestClient(_config["EmailSetting:BaseUrl"]);
                var request = new RestRequest(_config["EmailSetting:EmailEndpoint"], Method.Post);
                //request.AddHeader("Authorization", $"Bearer {_emailSetting.ApiKey}");
                request.AddHeader("Authorization", $"Bearer {_config["EmailSetting:ApiKey"]}");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", $"{requestBody}", ParameterType.RequestBody);
                var response = await client.ExecuteAsync(request);
                if (response.IsSuccessStatusCode)
                    isSuccessful = true;

                return isSuccessful;
            }
            catch (Exception)
            {
                return isSuccessful;
            }

        }

        #region Private Methods
        private void AddProperty(ExpandoObject placeholder, string propertyName, object propertyValue)
        {
            var placeholderDictionary = placeholder as IDictionary<string, object>;

            if (placeholderDictionary.ContainsKey(propertyName))
                placeholderDictionary[propertyName] = propertyValue;
            else
                placeholderDictionary.Add(propertyName, propertyValue);
        }
        #endregion

    }
}
