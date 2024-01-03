using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.Constants.NotificationMessages
{
    public class OrderMessages
    {
        internal const string ORDER_SUBMISSION_FAILED_NOTIFICATION = "We are sorry your order cannot be processed. We value your business. Do contact our sales team to help you with the order. ";
        internal const string ORDER_SUBMITTED_NOTIFICATION = "Your order has been placed. Thank you for your business. We will revert shortly.";
        internal const string ORDER_CANCELLATION_REQUEST_NOTIFICATION = "Your cancellation request is received. We will revert shortly.";
        internal const string ORDER_STATUS_CHANGED_NOTIFICATION = "Your order has progressed to the next stage.  We will revert shortly with more information.";
        internal const string DELIVERY_STATUS_CHANGED_NOTIFICATION = "Your delivery has progressed to the next stage.  We will revert shortly with more information.";
        internal const string TRIP_STATUS_CHANGED_NOTIFICATION = "Your delivery trip has progressed to the next stage.  We will revert shortly with more information.";
        internal const string ORDER_UPDATED_NOTIFICATION = "Your order has been updated.";


        public static string GetMessage(string key)
        {
            Dictionary<string, string> messages = new()
            {
                { NotificationMessagesKey.ORDER_SUBMISSION_FAILED_NOTIFICATION, ORDER_SUBMISSION_FAILED_NOTIFICATION },
                { NotificationMessagesKey.ORDER_SUBMITTED_NOTIFICATION, ORDER_SUBMITTED_NOTIFICATION },
                { NotificationMessagesKey.ORDER_CANCELLATION_REQUEST_NOTIFICATION, ORDER_CANCELLATION_REQUEST_NOTIFICATION },
                { NotificationMessagesKey.ORDER_STATUS_CHANGED_NOTIFICATION, ORDER_STATUS_CHANGED_NOTIFICATION },
                { NotificationMessagesKey.DELIVERY_STATUS_CHANGED_NOTIFICATION, DELIVERY_STATUS_CHANGED_NOTIFICATION },
                { NotificationMessagesKey.TRIP_STATUS_CHANGED_NOTIFICATION, TRIP_STATUS_CHANGED_NOTIFICATION },
                { NotificationMessagesKey.ORDER_UPDATED_NOTIFICATION, ORDER_UPDATED_NOTIFICATION }
            };

            var message = messages.FirstOrDefault(x => x.Key == key);

            if (!message.Equals(default(KeyValuePair<string, string>)))
                return message.Value;

            return string.Empty;
        }

    }
}
