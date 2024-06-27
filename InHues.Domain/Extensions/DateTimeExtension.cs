

using System.Globalization;
using System;

namespace InHues.Domain.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToDateTime(this string stringDate, DateTime defaultVal = new DateTime()) => !string.IsNullOrEmpty(stringDate) ? DateTime.ParseExact(stringDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture) : defaultVal;
        public static DateTime? ToNullableDateTime(this string stringDate, DateTime? defaultVal = null) => !string.IsNullOrEmpty(stringDate) ? DateTime.ParseExact(stringDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture) : defaultVal;
        public static DateTimeOffset ToDateTimeOffset(this string stringDate) => DateTimeOffset.ParseExact(stringDate, "yyyy-MM-ddTHH:mm:ss.fffffffK", CultureInfo.InvariantCulture);
        public static DateTimeOffset? ToNullableDateTimeOffset(this string stringDate, DateTimeOffset? defaultVal = null) => !string.IsNullOrEmpty(stringDate) ? DateTimeOffset.ParseExact(stringDate, "yyyy-MM-ddTHH:mm:ss.fffffffK", CultureInfo.InvariantCulture) : defaultVal;
        public static string ToSQLFormat(this DateTime date) => date.ToString("yyyy-MM-ddTHH:mm:ssZ");//2019-03-16T00:00:00.00Z
        public static string ToSQLFormat(this DateTimeOffset date) => date.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");//2019-03-16T00:00:00.00Z
        public static string ToSQLFormat(this DateTimeOffset? date) => date is not null ? date.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK") : null;//2019-03-16T00:00:00.00Z
        public static string ToSQLFormat(this DateTime? date) => date is not null ? date.Value.ToString("yyyy-MM-ddTHH:mm:ssZ") : null;//2019-03-16T00:00:00.00Z
        public static string ToSQLFormat(this DateOnly date) => date.ToString("yyyy-MM-dd");//2019-03-16
        public static string ToSQLFormat(this DateOnly? date) => date is not null ? date.Value.ToString("yyyy-MM-dd") : null;//2019-03-16
        //public static string ToSQLFormat(this DateTime date) => date.ToString("yyyy-MM-ddTHH\\:mm\\:ss.zz", CultureInfo.InvariantCulture);
    }
}
