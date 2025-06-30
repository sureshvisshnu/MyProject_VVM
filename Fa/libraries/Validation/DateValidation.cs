using fa.views.controls.text;
using System;
using System.Globalization;

namespace fa.libraries.Validation
{
    public class DateValidation
    {

        private static volatile DateValidation instance;
        private static object syncRoot = new Object();
        DateValidation()
        {

        }
        public static DateValidation Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DateValidation();
                    }
                }

                return instance;
            }
        }
        public string ChangeMaskFormat(DateTime date)
        {
            string MaskFormat = null;
            if (Global.Company.DateFormat == "MM/dd/yy" || Global.Company.DateFormat == "yy/MM/dd")
            {
                MaskFormat = "00/00/00";
            }
            else if (Global.Company.DateFormat == "dd/MM/yyyy" || Global.Company.DateFormat == "MM/dd/yyyy")
            {
                MaskFormat = "00/00/0000";
            }
            else if (Global.Company.DateFormat == "M/d/yyyy")
            {
                MaskFormat = ResetTexBoxMask(date, Global.Company.DateFormat, MaskFormat);
            }
            else if (Global.Company.DateFormat == "M/d/yy")
            {
                MaskFormat = ResetTexBoxMask(date, Global.Company.DateFormat, MaskFormat);
            }
            else if (Global.Company.DateFormat == "yyyy/MM/dd")
            {
                MaskFormat = "0000/00/00";
            }
            else if (Global.Company.DateFormat == "dd/MMM/yy")
            {
                MaskFormat = "00/LLL/00";
            }
            return MaskFormat;
        }
        public string ResetTexBoxMask(DateTime date, string Format, string MaskFormat)
        {
            if (Format == "M/d/yyyy")
            {
                if (date.Day < 10 && date.Month < 10)
                {
                    MaskFormat = "0/0/0000";
                }
                else if (date.Day < 10)
                {
                    MaskFormat = "00/0/0000";
                }
                else if (date.Month < 10)
                {
                    MaskFormat = "0/00/0000";
                }
                else
                {
                    MaskFormat = "00/00/0000";
                }
            }
            else if (Format == "M/d/yy")
            {
                if (date.Day < 10 && date.Month < 10)
                {
                    MaskFormat = "0/0/00";
                }
                else if (date.Day < 10)
                {
                    MaskFormat = "00/0/00";
                }
                else if (date.Month < 10)
                {
                    MaskFormat = "0/00/00";
                }
                else
                {
                    MaskFormat = "00/00/00";
                }
            }
            return MaskFormat;
        }
    }
}
