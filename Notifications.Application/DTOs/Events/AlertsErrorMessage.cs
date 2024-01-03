using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class AlertsErrorMessage
    {
        public string Source { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime DateReported { get; set; }

    }
}
