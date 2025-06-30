using fa.model.Accounting.Masters;
using FaData.Utils;

namespace fa.report
{
    public abstract class Report : IReport
    {
        public Company Company { get; set; }
        public CostCenter CostCenter { get; set; }
        private DateTime _FromDate { get; set; }
        public DateTime FromDate {
            get
            {
                return _FromDate;
            }

            set
            {
                _FromDate = DateUtils.Floor(value);
            }
        }
        private DateTime _ToDate { get; set; }
        public DateTime ToDate
        {
            get
            {
                return _ToDate;
            }

            set
            {
                _ToDate = DateUtils.Ceil(value);
            }
        }
        public abstract string ReportTitle();
        public abstract string ReportName();
        public abstract void GenerateReport();        
        public long PageSize = long.MaxValue;
        public long PageNumber = 0;
    }
}
