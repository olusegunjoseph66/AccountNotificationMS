using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.Constants.NotificationMessages
{
    public class NotificationMessagesKey
    {
        public const string DISTRIBUTOR_WELCOME_NOTIFICATION = "N01";
        public const string ADMIN_WELCOME_NOTIFICATION = "N02";
        public const string PASSWORD_CHANGE_CONFIRMATION_NOTIFICATION = "N03";
        public const string ACCOUNT_LINKED_NOTIFICATION = "N04";
        public const string ACCOUNT_DELETION_REQUEST_NOTIFICATION = "N05";

        public const string ORDER_SUBMISSION_FAILED_NOTIFICATION = "N07";
        public const string ORDER_SUBMITTED_NOTIFICATION = "N08";
        public const string ORDER_CANCELLATION_REQUEST_NOTIFICATION = "N09";
        public const string ORDER_STATUS_CHANGED_NOTIFICATION = "N10";
        public const string DELIVERY_STATUS_CHANGED_NOTIFICATION = "N11";
        public const string TRIP_STATUS_CHANGED_NOTIFICATION = "N12";
        public const string ORDER_UPDATED_NOTIFICATION = "N13";

        public const string SUPPORT_REQUEST_ADMIN_NOTIFICATION = "N14";
        public const string SUPPORT_REQUEST_NOTIFICATION = "N15";
        public const string SUPPORT_REQUEST_COMMENT_NOTIFICATION = "N16";
        public const string SUPPORT_REQUEST_UPDATE_NOTIFICATION = "N17";

        public const string ERROR_ALERT_NOTIFICATION = "N18";
    }
}
