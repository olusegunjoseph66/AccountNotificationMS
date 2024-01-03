using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.Constants
{
    public class EventMessages
    {
        public const string NOTIFICATION_SETTINGS_UPDATED = "notifications.settings.updated";
        public const string NOTIFICATION_TOPIC = "notifications";
        public const string ACCOUNT_TOPIC = "accounts";
        public const string ACCOUNTS_USER_CREATED = "accounts.user.created";
        public const string ACCOUNTS_OTP_GENERATED = "accounts.otp.generated";
        public const string ACCOUNTS_PASSWORD_UPDATED = "accounts.password.updated";
        public const string ACCOUNTS_SAPACCOUNT_CREATED = "accounts.sapAccount.created";
        public const string ACCOUNTS_DELETION_REQUEST_CREATED = "accounts.deletionRequest.created";
        public const string ACCOUNTS_USER_UPDATED = "accounts.user.updated";
        public const string ACCOUNTS_USER_DELETED = "accounts.user.deleted";
        public const string ACCOUNTS_USER_LOGIN = "accounts.user.login";

        public const string ORDER_TOPIC = "orders";
        public const string ORDERS_OTP_GENERATED = "orders.otp.generated";
        public const string ORDERS_DMSORDER_SUBMISSION_FAILED = "orders.dmsorder.submissionfailed";
        public const string ORDERS_DMSORDER_SUBMISSION_SUCCESSFUL = "orders.dmsorder.submissionsuccessful";
        public const string ORDERS_DMSORDER_UPDATED = "orders.dmsorder.updated";
        public const string ORDERS_DMSORDER_CANCELLATION = "orders.cancellation.requested";
        public const string ORDERS_DMSORDER_STATUS_CHANGED = "orders.dmsorder.orderstatuschanged";
        public const string ORDERS_DMSORDER_ATC_AVAILABLE = "orders.dmsorder.atcavailable";
        public const string ORDERS_DMSORDER_DELIVERY_STATUS_CHANGED = "orders.dmsorder.deliverystatuschanged";
        public const string ORDERS_DMSORDER_DELIVERY_TRIP_STATUS_CHANGED = "orders.dmsorder.tripstatuschanged";
        public const string ORDERS_DMSORDER_CREATED = "orders.dmsorder.created";

        public const string SUPPORT_TOPIC = "support";
        public const string SUPPORT_REQUEST_CREATED = "support.request.created";
        public const string SUPPORT_COMMENT_CREATED = "support.comment.created";
        public const string SUPPORT_COMMENT_UPDATED = "support.comment.updated";
        public const string SUPPORT_REQUEST_UPDATED = "support.request.updated";
        public const string ALERT_TOPIC = "alerts";
        public const string ALERTS_ERROR = "alerts.error";
    }
}
