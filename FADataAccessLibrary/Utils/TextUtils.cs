namespace FADataAccessLibrary.Utils
{
    public static class TextUtils
    {
        public static String DecimalPlace(int Decimal)
        {

            string Result = "0.";
            for (int i = 0; i < Decimal; i++)
            {
                Result = Result + "0";
            }
            return Result;
        }
    }
}
