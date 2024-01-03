using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.Constants.NotificationMessages
{
    public class AccountMessages
    {
        internal const string DISTRIBUTOR_WELCOME_NOTIFICATION = "Congratulations! Welcome to the Dangote Distributor Management System.  Do contact our sales team if you need any assistance.";
        internal const string ADMIN_WELCOME_NOTIFICATION = "Congratulations! Welcome to the Dangote Distributor Management System.";
        internal const string PASSWORD_CHANGE_CONFIRMATION_NOTIFICATION = "Congratulations! Your password has been changed. Do contact our support team if you did not perform this operation.";
        internal const string ACCOUNT_LINKED_NOTIFICATION = "Congratulations! Your account has been linked to your other account(s)";
        internal const string ACCOUNT_DELETION_REQUEST_NOTIFICATION = "Your account has been unlinked to your other account(s). Do contact our support team if you need any assistance.  We are happy to help.";


        public static string GetMessage(string key)
        {
            Dictionary<string, string> messages = new()
            {
                { NotificationMessagesKey.DISTRIBUTOR_WELCOME_NOTIFICATION, DISTRIBUTOR_WELCOME_NOTIFICATION },
                { NotificationMessagesKey.ADMIN_WELCOME_NOTIFICATION, ADMIN_WELCOME_NOTIFICATION },
                { NotificationMessagesKey.PASSWORD_CHANGE_CONFIRMATION_NOTIFICATION, PASSWORD_CHANGE_CONFIRMATION_NOTIFICATION },
                { NotificationMessagesKey.ACCOUNT_LINKED_NOTIFICATION, ACCOUNT_LINKED_NOTIFICATION },
                { NotificationMessagesKey.ACCOUNT_DELETION_REQUEST_NOTIFICATION, ACCOUNT_DELETION_REQUEST_NOTIFICATION }
            };

            var message = messages.FirstOrDefault(x => x.Key == key);

            if (!message.Equals(default(KeyValuePair<string, string>)))
                return message.Value;

            return string.Empty;
        }

    }

    
}
