using if_employee.Exceptions;
using if_employee.Interfaces;

namespace if_employee
{
    public class Company : ICompany
    {
        private readonly ISalaryService _salaryService;

        public Company(string companyName, ISalaryService salaryService)
        {
            Name = companyName;
            _salaryService = salaryService;
        }

        public string Name { get; }
        public Employee[] Employees => _salaryService.GetEmployees().ToArray();

        public void AddEmployee(Employee employee, DateTime contractStartDate)
        {
            var employeeId = employee.Id;

            if (_salaryService.GetEmployees().Any(e => e.Id == employeeId))
            {
                throw new DuplicateEmployeeIdException();
            }

            if (employee.Id < 0)
            {
                throw new NegativeEmployeeIdException();
            }

            if (string.IsNullOrEmpty(employee.FullName))
            {
                throw new NoNameException();
            }

            if (employee.HourlySalary < 0)
            {
                throw new NegativeSalaryException();
            }

            if (employee.HourlySalary == 0) 
            {
                throw new ZeroHourlySalaryException();
            }
            
            _salaryService.GetEmployees().Add(employee);
            _salaryService.GetContractDates().Add(new EmployeeContractDates(employeeId, contractStartDate));
        }

        public void RemoveEmployee(int employeeId, DateTime contractEndDate)
        {
            if (_salaryService.GetEmployees().FirstOrDefault(e => e.Id == employeeId) == null)
            {
                throw new NonexistentEmployeeIdException();
            }

            _salaryService.GetEmployees().RemoveAll(e => e.Id == employeeId);
            _salaryService.GetContractDates()
                .First(c => c.Id == employeeId && c.ContractEndDate == null)
                .ContractEndDate = contractEndDate;
        }

        public void ReportHours(int employeeId, DateTime dateAndTime, int hours, int minutes)
        {
            _salaryService.AddWorkdayStart(employeeId, dateAndTime);

            var employeeReport = GetEmployeeReport(employeeId, dateAndTime);

            if (hours < 0 || minutes < 0)
            {
                throw new NegativeTimeException();
            }

            _salaryService.SetWorkdayEnd(employeeId, hours, minutes);

            _salaryService.CalculateSalary(employeeId, hours, minutes, employeeReport);
        }

        public EmployeeMonthlyReport[] GetMonthlyReport(DateTime periodStartDate, DateTime periodEndDate)
        {
            return _salaryService.GetMonthlyReports().Where(r =>
                r.Year >= periodStartDate.Year && r.Year <= periodEndDate.Year 
                && (periodStartDate.Month < periodEndDate.Month
                    ? r.Month >= periodStartDate.Month && r.Month <= periodEndDate.Month
                    : r.Month >= periodStartDate.Month && r.Month <= 12
                       || r.Month >= 1 && r.Month <= periodEndDate.Month)).ToArray();
        }

        private EmployeeMonthlyReport GetEmployeeReport(int id, DateTime dateAndTime)
        {
            var employeeReport = _salaryService.GetMonthlyReports()
                                    .FirstOrDefault(r => r.EmployeeId == id &&
                                     r.Year == dateAndTime.Year &&
                                     r.Month == dateAndTime.Month);

            if (employeeReport == null)
            {
                var report = new EmployeeMonthlyReport
                {
                    EmployeeId = id,
                    Year = dateAndTime.Year,
                    Month = dateAndTime.Month,
                    Salary = 0
                };
                _salaryService.GetMonthlyReports().Add(report);
                return report;
            }

            return employeeReport;
        }
    }
}
