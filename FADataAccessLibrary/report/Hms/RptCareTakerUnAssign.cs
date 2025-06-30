using fa.api.Accounting;
using fa.api.Hms;
using fa.model.Hms.common;
using fa.context;
using fa.model.Employee;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.report;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Data;

namespace FADataAccessLibrary.report.Hms
{

    public class RptCareTakerUnAssign : Report
    {
        public int FilterIndex = 0;
        public long[] ConsultantId { get; set; }
        public long[] DepartmentId { get; set; }
        public bool UnAssignStatus = true;
        public List<UnAssignCareTakerReport> LineUnAssignCareTakerReport = new List<UnAssignCareTakerReport>();

        public override string ReportTitle()
        {
            return string.Format("Care Taker Assign Report");
        }
        public string ReportSubTitle()
        {
            return String.Format("From {0} To {1}", DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));
        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Care Taker Assign Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<InPatientAdmission> InPatientAdmissions = null;
                InPatientAdmissions = (Context.InPatientAdmissions.Where(pa => pa.DateOfAdmission >= this.FromDate.Date).OrderByDescending(x => x.DateOfAdmission)).ToList();

                if (InPatientAdmissions != null)
                {
                    DateTime? TempDate = null;
                    List<long> employeeIds = new List<long>(); // Initialize a list to hold EmployeeId values
                                                               // Get all employees for the company
                    List<Employee> allEmployees = EmployeeManager.Instance.ListEmployeeByCompanyIdTitle(Company.CompanyId, "Doctor").ToList();
                    // Extract EmployeeIds from the Employee objects
                    List<long> allEmployeeIds = allEmployees.Select(emp => emp.Id).ToList();
                    int ECount = 0;
                    string TaskResult = "UnAssigned";
                    bool UnAssignStatus = false;
                    if (FilterIndex == 0)
                    {
                        foreach (InPatientAdmission Admission in InPatientAdmissions.Where(x => x.DateOfAdmission.Date <= ToDate.Date))
                        {
                            //if (FilterIndex == 0)
                            {
                                InPatientLocation InPatientLocation = Context.InPatientLocations
                                    .Include("Ward")
                                    .Include("Bed")
                                    .FirstOrDefault(x => x.AdmissionId == Admission.Id);

                                if (InPatientLocation != null)
                                {
                                    Employee Emp = EmployeeManager.Instance.GetEmployeeInfoById(Admission.CurrentMedicalTeam?.PrimaryDoctorId ?? 0);
                                    if (Emp != null)
                                    {
                                        employeeIds.Add(Emp.Id); // Add the AccountId (EmployeeId) to the list
                                    }
                                }
                            }
                        }
                    }
                    else if (FilterIndex == 1)
                    {
                        foreach (var employeeId in ConsultantId)
                        {
                            long ConsultEmployeeId = employeeId;
                            long AdmissionId = 0;
                            bool DischargeState = false;
                            IList<MedicalTeam> AdmissionIds = MedicalTeamManager.Instance.ListMedicalTeambyAdmissionIds(ConsultEmployeeId);
                            if(AdmissionIds != null)
                            {
                                foreach(var PatAmission in AdmissionIds)
                                {
                                    AdmissionId = PatAmission.AdmissionId;
                                    DischargeNote DischargeStatus = DischargeNoteManager.Instance.GetDischargeStatusByAdmissionId(AdmissionId);
                                    if(DischargeStatus != null)
                                    {
                                        DischargeState = true;
                                        FromDate = DischargeStatus.Date;
                                        ToDate = (DateTime)DischargeStatus.DischargeOn;
                                    }
                                }                                
                            }
                            if (DischargeState == true)
                            {
                                IList<MedicalTeam> DrMedicalTeam = MedicalTeamManager.Instance.ListMedicalTeambyDrId(ConsultEmployeeId, FromDate);
                                if (DrMedicalTeam != null && DrMedicalTeam.Count > 0)
                                {
                                    UnAssignStatus = false;

                                }
                                else
                                {
                                    UnAssignStatus = true;
                                    TaskResult = "UnAssigned";
                                    UnAssignCareTakerReport CTRreport = CreateUnAssignCareTakerReportConsultant(Context, ConsultEmployeeId, TaskResult, FromDate, ToDate);
                                    if (CTRreport != null)
                                    {
                                        LineUnAssignCareTakerReport.Add(CTRreport);
                                    }
                                }
                            }
                            else
                            {
                                IList<MedicalTeam> DrMedicalTeam = MedicalTeamManager.Instance.ListMedicalTeambyDrId(ConsultEmployeeId, FromDate);
                                if (DrMedicalTeam != null && DrMedicalTeam.Count > 0)
                                {
                                    UnAssignStatus = false;

                                }
                                else
                                {
                                    UnAssignStatus = true;
                                    TaskResult = "UnAssigned";
                                    UnAssignCareTakerReport CTRreport = CreateUnAssignCareTakerReportConsultant(Context, ConsultEmployeeId, TaskResult, FromDate, ToDate);
                                    if (CTRreport != null)
                                    {
                                        LineUnAssignCareTakerReport.Add(CTRreport);
                                    }
                                }
                            }
                            
                            
                            //IList<InPatientAdmission> InPatientAdmission = null;
                            //InPatientAdmission = (Context.InPatientAdmissions.Where(pa => pa.DateOfAdmission >= this.FromDate.Date).OrderByDescending(x => x.DateOfAdmission)).ToList();
                            //if (InPatientAdmission != null)
                            //{
                            //    foreach (InPatientAdmission Admission in InPatientAdmission.Where(x => x.DateOfAdmission.Date <= ToDate.Date && x.CompanyId == Company.CompanyId))
                            //    {
                            //        MedicalTeam IpMedicalTeam = MedicalTeamManager.Instance.GetMedicalTeambyAdmissionId(Admission.Id, ConsultEmployeeId);
                            //        if (IpMedicalTeam != null)
                            //        {
                            //            TaskResult = "Assigned";
                            //        }
                            //    }
                            //}
                            //if (ConsultantId.Length != ECount)
                            //{
                            //    InPatientAdmission = (Context.InPatientAdmissions.Where(pa => pa.DateOfAdmission < this.FromDate.Date && pa.CompanyId == Company.CompanyId).OrderByDescending(x => x.DateOfAdmission)).ToList();
                            //    if(InPatientAdmission != null)
                            //    {
                            //        foreach (InPatientAdmission Admission in InPatientAdmission)
                            //        {
                            //            IList<MedicalTeam> IpMedicalTeam = MedicalTeamManager.Instance.GetMedicalTeambyAdmissionDateId(Admission.Id, ConsultEmployeeId, FromDate, ToDate);
                            //            if (IpMedicalTeam != null && IpMedicalTeam.Count > 0)
                            //            {
                            //                foreach(MedicalTeam MedicalTeams in IpMedicalTeam)
                            //                {
                            //                    if(MedicalTeams.Active == true)
                            //                    {
                            //                        TaskResult = "Assigned";

                            //                    }
                            //                    else
                            //                    {
                            //                        TaskResult = "UnAssigned";
                            //                    }
                            //                }
                            //            }
                            //            else
                            //            {
                            //                TaskResult = "UnAssigned";
                            //            }
                            //        }
                            //        UnAssignCareTakerReport CTRreportByDr = CreateUnAssignCareTakerReportConsultant(Context, ConsultEmployeeId, TaskResult, FromDate, ToDate);
                            //        if (CTRreportByDr != null)
                            //        {
                            //            LineUnAssignCareTakerReport.Add(CTRreportByDr);
                            //        }
                            //    }                                
                            //}
                            TaskResult = "UnAssigned";
                            ECount++;
                        }                        
                    }
                    else if (FilterIndex == 2)
                    {
                        //IList<InPatientAdmission> IpAdmissions = null;
                        //IpAdmissions = IpManager.Instance.GetListofInPatientAdmissionByCompanyIdDate(Company.CompanyId, FromDate, ToDate);
                        //if(IpAdmissions != null)
                        //{

                        //}
                        long EmployeesId = 0L;
                        DateTime LastDate;
                        DateTime DateIn;
                        foreach (var dipid in DepartmentId)
                        {
                            ECount = 0;
                            IList<Employee> Employeee = EmployeeManager.Instance.ListEmployeeByDepartmentId(dipid);
                            if (Employeee != null)
                            {
                                foreach (Employee employees in Employeee)
                                {
                                    EmployeesId = employees.Id;
                                    int AssignCount = 0;
                                    IList<MedicalTeam> medicalTeamDr = MedicalTeamManager.Instance.ListAllMedicalTeamHistoryByPrDr(EmployeesId);
                                    if ((medicalTeamDr != null) && (medicalTeamDr.Count > 0))
                                    {
                                        foreach (MedicalTeam medicalTeam in medicalTeamDr.OrderBy(x => x.AdmissionId))
                                        {
                                            UnAssignStatus = false;
                                            InPatientLocation IpAdmission = IpManager.Instance.GetInPatientLocationbyAdmitIdDate(medicalTeam.AdmissionId, FromDate.Date, ToDate.Date);
                                            if (IpAdmission != null)
                                            {
                                                AssignCount++;                                                
                                                LastDate = (IpAdmission.DateMovedOut == DateTime.MinValue) ? ToDate : IpAdmission.DateMovedOut;
                                                UnAssignStatus = (IpAdmission.DateMovedOut == DateTime.MinValue) ? true : false;
                                                if(IpAdmission.DateMovedIn > FromDate.Date)
                                                {
                                                    DateTime CurrentDate = IpAdmission.DateMovedIn.AddDays(-1);
                                                    TaskResult = "UnAssigned";
                                                    UnAssignCareTakerReport CTRreportFirst = CreateUnAssignCareTakerReportDept(Context, EmployeesId, TaskResult, FromDate.Date, CurrentDate);
                                                    if (CTRreportFirst != null)
                                                    {
                                                        LineUnAssignCareTakerReport.Add(CTRreportFirst);
                                                    }
                                                    DateIn = CurrentDate;
                                                    TaskResult = "Assigned";
                                                }
                                                UnAssignCareTakerReport CTRreport = CreateUnAssignCareTakerReportDept(Context, EmployeesId, TaskResult, IpAdmission.DateMovedIn, LastDate);
                                                if (CTRreport != null)
                                                {
                                                    LineUnAssignCareTakerReport.Add(CTRreport);
                                                }
                                                TaskResult = "UnAssigned";
                                                LastDate = IpAdmission.DateMovedIn;
                                            }
                                            else
                                            {
                                                AssignCount++;
                                                TaskResult = "UnAssigned";
                                            }
                                            if (AssignCount == medicalTeamDr.Count && UnAssignStatus == false)
                                            {
                                                UnAssignCareTakerReport CTRreport = CreateUnAssignCareTakerReportDept(Context, EmployeesId, TaskResult, FromDate, ToDate);
                                                if (CTRreport != null)
                                                {
                                                    LineUnAssignCareTakerReport.Add(CTRreport);
                                                }
                                            }
                                            TaskResult = "UnAssigned";
                                            ECount++;
                                        }
                                    }
                                    ECount = 0;
                                    IList<MedicalTeam> medicalTeamNr = MedicalTeamManager.Instance.ListAllMedicalTeamHistoryByPrCG(EmployeesId);
                                    if ((medicalTeamNr != null) && (medicalTeamNr.Count > 0))
                                    {
                                        foreach (MedicalTeam medicalTeam in medicalTeamNr.OrderBy(x => x.AdmissionId))
                                        {
                                            InPatientLocation IpAdmission = IpManager.Instance.GetInPatientLocationbyAdmitIdDate(medicalTeam.AdmissionId, FromDate.Date, ToDate.Date);
                                            if (IpAdmission != null)
                                            {
                                                AssignCount++;
                                                LastDate = (IpAdmission.DateMovedOut == DateTime.MinValue) ? ToDate : IpAdmission.DateMovedOut;
                                                UnAssignStatus = (IpAdmission.DateMovedOut == DateTime.MinValue) ? true : false;
                                                if (IpAdmission.DateMovedIn > FromDate.Date)
                                                {
                                                    DateTime CurrentDate = IpAdmission.DateMovedIn.AddDays(-1);
                                                    IList<MedicalTeam> CareTakerHistoryInfo = MedicalTeamManager.Instance.ListAllMedicalTeamHistoryByPrimaryId("Nurse", EmployeesId, FromDate);
                                                    if (CareTakerHistoryInfo != null)
                                                    {
                                                        foreach(MedicalTeam TeamActive in CareTakerHistoryInfo )
                                                        {
                                                            if(TeamActive.Active == true)
                                                            {
                                                                TaskResult = "Assigned";
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                TaskResult = "UnAssigned";
                                                            }
                                                        }
                                                        UnAssignCareTakerReport CTRreportFirst = CreateUnAssignCareTakerReportDept(Context, EmployeesId, TaskResult, FromDate.Date, CurrentDate);
                                                        if (CTRreportFirst != null)
                                                        {
                                                            LineUnAssignCareTakerReport.Add(CTRreportFirst);
                                                        }
                                                        DateIn = CurrentDate;
                                                        TaskResult = "Assigned";
                                                    }
                                                    UnAssignCareTakerReport CTRreport = CreateUnAssignCareTakerReportDept(Context, EmployeesId, TaskResult, IpAdmission.DateMovedIn, LastDate);
                                                    if (CTRreport != null)
                                                    {
                                                        LineUnAssignCareTakerReport.Add(CTRreport);
                                                    }
                                                    TaskResult = "UnAssigned";
                                                }                                                
                                            }
                                            else
                                            {
                                                AssignCount++;
                                                TaskResult = "UnAssigned";
                                            }
                                            if (AssignCount == medicalTeamNr.Count && UnAssignStatus == false)
                                            {
                                                UnAssignCareTakerReport CTRreport = CreateUnAssignCareTakerReportDept(Context, EmployeesId, TaskResult, FromDate, ToDate);
                                                if (CTRreport != null)
                                                {
                                                    LineUnAssignCareTakerReport.Add(CTRreport);
                                                }
                                            }
                                            TaskResult = "UnAssigned";
                                            ECount++;
                                        }
                                    }                                    
                                }                                
                            }
                        }
                    }
                    if (FilterIndex == 0)
                    {
                        List<long> remainingEmployeeIds = allEmployeeIds.Except(employeeIds).ToList();
                        List<UnAssignCareTakerReport> CTRreportsNew = CreateUnAssignCareTakerReportNew(Context, remainingEmployeeIds, ref TempDate, TaskResult, FromDate, ToDate);

                        foreach (UnAssignCareTakerReport report in CTRreportsNew)
                        {
                            LineUnAssignCareTakerReport.Add(report);
                        }
                    }
                }
            }
        }
        private UnAssignCareTakerReport CreateUnAssignCareTakerReportConsultant(AccountMasterContext Context, long EmployeeId, string TaskStatusResult, DateTime FromDt, DateTime ToDt)
        {
            UnAssignCareTakerReport careTaker = new UnAssignCareTakerReport();

            Employee Emp = EmployeeManager.Instance.GetEmployeeInfoByIdType(EmployeeId);
            if (Emp != null)
            {
                careTaker.AdmissionId = Emp.Id != 0 ? Emp.Id.ToString() : string.Empty;
                careTaker.Name = Emp?.Name ?? string.Empty;
                careTaker.Age = Emp.DateOfBirth.HasValue ? (DateTime.Now.Year - Emp.DateOfBirth.Value.Year).ToString() : "";
                careTaker.Address = Emp.Address.FullAddress;
                careTaker.DepartmentOfConsultant = Emp?.Department?.Name ?? "";
                careTaker.Notes = ""; // You may need to set appropriate values for Notes
                careTaker.TaskStatus = TaskStatusResult;
                careTaker.AssignDate = FromDt.Date + " to " + ToDt.Date;
            }
            return careTaker;
        }
        private UnAssignCareTakerReport CreateUnAssignCareTakerReportDept(AccountMasterContext Context, long EmployeeId, string TaskStatusResult, DateTime FromDate, DateTime EndDate)
        {
            UnAssignCareTakerReport careTaker = new UnAssignCareTakerReport();

            Employee Emp = EmployeeManager.Instance.GetEmployeeInfoByIdType(EmployeeId);
            if (Emp != null)
            {
                careTaker.AdmissionId = Emp.Id != 0 ? Emp.Id.ToString() : string.Empty;
                careTaker.Name = Emp?.Name ?? string.Empty;
                careTaker.Age = Emp.DateOfBirth.HasValue ? (DateTime.Now.Year - Emp.DateOfBirth.Value.Year).ToString() : "";
                careTaker.Address = Emp.Address.FullAddress;
                careTaker.DepartmentOfConsultant = Emp?.Department?.Name ?? "";
                careTaker.Notes = ""; // You may need to set appropriate values for Notes
                careTaker.TaskStatus = TaskStatusResult;
                careTaker.AssignDate = FromDate.Date + " to " + EndDate.Date;
            }
            return careTaker;
        }
        private List<UnAssignCareTakerReport> CreateUnAssignCareTakerReportNew(AccountMasterContext Context, List<long> remainingEmployeeIds, ref DateTime? TempDate, string TaskStatusResult, DateTime FromDt, DateTime ToDt)
        {
            List<UnAssignCareTakerReport> careTakerReports = new List<UnAssignCareTakerReport>();

            foreach (long employeeId in remainingEmployeeIds)
            {
                Employee Emp = EmployeeManager.Instance.GetEmployeeInfoByIdType(employeeId);
                if (Emp != null)
                {
                    UnAssignCareTakerReport careTaker = new UnAssignCareTakerReport();
                    
                    careTaker.AdmissionId = Emp.Id != 0 ? Emp.Id.ToString() : string.Empty;
                    careTaker.Name = Emp?.Name ?? string.Empty;
                    careTaker.Age = Emp.DateOfBirth.HasValue ? (DateTime.Now.Year - Emp.DateOfBirth.Value.Year).ToString() : "";
                    careTaker.Address = Emp.Address.FullAddress;
                    careTaker.DepartmentOfConsultant = Emp?.Department?.Name ?? "";
                    careTaker.Notes = ""; // You may need to set appropriate values for Notes
                    careTaker.TaskStatus = TaskStatusResult;
                    careTaker.AssignDate = FromDt.Date + " to " + ToDate.Date;
                    careTakerReports.Add(careTaker);
                }
            }

            return careTakerReports;
        }
        public DataTable CreateUnAssignCareTakerReport(long CompanyId, DateTime globalStartDate, DateTime globalEndDate, long departmentId)
        {
            try
            {
                DataTable dataTable = new DataTable();
                AccountMasterContext Context = new AccountMasterContext();
                string ConString = Context.Database.GetDbConnection().ConnectionString;

                // Query to get the list of doctors and their assignment date ranges
                string doctorQuery = @"
            SELECT DISTINCT
                e.Id AS DoctorId,
                a.Name AS DoctorName,
                MIN(il.DateMovedIn) AS AssignmentStartDate,  -- Earliest assignment start date
                MAX(COALESCE(il.DateMovedOut, @GlobalEndDate)) AS AssignmentEndDate  -- Latest assignment end date or the global end date
            FROM employees e
            INNER JOIN accounts a ON e.Id = a.Id
            INNER JOIN inpatientlocations il ON il.AuthorizedByDoctorId = e.Id
            INNER JOIN titles t ON e.TitleId = t.Id
            WHERE e.IsServiceProvider = 1
            AND t.Name = 'Doctor'
            AND e.DepartmentId = @DepartmentId
            AND t.CompanyId = @CompanyId
            AND il.DateMovedIn <= @GlobalEndDate  -- Ensure that assignments overlap with the global date range
            GROUP BY e.Id, a.Name
            ORDER BY e.Id;
        ";

                // Query to retrieve assignment information for each doctor and date
                string assignmentQuery = @"
            WITH DoctorAssignments AS (
                SELECT
                    il.AuthorizedByDoctorId AS DoctorId,
                    a.Name AS DoctorName,
                    d.Name AS DepartmentName,
                    il.DateMovedIn AS AssignmentStartDate,
                    COALESCE(il.DateMovedOut, @EndDate) AS AssignmentEndDate,  -- If no end date, use global end date
                    ia.PatientId,
                    ia.Status AS AdmissionStatus,
                    ia.DateOfAdmission,
                    ia.LastModifiedDate AS DateOfDischarge,
                    il.WardId,
                    il.BedId
                FROM inpatientlocations il
                INNER JOIN inpatientadmissions ia ON il.AdmissionId = ia.Id
                INNER JOIN employees e ON e.Id = il.AuthorizedByDoctorId
                INNER JOIN accounts a ON a.Id = e.Id
                INNER JOIN departments d ON d.Id = e.DepartmentId
                WHERE e.IsServiceProvider = 1
                AND il.AuthorizedByDoctorId = @DoctorId
                AND ia.DateOfAdmission <= @EndDate
                AND (ia.LastModifiedDate >= @StartDate OR ia.Status = 0)
                AND e.DepartmentId = @DepartmentId
                AND a.CompanyId = @CompanyId
            )
            SELECT
                DoctorId,
                DoctorName,
                DepartmentName,
                AssignmentStartDate,
                AssignmentEndDate,
                CASE 
                    WHEN @DateUnion BETWEEN AssignmentStartDate AND AssignmentEndDate THEN 'Assigned'
                    ELSE 'Unassigned'
                END AS DoctorStatus,
                @DateUnion AS DateUnion
            FROM DoctorAssignments
            ORDER BY DoctorId, AssignmentStartDate;
        ";

                using (MySqlConnection connection = new MySqlConnection(ConString))
                {
                    connection.Open();

                    // Step 1: Retrieve the list of doctors and their assignment date ranges
                    List<(long DoctorId, DateTime AssignmentStartDate, DateTime AssignmentEndDate)> doctorAssignments = new List<(long, DateTime, DateTime)>();

                    using (MySqlCommand doctorCommand = new MySqlCommand(doctorQuery, connection))
                    {
                        doctorCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                        doctorCommand.Parameters.AddWithValue("@CompanyId", CompanyId);
                        doctorCommand.Parameters.AddWithValue("@GlobalEndDate", globalEndDate);

                        using (MySqlDataReader reader = doctorCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                long doctorId = reader.GetInt64("DoctorId");
                                DateTime assignmentStartDate = reader.GetDateTime("AssignmentStartDate");
                                DateTime assignmentEndDate = reader.GetDateTime("AssignmentEndDate");

                                doctorAssignments.Add((doctorId, assignmentStartDate, assignmentEndDate));
                            }
                        }
                    }

                    // Step 2: Process each doctor, using their specific assignment date range
                    foreach (var doctor in doctorAssignments)
                    {
                        long doctorId = doctor.DoctorId;
                        DateTime assignmentStartDate = doctor.AssignmentStartDate;
                        DateTime assignmentEndDate = doctor.AssignmentEndDate;

                        // Generate a date range between the assignment start and end dates
                        List<DateTime> dateRange = new List<DateTime>();
                        for (DateTime date = assignmentStartDate; date <= assignmentEndDate; date = date.AddDays(1))
                        {
                            dateRange.Add(date);
                        }

                        // Step 3: Execute the assignment query for each date in the range
                        foreach (var dateUnion in dateRange)
                        {
                            using (MySqlCommand command = new MySqlCommand(assignmentQuery, connection))
                            {
                                // Add parameters for doctorId, specific assignment date range, and the current dateUnion
                                command.Parameters.AddWithValue("@DoctorId", doctorId);
                                command.Parameters.AddWithValue("@StartDate", assignmentStartDate);
                                command.Parameters.AddWithValue("@EndDate", assignmentEndDate);
                                command.Parameters.AddWithValue("@DateUnion", dateUnion);
                                command.Parameters.AddWithValue("@DepartmentId", departmentId);
                                command.Parameters.AddWithValue("@CompanyId", CompanyId);

                                // Fill the DataTable with the result of the current doctor and date
                                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                                {
                                    adapter.Fill(dataTable);
                                }
                            }
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the report.", ex);
            }
        }

        public DataTable CreateUnAssignCareTakerReport16092024_2(long CompanyId, DateTime startDate, DateTime endDate, long departmentId)
        {
            try
            {
                DataTable dataTable = new DataTable();
                AccountMasterContext Context = new AccountMasterContext();
                string ConString = Context.Database.GetDbConnection().ConnectionString;

                // Query to get the list of doctors in the department
                string doctorQuery = @"
                    SELECT DISTINCT e.Id AS DoctorId, a.Name AS DoctorName
                    FROM employees e
                    INNER JOIN accounts a ON e.Id = a.Id
                    INNER JOIN titles t ON e.TitleId = t.Id
                    WHERE e.IsServiceProvider = 1
                    AND t.Name = 'Doctor'
                    AND e.DepartmentId = @DepartmentId
                    AND t.CompanyId = @CompanyId;
                ";

                // Query to retrieve the assignment information for each doctor and date
                string assignmentQuery = @"
                    WITH DoctorAssignments AS (
                        SELECT
                            il.AuthorizedByDoctorId AS DoctorId,
                            a.Name AS DoctorName,
                            d.Name AS DepartmentName,
                            il.DateMovedIn AS AssignmentStartDate,
                            il.DateMovedOut AS AssignmentEndDate,
                            ia.PatientId,
                            ia.Status AS AdmissionStatus,
                            ia.DateOfAdmission,
                            ia.LastModifiedDate AS DateOfDischarge,
                            il.WardId,
                            il.BedId
                        FROM inpatientlocations il
                        INNER JOIN inpatientadmissions ia ON il.AdmissionId = ia.Id
                        INNER JOIN employees e ON e.Id = il.AuthorizedByDoctorId
                        INNER JOIN accounts a ON a.Id = e.Id
                        INNER JOIN departments d ON d.Id = e.DepartmentId
                        WHERE e.IsServiceProvider = 1
                        AND il.AuthorizedByDoctorId = @DoctorId
                        AND ia.DateOfAdmission <= @EndDate
                        AND (ia.LastModifiedDate >= @StartDate OR ia.Status = 0)
                        AND e.DepartmentId = @DepartmentId
                        AND a.CompanyId = @CompanyId
                    )
                    SELECT
                        DoctorId,
                        DoctorName,
                        DepartmentName,
                        AssignmentStartDate,
                        COALESCE(AssignmentEndDate, @EndDate) AS AssignmentEndDate,
                        CASE 
                            WHEN @DateUnion BETWEEN AssignmentStartDate AND COALESCE(AssignmentEndDate, @EndDate) THEN 'Assigned'
                            ELSE 'Unassigned'
                        END AS DoctorStatus,
                        @DateUnion AS DateUnion
                    FROM DoctorAssignments
                    ORDER BY DoctorId, AssignmentStartDate;
                ";

                using (MySqlConnection connection = new MySqlConnection(ConString))
                {
                    connection.Open();

                    // Step 1: Retrieve the list of doctors for the department
                    List<long> doctorIds = new List<long>();
                    using (MySqlCommand doctorCommand = new MySqlCommand(doctorQuery, connection))
                    {
                        doctorCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                        doctorCommand.Parameters.AddWithValue("@CompanyId", CompanyId);

                        using (MySqlDataReader reader = doctorCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                doctorIds.Add(reader.GetInt64("DoctorId"));
                            }
                        }
                    }

                    // Step 2: Generate the list of dates (DateRange) between startDate and endDate in C#
                    List<DateTime> dateRange = new List<DateTime>();
                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        dateRange.Add(date);
                    }

                    // Step 3: Process each doctor sequentially, then iterate over the date range
                    foreach (long doctorId in doctorIds)
                    {
                        foreach (var dateUnion in dateRange)
                        {
                            using (MySqlCommand command = new MySqlCommand(assignmentQuery, connection))
                            {
                                // Add parameters for doctorId, startDate, endDate, dateUnion, departmentId, and companyId
                                command.Parameters.AddWithValue("@DoctorId", doctorId);
                                command.Parameters.AddWithValue("@StartDate", startDate);
                                command.Parameters.AddWithValue("@EndDate", endDate);
                                command.Parameters.AddWithValue("@DateUnion", dateUnion);
                                command.Parameters.AddWithValue("@DepartmentId", departmentId);
                                command.Parameters.AddWithValue("@CompanyId", CompanyId);

                                // Fill the DataTable with the result of the current doctor and date
                                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                                {
                                    adapter.Fill(dataTable);
                                }
                            }
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the report.", ex);
            }
        }

        public DataTable CreateUnAssignCareTakerReportAlmostCorrect(long CompanyId, DateTime startDate, DateTime endDate, long departmentId)
        {
            try
            {
                DataTable dataTable = new DataTable();
                AccountMasterContext Context = new AccountMasterContext();
                string ConString = Context.Database.GetDbConnection().ConnectionString;

                // Modified SQL query
                string query = @"
                    WITH DoctorAssignments AS (
                        SELECT
                            e.Id AS DoctorId,
                            a.Name AS EmployeeName,  -- Employee name from accounts
                            d.Name AS DepartmentName,  -- Department name
                            il.DateMovedIn AS AssignmentStartDate,  -- Start date from inpatientlocations
                            il.DateMovedOut AS AssignmentEndDate,  -- End date from inpatientlocations
                            ia.Status AS AdmissionStatus,  -- Status from inpatientadmissions
                            ia.PatientId,
                            ia.DateOfAdmission,
                            ia.LastModifiedDate AS DateOfDischarge,  -- Discharge date from inpatientadmissions
                            il.AuthorizedByDoctorId AS DoctorAssigned,  -- Assigned doctor
                            il.WardId,  -- Ward details from inpatientlocations
                            il.BedId  -- Bed details from inpatientlocations
                        FROM inpatientlocations il
                        INNER JOIN inpatientadmissions ia ON il.AdmissionId = ia.Id  -- Link to inpatientadmissions
                        INNER JOIN employees e ON e.Id = il.AuthorizedByDoctorId  -- Link to employees for authorized doctor
                        INNER JOIN accounts a ON a.Id = e.Id  -- Join with accounts to get employee name
                        INNER JOIN titles t ON t.Id = e.TitleId  -- Join with titles to identify doctors
                        INNER JOIN departments d ON d.Id = e.DepartmentId  -- Join with departments for department name
                        WHERE e.IsServiceProvider = 1
                        AND t.Name = 'Doctor'  -- Filter for employees with the title 'Doctor'
                        AND ia.DateOfAdmission <= @EndDate
                        AND (ia.LastModifiedDate >= @StartDate OR ia.Status = 0)  -- Date or status conditions
                        AND e.DepartmentId = @DepartmentId
                        AND t.CompanyId = @CompanyId  -- Ensure the title belongs to the specified company
                    ),
                    DoctorStatus AS (
                        SELECT
                            da.DoctorId,
                            da.EmployeeName,  -- Employee name from accounts
                            da.DepartmentName,  -- Department name
                            @DateUnion AS DateUnion,
                            CASE 
                                WHEN @DateUnion BETWEEN da.AssignmentStartDate AND COALESCE(da.AssignmentEndDate, @EndDate) THEN 'Assigned'
                                ELSE 'Unassigned'
                            END AS DoctorStatus
                        FROM DoctorAssignments da
                    ),
                    GroupedStatus AS (
                        SELECT
                            ds.DoctorId,
                            ds.EmployeeName,  -- Employee name from accounts
                            ds.DepartmentName,  -- Department name
                            ds.DateUnion AS StatusDate,
                            ds.DoctorStatus,
                            ROW_NUMBER() OVER (PARTITION BY ds.DoctorId ORDER BY ds.DateUnion) - 
                            ROW_NUMBER() OVER (PARTITION BY ds.DoctorId, ds.DoctorStatus ORDER BY ds.DateUnion) AS GroupingKey
                        FROM DoctorStatus ds
                    )
                    SELECT
                        DoctorId,
                        EmployeeName,  -- Employee name from accounts
                        DepartmentName,  -- Department name
                        MIN(StatusDate) AS FromDate,
                        MAX(StatusDate) AS ToDate,
                        DoctorStatus
                    FROM GroupedStatus
                    GROUP BY DoctorId, EmployeeName, DepartmentName, DoctorStatus, GroupingKey
                    ORDER BY DoctorId, FromDate;
                ";

                // Generate the list of dates (DateRange) between startDate and endDate in C#
                List<DateTime> dateRange = new List<DateTime>();
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    dateRange.Add(date);
                }

                using (MySqlConnection connection = new MySqlConnection(ConString))
                {
                    connection.Open();

                    // Execute the query for each date in the range
                    foreach (var dateUnion in dateRange)
                    {
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            // Add parameters for startDate, endDate, departmentId, CompanyId, and DateUnion
                            command.Parameters.AddWithValue("@StartDate", startDate);
                            command.Parameters.AddWithValue("@EndDate", endDate);
                            command.Parameters.AddWithValue("@DepartmentId", departmentId);
                            command.Parameters.AddWithValue("@CompanyId", CompanyId);
                            command.Parameters.AddWithValue("@DateUnion", dateUnion);

                            // Fill the DataTable with the result
                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                            {
                                adapter.Fill(dataTable);
                            }
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the report.", ex);
            }
        }

        public DataTable CreateUnAssignCareTakerReport16092024Modified(long CompanyId, DateTime startDate, DateTime endDate, long departmentId)
        {
            try
            {
                DataTable dataTable = new DataTable();
                AccountMasterContext Context = new AccountMasterContext();
                string ConString = Context.Database.GetDbConnection().ConnectionString;

                // The SQL query without recursive DateRange CTE
                string query = @"
                        WITH RECURSIVE DateRange AS (
                            SELECT @StartDate AS DateUnion
                            UNION ALL
                            SELECT DATE_ADD(DateUnion, INTERVAL 1 DAY)
                            FROM DateRange
                            WHERE DateUnion < @EndDate
                        ),
                        DoctorAssignments AS (
                            SELECT
                                e.Id AS DoctorId,
                                e.TitleId,
                                t.Name AS DoctorTitle,
                                d.Name AS DepartmentName,
                                mt.From AS AssignmentStartDate,
                                mt.To AS AssignmentEndDate,
                                ia.Status AS AdmissionStatus,
                                ia.PatientId,
                                ia.DateOfAdmission,
                                ia.LastModifiedDate AS DateOfDischarge,
                                mt.PrimaryDoctorId,
                                mt.SecondaryDoctorId,
                                mt.PrimaryCareGiverId,
                                mt.SecondaryCareGiverId
                            FROM medicalteams mt
                            INNER JOIN inpatientadmissions ia ON mt.AdmissionId = ia.Id
                            INNER JOIN employees e ON (e.Id = mt.PrimaryDoctorId OR e.Id = mt.SecondaryDoctorId)
                            INNER JOIN departments d ON d.Id = e.DepartmentId
                            INNER JOIN titles t ON t.Id = e.TitleId
                            WHERE e.IsServiceProvider = 1
                            AND t.Name = 'Doctor'  -- Assuming 'Doctor' is the title name
                            AND t.CompanyId = @CompanyId
                            AND ia.DateOfAdmission <= @EndDate
                            AND (ia.LastModifiedDate >= @StartDate OR ia.Status = 0)
                            AND d.Id = @DepartmentId
                        ),
                        DoctorStatus AS (
                            SELECT
                                da.DoctorId,
                                da.DoctorTitle,
                                da.DepartmentName,
                                dr.DateUnion,
                                CASE 
                                    WHEN dr.DateUnion BETWEEN da.AssignmentStartDate AND COALESCE(da.AssignmentEndDate, @EndDate) THEN 'Assigned'
                                    ELSE 'Unassigned'
                                END AS DoctorStatus
                            FROM DoctorAssignments da
                            CROSS JOIN DateRange dr
                            WHERE dr.DateUnion BETWEEN @StartDate AND @EndDate
                        ),
                        GroupedStatus AS (
                            SELECT
                                ds.DoctorId,
                                ds.DoctorTitle,
                                ds.DepartmentName,
                                ds.DateUnion AS StatusDate,
                                ds.DoctorStatus,
                                ROW_NUMBER() OVER (PARTITION BY ds.DoctorId ORDER BY ds.DateUnion) - 
                                ROW_NUMBER() OVER (PARTITION BY ds.DoctorId, ds.DoctorStatus ORDER BY ds.DateUnion) AS GroupingKey
                            FROM DoctorStatus ds
                        )
                        SELECT
                            DoctorId,
                            DoctorTitle AS DoctorName,
                            DepartmentName,
                            MIN(StatusDate) AS FromDate,
                            MAX(StatusDate) AS ToDate,
                            DoctorStatus
                        FROM GroupedStatus
                        GROUP BY DoctorId, DoctorTitle, DepartmentName, DoctorStatus, GroupingKey
                        ORDER BY DoctorId, FromDate;
                    ";

                // Generate the list of dates (DateRange) between startDate and endDate in C#
                List<DateTime> dateRange = new List<DateTime>();
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    dateRange.Add(date);
                }

                using (MySqlConnection connection = new MySqlConnection(ConString))
                {
                    connection.Open();

                    // Execute the query for each date in the range
                    foreach (var dateUnion in dateRange)
                    {
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            // Add parameters for startDate, endDate, departmentId, and DateUnion
                            command.Parameters.AddWithValue("@StartDate", startDate);
                            command.Parameters.AddWithValue("@EndDate", endDate);
                            command.Parameters.AddWithValue("@DepartmentId", departmentId);
                            command.Parameters.AddWithValue("@DateUnion", dateUnion);

                            // Fill the DataTable with the result
                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                            {
                                adapter.Fill(dataTable);
                            }
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the report.", ex);
            }
        }
        public DataTable CreateUnAssignCareTakerReport11092024(long CompanyId, DateTime startDate, DateTime endDate, long departmentId)
        {
            try
            {
                DataTable dataTable = new DataTable();
                AccountMasterContext Context = new AccountMasterContext();
                string ConString = Context.Database.GetDbConnection().ConnectionString;

                // The SQL query without recursive DateRange CTE
                string query = @"
                    WITH DoctorAssignments AS (
                        SELECT
                            e.Id AS DoctorId,
                            e.TitleId,
                            d.Name AS DepartmentName,
                            mt.From AS AssignmentStartDate,
                            mt.To AS AssignmentEndDate,
                            ia.Status AS AdmissionStatus,
                            ia.PatientId,
                            ia.DateOfAdmission,
                            ia.LastModifiedDate AS DateOfDischarge,
                            mt.PrimaryDoctorId,
                            mt.SecondaryDoctorId,
                            mt.PrimaryCareGiverId,
                            mt.SecondaryCareGiverId
                        FROM medicalteams mt
                        INNER JOIN inpatientadmissions ia ON mt.AdmissionId = ia.Id
                        INNER JOIN employees e ON e.Id = mt.PrimaryDoctorId OR e.Id = mt.SecondaryDoctorId
                        INNER JOIN departments d ON d.Id = e.DepartmentId
                        WHERE e.IsServiceProvider = 1
                        AND ia.DateOfAdmission <= @EndDate
                        AND (ia.LastModifiedDate >= @StartDate OR ia.Status = 0)
                        AND d.Id = @DepartmentId
                    ),
                    DoctorStatus AS (
                        SELECT
                            da.DoctorId,
                            da.TitleId,
                            da.DepartmentName,
                            @DateUnion AS DateUnion,
                            CASE 
                                WHEN @DateUnion BETWEEN da.AssignmentStartDate AND COALESCE(da.AssignmentEndDate, @EndDate) THEN 'Assigned'
                                ELSE 'Unassigned'
                            END AS DoctorStatus
                        FROM DoctorAssignments da
                    ),
                    GroupedStatus AS (
                        SELECT
                            ds.DoctorId,
                            ds.TitleId,
                            ds.DepartmentName,
                            ds.DateUnion AS StatusDate,
                            ds.DoctorStatus,
                            ROW_NUMBER() OVER (PARTITION BY ds.DoctorId ORDER BY ds.DateUnion) - 
                            ROW_NUMBER() OVER (PARTITION BY ds.DoctorId, ds.DoctorStatus ORDER BY ds.DateUnion) AS GroupingKey
                        FROM DoctorStatus ds
                    )
                    SELECT
                        DoctorId,
                        TitleId AS DoctorName,
                        DepartmentName,
                        MIN(StatusDate) AS FromDate,
                        MAX(StatusDate) AS ToDate,
                        DoctorStatus
                    FROM GroupedStatus
                    GROUP BY DoctorId, TitleId, DepartmentName, DoctorStatus, GroupingKey
                    ORDER BY DoctorId, FromDate;
                ";

                // Generate the list of dates (DateRange) between startDate and endDate in C#
                List<DateTime> dateRange = new List<DateTime>();
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    dateRange.Add(date);
                }

                using (MySqlConnection connection = new MySqlConnection(ConString))
                {
                    connection.Open();

                    // Execute the query for each date in the range
                    foreach (var dateUnion in dateRange)
                    {
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            // Add parameters for startDate, endDate, departmentId, and DateUnion
                            command.Parameters.AddWithValue("@StartDate", startDate);
                            command.Parameters.AddWithValue("@EndDate", endDate);
                            command.Parameters.AddWithValue("@DepartmentId", departmentId);
                            command.Parameters.AddWithValue("@DateUnion", dateUnion);

                            // Fill the DataTable with the result
                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                            {
                                adapter.Fill(dataTable);
                            }
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the report.", ex);
            }
        }

        public DataTable CreateUnAssignCareTakerReport11092024First(long CompanyId, DateTime startDate, DateTime endDate, long departmentId)
        {
            try
            {
                DataTable dataTable = new DataTable();
                AccountMasterContext Context = new AccountMasterContext();
                string ConString = Context.Database.GetDbConnection().ConnectionString;

                // The query
                string query = @"
                        WITH DoctorAssignments AS (
                            SELECT
                                e.Id AS DoctorId,
                                e.TitleId,
                                d.Name AS DepartmentName,
                                mt.From AS AssignmentStartDate,
                                mt.To AS AssignmentEndDate,
                                ia.Status AS AdmissionStatus,
                                ia.PatientId,
                                ia.DateOfAdmission,
                                ia.LastModifiedDate AS DateOfDischarge,
                                mt.PrimaryDoctorId,
                                mt.SecondaryDoctorId,
                                mt.PrimaryCareGiverId,
                                mt.SecondaryCareGiverId
                            FROM medicalteams mt
                            INNER JOIN inpatientadmissions ia ON mt.AdmissionId = ia.Id
                            INNER JOIN employees e ON e.Id = mt.PrimaryDoctorId OR e.Id = mt.SecondaryDoctorId
                            INNER JOIN departments d ON d.Id = e.DepartmentId
                            WHERE e.IsServiceProvider = 1
                            AND ia.DateOfAdmission <= @EndDate
                            AND (ia.LastModifiedDate >= @StartDate OR ia.Status = 0)
                            AND d.Id = @DepartmentId
                        ),
                        DateRange AS (
                            SELECT @StartDate AS DateUnion
                            UNION ALL
                            SELECT DATE_ADD(DateUnion, INTERVAL 1 DAY)
                            FROM DateRange
                            WHERE DateUnion < @EndDate
                        ),
                        DoctorStatus AS (
                            SELECT
                                da.DoctorId,
                                da.TitleId,
                                da.DepartmentName,
                                dr.DateUnion,
                                CASE 
                                    WHEN dr.DateUnion BETWEEN da.AssignmentStartDate AND COALESCE(da.AssignmentEndDate, @EndDate) THEN 'Assigned'
                                    ELSE 'Unassigned'
                                END AS DoctorStatus
                            FROM DoctorAssignments da
                            CROSS JOIN DateRange dr
                            WHERE dr.DateUnion BETWEEN @StartDate AND @EndDate
                        ),
                        GroupedStatus AS (
                            SELECT
                                ds.DoctorId,
                                ds.TitleId,
                                ds.DepartmentName,
                                ds.DateUnion AS StatusDate,
                                ds.DoctorStatus,
                                ROW_NUMBER() OVER (PARTITION BY ds.DoctorId ORDER BY ds.DateUnion) - 
                                ROW_NUMBER() OVER (PARTITION BY ds.DoctorId, ds.DoctorStatus ORDER BY ds.DateUnion) AS GroupingKey
                            FROM DoctorStatus ds
                        )
                        SELECT
                            DoctorId,
                            TitleId AS DoctorName,
                            DepartmentName,
                            MIN(StatusDate) AS FromDate,
                            MAX(StatusDate) AS ToDate,
                            DoctorStatus
                        FROM GroupedStatus
                        GROUP BY DoctorId, TitleId, DepartmentName, DoctorStatus, GroupingKey
                        ORDER BY DoctorId, FromDate;
                    ";

                using (MySqlConnection connection = new MySqlConnection(ConString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add parameters for startDate, endDate, and departmentId
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        command.Parameters.AddWithValue("@DepartmentId", departmentId);

                        // Fill the DataTable with the result
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the report.", ex);
            }
        }

        private UnAssignCareTakerReport CreateUnAssignCareTakerReport(AccountMasterContext Context, MedicalTeam CareTakerInfo, InPatientAdmission Admission, ref DateTime? TempDate)
        {
            //using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    string connectionString = Context.Database.GetDbConnection().ConnectionString;
                    string query = @"
                        WITH DoctorAssignments AS (
                            SELECT
                                e.Id AS DoctorId,
                                e.TitleId,
                                d.Name AS DepartmentName,
                                mt.From AS AssignmentStartDate,
                                mt.To AS AssignmentEndDate,
                                ia.Status AS AdmissionStatus,
                                ia.PatientId,
                                ia.DateOfAdmission,
                                ia.LastModifiedDate AS DateOfDischarge,
                                mt.PrimaryDoctorId,
                                mt.SecondaryDoctorId,
                                mt.PrimaryCareGiverId,
                                mt.SecondaryCareGiverId
                            FROM medicalteams mt
                            INNER JOIN inpatientadmissions ia ON mt.AdmissionId = ia.Id
                            INNER JOIN employees e ON e.Id = mt.PrimaryDoctorId OR e.Id = mt.SecondaryDoctorId
                            INNER JOIN departments d ON d.Id = e.DepartmentId
                            WHERE e.IsServiceProvider = 1
                            AND ia.DateOfAdmission <= @EndDate
                            AND (ia.LastModifiedDate >= @StartDate OR ia.Status = 0)
                            AND d.Id = @DepartmentId
                        ),
                        DateRange AS (
                            SELECT @StartDate AS DateUnion
                            UNION ALL
                            SELECT DATE_ADD(DateUnion, INTERVAL 1 DAY)
                            FROM DateRange
                            WHERE DateUnion < @EndDate
                        ),
                        DoctorStatus AS (
                            SELECT
                                da.DoctorId,
                                da.TitleId,
                                da.DepartmentName,
                                dr.DateUnion,
                                CASE 
                                    WHEN dr.DateUnion BETWEEN da.AssignmentStartDate AND COALESCE(da.AssignmentEndDate, @EndDate) THEN 'Assigned'
                                    ELSE 'Unassigned'
                                END AS DoctorStatus
                            FROM DoctorAssignments da
                            CROSS JOIN DateRange dr
                            WHERE dr.DateUnion BETWEEN @StartDate AND @EndDate
                        ),
                        GroupedStatus AS (
                            SELECT
                                ds.DoctorId,
                                ds.TitleId,
                                ds.DepartmentName,
                                ds.DateUnion AS StatusDate,
                                ds.DoctorStatus,
                                ROW_NUMBER() OVER (PARTITION BY ds.DoctorId ORDER BY ds.DateUnion) - 
                                ROW_NUMBER() OVER (PARTITION BY ds.DoctorId, ds.DoctorStatus ORDER BY ds.DateUnion) AS GroupingKey
                            FROM DoctorStatus ds
                        )
                        SELECT
                            DoctorId,
                            TitleId AS DoctorName,
                            DepartmentName,
                            MIN(StatusDate) AS FromDate,
                            MAX(StatusDate) AS ToDate,
                            DoctorStatus
                        FROM GroupedStatus
                        GROUP BY DoctorId, TitleId, DepartmentName, DoctorStatus, GroupingKey
                        ORDER BY DoctorId, FromDate;
                    ";

                    // Define the date range and department ID (parameters)
                    DateTime startDate = new DateTime(2024, 6, 1);
                    DateTime endDate = new DateTime(2024, 6, 15);
                    long departmentId = 1; // Example department ID, change it as needed

                    if (!string.IsNullOrEmpty(query))
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                // Add the parameters
                                command.Parameters.AddWithValue("@StartDate", startDate);
                                command.Parameters.AddWithValue("@EndDate", endDate);
                                command.Parameters.AddWithValue("@DepartmentId", departmentId);

                                using (MySqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        // Retrieve the fields from the result set
                                        long doctorId = reader.GetInt64("DoctorId");
                                        string doctorName = reader.GetString("DoctorName");
                                        string departmentName = reader.GetString("DepartmentName");
                                        DateTime fromDate = reader.GetDateTime("FromDate");
                                        DateTime toDate = reader.GetDateTime("ToDate");
                                        string doctorStatus = reader.GetString("DoctorStatus");

                                        // Display the results
                                        Console.WriteLine($"Doctor ID: {doctorId}, Doctor Name: {doctorName}, Department: {departmentName}, " +
                                                          $"From: {fromDate.ToShortDateString()}, To: {toDate.ToShortDateString()}, Status: {doctorStatus}");
                                    }
                                }
                                //using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                                //{
                                //    adapter.Fill(dataTable);
                                //}
                            }
                        }
                    }
                    
                }
                catch (MySqlException ex)
                {
                    int errorCode = ex.Number;
                    if (ex.HResult == -2147467259)
                    {
                        Console.WriteLine("Query was stopped: " + ex.HResult);
                    }
                }
            }
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
            }
                // Query to get doctor assignments with the specified date range
                

            UnAssignCareTakerReport CareTaker = new UnAssignCareTakerReport();

            CareTaker.AdmissionId = CareTakerInfo.AdmissionId != null ? CareTakerInfo.AdmissionId.ToString() : string.Empty;
            
            if (Admission.CurrentMedicalTeam != null)
            {
                Employee Emp = EmployeeManager.Instance.GetEmployeeInfoById(Admission.CurrentMedicalTeam?.PrimaryDoctorId ?? 0);
                CareTaker.DepartmentOfConsultant = Emp?.Department?.Name ?? "";
                CareTaker.AuthorizedDr = Admission.CurrentMedicalTeam.AuthorizedByDoctor?.Name ?? string.Empty;
                CareTaker.Name = Admission.CurrentMedicalTeam.PrimaryDoctor?.Name ?? string.Empty;
                //CareTaker.Name = Admission.CurrentMedicalTeam.PrimaryCareGiver?.Name ?? string.Empty;
            }
            else
            {
                CareTaker.DepartmentOfConsultant = "";
                CareTaker.AuthorizedDr = CareTakerInfo.AuthorizedByDoctor?.Name ?? string.Empty;
                CareTaker.Name = CareTakerInfo.PrimaryDoctor?.Name ?? string.Empty;
            }
            
            CareTaker.Notes = CareTakerInfo.Notes ?? string.Empty;

            return CareTaker;
        }
    }
    public class UnAssignCareTakerReport
    {
        public String AdmissionId { get; set; }
        public String Name { get; set; }
        public String Age { get; set; }
        public String DepartmentOfConsultant { get; set; }
        public String Address { get; set; }
        public String AssignDate { get; set; }
        public String Notes { get; set; }
        public String TaskStatus { get; set; }
        public String AuthorizedDr { get; set; }
    }
}
