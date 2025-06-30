namespace fa.model.Application
{
    public class Application
    {
        public long Id { get; set; }
        public int CurrentInitializationStep { get; set; }
        public bool Initialized { get; set; }
        public DateTime DateInitialized { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string RegisteredCompanyName {get;set;}
        public string RegisteredCompanyAddress { get; set; }
    }

    public class ApplicationLicence
    {
        public long Id { get; set; }
        public string LicenseKey { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Validated { get; set; }
    }

    public enum ApplicationType
    {
        HOSPITAL, RETAIL, WHOLESALE
    }
}
