using InHues.Domain.Entities;
using System;

namespace InHues.Domain.Extensions
{
    public static class GenericExtensions
    {
        public static string ToCurrency(this int value, string decimalPlace = "#,##0", string symbol = "₱") {
            return $"{symbol} {value.ToString(decimalPlace)}";
        }

        public static string ToCurrency(this double value, string decimalPlace = "#,##0", string symbol = "₱")
        {
            return $"{symbol} {value.ToString(decimalPlace)}";
        }
        public static DateTimeOffset ToLocalDT(this DateTimeOffset dt, string timezoneId = "Asia/Singapore") {
            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            return TimeZoneInfo.ConvertTime(dt, targetTimeZone);
        }
        public static DateTime ToLocalDT(this DateTime dt, string timezoneId = "Asia/Singapore")
        {
            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            return TimeZoneInfo.ConvertTime(dt, targetTimeZone);
        }
    }
}
