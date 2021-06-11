using System;
using System.Globalization;

namespace MPS.Common.Extenstions
{
    public static class PersianDate
    {
        /// <summary>
        /// یک استرینگ تاریخ شمسی را به معادل میلادی تبدیل میکند
        /// </summary>
        /// <param name="persianDate">تاریخ شمسی</param>
        /// <returns>تاریخ میلادی</returns>
        public static DateTime ToGeorgianDateTime(this string persianDate)
        {
            var persianCalender = new PersianCalendar();
            var date = DateTime.Parse(persianDate);
            return persianCalender.ToDateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// یک تاریخ میلادی را به معادل فارسی آن تبدیل میکند
        /// </summary>
        /// <param name="georgianDate">تاریخ میلادی</param>
        /// <returns>تاریخ شمسی</returns>
        public static string ToPersianDateString(this DateTime georgianDate)
        {
            try
            {
                var persianCalendar = new PersianCalendar();

                var year = persianCalendar.GetYear(georgianDate).ToString();
                var month = persianCalendar.GetMonth(georgianDate).ToString().PadLeft(2, '0');
                var day = persianCalendar.GetDayOfMonth(georgianDate).ToString().PadLeft(2, '0');
                return $"{year}/{month}/{day}";
            }catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// یک تعداد روز را از یک تاریخ شمسی کم میکند یا به آن آضافه میکند
        /// </summary>
        /// <param name="persianDate">تاریخ شمسی اول</param>
        /// <param name="days">تعداد روزی که میخواهیم اضافه یا کم کنیم</param>
        /// <returns>تاریخ شمسی به اضافه تعداد روز</returns>
        public static string AddDaysToShamsiDate(this string persianDate, int days)
        {
            var dt = persianDate.ToGeorgianDateTime();
            dt = dt.AddDays(days);
            return dt.ToPersianDateString();
        }
    }
}