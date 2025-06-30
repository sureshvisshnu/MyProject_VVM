namespace FaData.Utils
{
    public static class DateUtils
    {
        public static DateTime Ceil(DateTime value)
        {
            return new System.DateTime(value.Year, value.Month, value.Day, 23, 59, 59);
        }

        public static DateTime Floor(DateTime value)
        {
            return new System.DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
        }

        public static int? ComputeAge(DateTime TodaysDate, DateTime DateOfBirth)
        {
            if (DateOfBirth.Year > 1900 && DateOfBirth.Year < 9998)
            {
                int age = 0;
                age = DateTime.Now.Year - DateOfBirth.Year;
                if (TodaysDate.DayOfYear < DateOfBirth.DayOfYear)
                    age = age - 1;
                return age;
            }
            return null;
        }

        public static string FormatDate(DateTime date, string format)
        {
            return date.ToString(format);
        }
    }
}
