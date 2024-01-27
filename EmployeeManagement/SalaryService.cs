using if_employee.Interfaces;

namespace if_employee
{
    public class SalaryService : ISalaryService
    {
        private readonly List<EmployeeContractDates> _contracts;
        private readonly List<WorkdayTime> _workdays;
        private readonly List<EmployeeMonthlyReport> _monthlyReports;
        private readonly List<Employee> _employees;

        public SalaryService(
            List<EmployeeContractDates> contracts, 
            List<WorkdayTime> workdays,
            List<EmployeeMonthlyReport> monthlyReports,
            List<Employee> employees)
        {
            _contracts = contracts;
            _workdays = workdays;
            _monthlyReports = monthlyReports;
            _employees = employees;
        }

        public List<EmployeeContractDates> GetContractDates()
        {
            return _contracts;
        }

        public void AddWorkdayStart(int id, DateTime workdayStart)
        {
            _workdays.Add(new WorkdayTime(id, workdayStart));
        }

        public void SetWorkdayEnd(int id, int hours, int minutes)
        {
            var workday = _workdays.First(t => t.Id == id &&
                                 t.WorkdayEnd == null);
            workday.WorkdayEnd = workday.WorkdayStart.AddHours(hours).AddMinutes(minutes);
        }

        public List<WorkdayTime> GetWorkdayList()
        {
            return _workdays;
        }

        public List<EmployeeMonthlyReport> GetMonthlyReports()
        {
            return _monthlyReports;
        }

        public List<Employee> GetEmployees()
        {
            return _employees;
        }

        public void CalculateSalary(int id, int hours, int minutes, EmployeeMonthlyReport employeeReport)
        {
            var hourlySalary = _employees.First(e => e.Id == id).HourlySalary;

            var salaryInWorkedMinutes = minutes / 60m * hourlySalary;
            var salaryInWorkedDay = hours * hourlySalary + salaryInWorkedMinutes;

            employeeReport.Salary += Math.Round(salaryInWorkedDay, 2);
        }
    }
}
