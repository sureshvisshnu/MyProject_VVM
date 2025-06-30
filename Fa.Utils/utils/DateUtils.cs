using System;
using System.Globalization;
using System.Web;
using fa.api.Log;

namespace fa.api.utils
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
        public static DateTime? ReportFromDate(string Format,int days)
        {
           return ((DateTime)DateUtils.ToDate(Global.CompanyFinancialPeriodBegin.ToString(Format), Format)).Date <= ((DateTime)DateUtils.ToDate(Global.TransactionDate.ToString(Format), Format)).AddDays(-days).Date ? ((DateTime)DateUtils.ToDate(Global.TransactionDate.ToString(Format), Format)).Date.AddDays(-days) : ((DateTime)DateUtils.ToDate(Global.TransactionDate.ToString(Format), Format)).AddDays((((DateTime)DateUtils.ToDate(Global.CompanyFinancialPeriodBegin.ToString(Format), Format)).Date- ((DateTime)DateUtils.ToDate(Global.TransactionDate.ToString(Format), Format))).TotalDays);
        }
        public static DateTime? ToDate(string StrDate, string Format)
        {
            try
            {
                if(StrDate!=null)
                {
                   DateTime.TryParseExact(StrDate, Format, null, System.Globalization.DateTimeStyles.None, out DateTime ToDate);
                    return ToDate.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
                }
                return null;
            }
            catch(FormatException fe)
            {
                Logger.LogError(fe);
            }
            return null;
        }

        public static DateTime? ToStartDate(int StrMonth, int Year)
        {
            try
            {
                DateTime StartDate =new DateTime(Year, StrMonth, 1);
                return StartDate;
            }
            catch (FormatException fe)
            {
                Logger.LogError(fe);
            }
            return null;
        }
        public static DateTime? ToEndDate(int StrMonth, int Year)
        {
            try
            {
                DateTime StartDate = new DateTime(Year, StrMonth, 1).AddYears(1).AddDays(-1);
                return StartDate;
            }
            catch (FormatException fe)
            {
                Logger.LogError(fe);
            }
            return null;
        }

        public static bool ValidDate_TillCurrentDate(string Date, string Format)
        {           
            DateTime dt;
            bool Status = DateTime.TryParseExact(Date, Format, null, DateTimeStyles.None, out dt);
            if(Status)
            {
                DateTime CurrentDate = DateTime.ParseExact(DateTime.Now.ToString(Format), Format, null).Date;
                int Year = CurrentDate.Year;

                if (DateTime.ParseExact(Date, Format, null).Year < 1900 || DateTime.ParseExact(Date, Format, null).Date > CurrentDate)
                {
                    Status = false;
                }
            }
            return Status;
        }
        public static bool ValidDate_TillFinancialPeriodsStartEndDate(string Date, string Format)
        {
            DateTime dt;
            bool Status = DateTime.TryParseExact(Date, Format, null, DateTimeStyles.None, out dt);
            if (Status)
            {
                DateTime CompanyFinancialPeriodBegin = DateTime.ParseExact(Global.CompanyFinancialPeriodBegin.ToString(Format), Format, null).Date;
                DateTime CompanyFinancialPeriodEnd = DateTime.ParseExact(Global.CompanyFinancialPeriodEnd.ToString(Format), Format, null).Date;
                DateTime CurrentDate = DateTime.ParseExact(Date, Format, null);
                if (CompanyFinancialPeriodBegin > CurrentDate || CompanyFinancialPeriodEnd < CurrentDate)
                {
                    Status = false;
                }
            }
            return Status;
        }
        public static bool ValidDate(string Date, string Format)
        {
            DateTime dt;
            bool Status = DateTime.TryParseExact(Date, Format, null, DateTimeStyles.None, out dt);
            if (Status)
            {
                int Year = DateTime.ParseExact(Date, Format, null).Year;
                if (Year < 1900 || Year >9998 )
                {
                    Status = false;
                }
            }
            return Status;
        }
       
        public static int? ComputeAge(DateTime TodaysDate,DateTime DateOfBirth)
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
        public static bool CheckGreaterDate(string FromDates, string Format)
        {
            bool Status = false;

            TimeSpan ts = new TimeSpan();
            DateTime DateFrom = Convert.ToDateTime(FromDates);
            DateTime TodayDates = Convert.ToDateTime(DateTime.Now);

            ts = DateFrom.Subtract(TodayDates);//Result in integer format
            if (ts.Hours>0)
            {
                Status = true;
            }
            return Status;
        }
        public static string FormatDate (DateTime date, string format)
        {
            return date.ToString(format);
        }        
        public static bool TryParseDate(string dateString, out DateTime parsedDateTime, string DateFormat)
        {
            string[] dateFormats = { DateFormat }; // Add more formats as needed

            return DateTime.TryParseExact(
                dateString,
                dateFormats,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out parsedDateTime
            );
        }
    }
}
