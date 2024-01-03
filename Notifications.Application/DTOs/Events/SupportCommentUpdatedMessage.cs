using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.DTOs.Events
{
    public class SupportCommentUpdatedMessage
    {
        public int CommentId { get; set; }
        public Request Request { get; set; } = new Request();
        public DateTime DateCreated { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByName { get; set; } = "";
        public string Comments { get; set; } = "";
    }
}
