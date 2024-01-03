using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.DTOs.EmailConfiguration
{
    public class EmailValuesDto
    {
        [JsonProperty("from")]
        public EmailPersonnelDto? From { get; set; }

        [JsonProperty("personalizations")]
        public List<PersonalizationDto>? Personalizations { get; set; }

        [JsonProperty("template_id")]
        public string? TemplateId { get; set; }
    }
}
