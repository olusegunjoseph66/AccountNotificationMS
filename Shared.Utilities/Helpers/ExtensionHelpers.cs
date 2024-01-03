using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;

namespace Shared.Utilities.Helpers
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Dereference of a possibly null reference.
    public static class ExtensionHelpers
    {
        public static string ToDescription(this Enum value)
        {
            FieldInfo? fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static T? Deserialize<T>(this string jsonString) where T : new()
        {
            return !string.IsNullOrWhiteSpace(jsonString) ? JsonConvert.DeserializeObject<T>(jsonString) : new T();
        }

        public static string Serialize(this object @object)
        {
            return @object != null ? JsonConvert.SerializeObject(@object) : string.Empty;
        }

        public static string ToPhoneNumberFormat(this string phoneNumber, string? countryPhoneCode = null)
        {
            string phoneCode = string.Empty;
            string convertedPhoneNumber = "";
            if(string.IsNullOrWhiteSpace(countryPhoneCode)) 
                phoneCode = "234";
            else
                phoneCode = countryPhoneCode;

            char[] phoneNumberDigits = phoneNumber.ToCharArray();
            string dialingCode = phoneNumber.Substring(1, 3);
            if (phoneNumberDigits[0] == '+')
            {
                if(dialingCode != phoneCode)
                    convertedPhoneNumber = $"{phoneCode}{phoneNumber.Substring(1)}";
                else
                    convertedPhoneNumber = phoneNumber.Substring(1);
            }
            else
            {
                if (dialingCode != phoneCode)
                    convertedPhoneNumber = $"{phoneCode}{phoneNumber.Substring(0)}";
                else
                    convertedPhoneNumber = phoneNumber.Substring(0);
            }

            return convertedPhoneNumber;
        }

        public static DateTime ConvertToLocal(this DateTime dateTime)
        {
            TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timezone);
        }
    }
#pragma warning restore CS8600 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}
