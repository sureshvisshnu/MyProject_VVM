using fa.model.UserProfile;

namespace fa.Data
{
    public static class Global
    {
        public static bool isAuthenticated = false;
        public static User User=null;
        public static DateTime TransactionDate;
        public static string IpAddressDefault = null!;
        public static string DefaultHostName = null!;
    }
}
