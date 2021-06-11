using System;

namespace Moba.Common.Helpers
{
    public static class DateTimeHelper
    {
        public static long GetTimeStamp(DateTime dt, DateTimeKind dtk)
        {
            long unixTimestamp = (long)(dt.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, dtk))).TotalSeconds;
            return unixTimestamp;
        }
    }
}