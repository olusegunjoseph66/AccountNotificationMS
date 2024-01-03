using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.DTOs.EmailConfiguration
{
    public class PersonalizationDto
    {
        [JsonProperty("subject")]
        public string? Subject { get; set; }

        [JsonProperty("to")]
        public List<EmailPersonnelDto>? To { get; set; }

        [JsonProperty("dynamic_template_data")]
        public dynamic? DynamicTemplate { get; set; }
    }
}
