using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs
{
    public class PlaceholderArray
    {
        public string Email { get; set; }
        public int UserId { get; set; }
        public List<Dictionary<string, string>> PlaceholderValues = new();
    }
}
