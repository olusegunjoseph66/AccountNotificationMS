using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.Constants.NotificationMessages
{
    public class SupportMessages
    {
        internal const string SUPPORT_REQUEST_ADMIN_NOTIFICATION = "A new support request has been received.";
        internal const string SUPPORT_REQUEST_NOTIFICATION = "Your request has been sent. Our support team will revert shortly. Thank you.";
        internal const string SUPPORT_REQUEST_COMMENT_NOTIFICATION = "We have provided response to your support request. Do let us know if we can help further.  Thank you.";
        internal const string SUPPORT_REQUEST_UPDATE_NOTIFICATION = "An update has been made to your support ticket.  Do let us know if we can help further.  Thank you.";


        public static string GetMessage(string key)
        {
            Dictionary<string, string> messages = new()
            {
                { NotificationMessagesKey.SUPPORT_REQUEST_ADMIN_NOTIFICATION, SUPPORT_REQUEST_ADMIN_NOTIFICATION },
                { NotificationMessagesKey.SUPPORT_REQUEST_NOTIFICATION, SUPPORT_REQUEST_NOTIFICATION },
                { NotificationMessagesKey.SUPPORT_REQUEST_COMMENT_NOTIFICATION, SUPPORT_REQUEST_COMMENT_NOTIFICATION },
                { NotificationMessagesKey.SUPPORT_REQUEST_UPDATE_NOTIFICATION, SUPPORT_REQUEST_UPDATE_NOTIFICATION }
            };

            var message = messages.FirstOrDefault(x => x.Key == key);

            if (!message.Equals(default(KeyValuePair<string, string>)))
                return message.Value;

            return string.Empty;
        }
    }
}
