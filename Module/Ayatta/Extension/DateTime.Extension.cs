using System;

namespace Ayatta.Extension
{
    public static partial class Extension
    {
        public static string ToString(this DateTime? dateTime, string format, string @default)
        {
            return dateTime?.ToString(format) ?? @default;
        }

        public static string ToString(this DateTime? dateTime, string format, DateTime @default)
        {
            return dateTime?.ToString(format) ?? @default.ToString(format);
        }
    }
}