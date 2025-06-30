namespace fa.model.Accounting.Transaction
{
    public enum DiscountType
    {
        PERCENT=1, VALUE=2
    }

    public static class DiscountCalculator
    {
        public static float calculateDiscount(DiscountType DiscountType, float Amount, float Discount)
        {
            float result = 0.0F;
            if(DiscountType == DiscountType.PERCENT)
            {
                result = (Amount * Discount) / 100;
            }
            else if(DiscountType == DiscountType.VALUE)
            {
                result = Discount;
            }
            return result;
        }
    }
}
