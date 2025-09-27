namespace fa.model.Accounting.Transactions
{
    public enum PaymentType
    {
        CASH, CHECK, CREDITCARD, BANKTRANSFER, UPI
    }
    public enum PaymentTransactioType
    {
        Cash = 0,
        GPay = 1,
        PhonePe = 2,
        Paytm = 3,
        UPI = 4,
        Card = 5,
        Cheque = 6,
        RTGS = 7
    }

}
