using System;

namespace fa.api.utils
{
    public class NumberUtils
    {
        public static decimal PricisionFix(decimal DecimalValue, int Precision)
        {
            try
            {
                decimal result;
                string byresult;
                byresult = DecimalValue.ToString(string.Format("#.{0}", new string('0', Precision)));
                result = Convert.ToDecimal(byresult);
                return result;
            }
            catch
            {
                return DecimalValue;
            }
        }
    }
}
