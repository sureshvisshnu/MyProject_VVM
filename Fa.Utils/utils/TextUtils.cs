using System;
using Microsoft.EntityFrameworkCore;

namespace fa.api.utils
{
    public static class TextUtils
    {
        public static long ToLong(string Number)
        {
            try
            {
                long MyNum = Int64.Parse(Number);
                return MyNum;
            }
            catch
            {
                return 0;
            }
        }

        public static String DecimalPlace(int Decimal)
        {

            string Result = "0.";
            for (int i = 0; i < Decimal; i++)
            {
                Result = Result + "0";
            }
            return Result;
        }
        public static bool isPatientNumber(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                long var;
                if (text.Length == 12 && long.TryParse(text, out var))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsPhoneNumber(string SearchText)
        {
            if (!String.IsNullOrEmpty(SearchText) && SearchText.All(char.IsDigit))
            {
                long var;
                if (SearchText.Length >= 3 && long.TryParse(SearchText, out var))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool isTockenNumber(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                long var;
                if (text.Length >= 1 && text.Length <= 3 && long.TryParse(text, out var))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool isReferenceNumber(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                long var;
                if (text.Length == 6 && long.TryParse(text, out var))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isAmount(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                double var;
                if (double.TryParse(text, out var))
                {
                    return true;
                }
            }
            return false;
        }

        public static string getRandomTempFileName(int Length, string Ext)
        {
            Random Random = new Random();
            var WindowsTempPath = System.IO.Path.GetTempPath();
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string RandomFileName = new string(Enumerable.Repeat(Chars, Length).Select(s => s[Random.Next(s.Length)]).ToArray());
            var FileName = String.Format("{0}{1}.{2}", WindowsTempPath, RandomFileName, Ext);
            return FileName;
        }
    }
}
