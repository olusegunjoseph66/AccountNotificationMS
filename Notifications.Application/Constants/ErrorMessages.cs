﻿namespace Notifications.Application.Constants
{
    public class ErrorMessages
    {
        internal const string DEFAULT_VALIDATION_MESSAGE = "Sorry, you have supplied one or more wrong inputs. Kindly check your input and try again.";
        internal const string DEFAULT_AUTHORIZATION_MESSAGE = "Sorry, you do not have the right to perform this operation.";

        public const string SERVER_ERROR = "Sorry, we are unable to fulfill your request at the moment, kindly try again later.";
        public const string DATABASE_CONFLICT_ERROR = "One or more unique fields already exist, kindly try again later.";
        public const string NOT_FOUND_ERROR = "Sorry, the resource you have requested for is not available at the moment.";
        public const string INVALID_OR_INCORRECT_VALUES = "Invalid or incorrect values provided";
        public const string CAN_NOT_UPDATE_WITH_NAME_OF_ANOTHER_CATEGORY = "Cannot update category with the name of another category";
        public const string FAQ_DETAILS_NOT_RETRIEVED = "Faq does not exist";
        public const string FAQ_CATEGORY_NOT_CREATED = "Faq Category Not Created";
        public const string CAN_NOT_UPDATE_WITH_QUESTION_OF_ANOTHER_FAQ = "Cannot update faq with the question of another faq";
        public const string FAILED_REQUEST_CATEGORY_LIST_RETRIEVAL = "An Error occurred while you were attempting to fetch request categories";
        public const string USER_ID_MUST_BE_SUPPLIED = "User Id Must Be Supplied";
        public const string CHANNELS_FETCH_FAILURE = "An Error Occurred While Fetching Channels";
        public const string COUNTRIES_FETCH_FAILURE = "An Error Occurred While Fetching Countries";
        public const string STATES_FETCH_FAILURE = "An Error Occurred While Fetching States";
        public const string COMPANIES_FETCH_FAILURE = "An Error Occurred While Fetching Companies";
        public const string CURRENCIES_FETCH_FAILURE = "An Error Occurred While Fetching Currencies";
        public const string DISTRIBUTOR_SAP_ACCOUNT_NOT_FOUND = "The provided Distributor Number is invalid or not found";
        public const string INVALID_ROUTE = "Invalid Route. You are not authorized to view this wallet";
        public const string SAP_WALLET_STATEMENT_FETCH_FAILURE = "An Error Occurred While Fetching Sap Statement";
        public const string EMPTY_LIST_OF_NOTIFICATIONS_SETTINGS="Notifications Settings List Can Not Be Empty";
        public const string NOTIFICATIONS_SETTINGS_FETCH_ERROR = "An Error Occurred While Fetching Notifications Settings";

         public const string FAILED_USER_NOTIFICATIONS_RETRIEVAL = "An Error Occurred While Fetching Notifications For This User";
        public const string FAILED_USER_NOTIFICATIONS_ACKNOWLEDGEMENT="An Error Occurred While User Was Acknowledging This Notification";
        public const string NON_EXISTENT_NOTIFICATION="The Notification Does Not Exist";
        public const string USER_NOT_AUTHENTICATED="You are not logged in";
        public const string INVALID_USER_NOTIFICATION_ID="Invalid User Notification Id";
        public const string NOT_A_DISTRIBUTOR="You Can Not Access This Endpoint Because You Are Not A Distributor";
    }
}
