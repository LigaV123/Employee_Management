using FluentAssertions;
using if_employee;

namespace SalaryService.Test
{
    [TestClass]
    public class SalaryServiceTests
    {
        private if_employee.SalaryService _service;
        private List<Employee> _employees;
        private List<EmployeeContractDates> _contracts;
        private List<WorkdayTime> _workdays;
        private List<EmployeeMonthlyReport> _monthlyReports;
        private Employee _employee;

        [TestInitialize]
        public void Setup()
        {
            _employees = new List<Employee>();
            _contracts = new List<EmployeeContractDates>();
            _workdays = new List<WorkdayTime>();
            _monthlyReports = new List<EmployeeMonthlyReport>();
            _service = new if_employee.SalaryService(_contracts, _workdays, _monthlyReports, _employees);

            _employee = new Employee
            {
                Id = 1,
                FullName = "Default Name",
                HourlySalary = 5m
            };
        }

        [TestMethod]
        public void GetContractDates_WhenCalled_ReturnsEmployeeContractDateList()
        {
            var result = _service.GetContractDates();

            result.Count.Should().Be(0);
        }

        [TestMethod]
        public void AddWorkdayStart_EmployeeProvidesIdAndStartDate_WorkdaysStartIsAdded()
        {
            var date = new DateTime(2019, 05, 19, 9, 15, 0);

            _service.AddWorkdayStart(_employee.Id, date);

            _workdays.Count.Should().Be(1);
            _workdays[0].WorkdayStart.Should().Be(date);
            _workdays[0].WorkdayEnd.Should().Be(null);
        }

        [TestMethod]
        public void SetWorkdayEnd_EmployeeProvidesIdAndEndDate_WorkdaysEndIsSet()
        {
            var startDate = new DateTime(2019, 05, 19, 9, 15, 0);
            _workdays.Add(new WorkdayTime(_employee.Id, startDate));

            _service.SetWorkdayEnd(_employee.Id, 1, 30);

            var result = new DateTime(2019, 05, 19, 10, 45, 0);

            _workdays.Count.Should().Be(1);
            _workdays[0].WorkdayStart.Should().Be(startDate);
            _workdays[0].WorkdayEnd.Should().Be(result);
        }

        [TestMethod]
        public void GetWorkdayList_WhenCalled_ReturnsWorkdayTimeList()
        {
            var result = _service.GetWorkdayList();

            result.Count.Should().Be(0);
        }

        [TestMethod]
        public void GetMonthlyReports_WhenCalled_ReturnsEmployeeMonthlyReportList()
        {
            var result = _service.GetMonthlyReports();

            result.Count.Should().Be(0);
        }

        [TestMethod]
        public void GetEmployees_WhenCalled_ReturnsEmployeeList()
        {
            var result = _service.GetEmployees();

            result.Count.Should().Be(0);
        }

        [TestMethod]
        public void CalculateSalary_EmployeeProvidedWorkedHoursAndMinutes_CalculatesAndSavesSalaryInEmployeeReport()
        {
            _employees.Add(_employee);
            var report = new EmployeeMonthlyReport
            {
                EmployeeId = _employee.Id,
                Year = 2019, 
                Month = 05,
                Salary = 0
            };

            _service.CalculateSalary(_employee.Id, 1, 30, report);

            report.Salary.Should().Be(7.50m);
        }

        [TestMethod]
        public void CalculateSalary_EmployeeProvidesZeroWorkedHoursAndMinutes_SalaryIsZero()
        {
            _employees.Add(_employee);
            var report = new EmployeeMonthlyReport
            {
                EmployeeId = _employee.Id,
                Year = 2019,
                Month = 05,
                Salary = 0
            };
            
            _service.CalculateSalary(_employee.Id,0, 0, report);

            report.Salary.Should().Be(0);
        }

        [TestMethod]
        public void CalculateSalary_SalaryIs10AndEmployeeProvidesZeroWorkedHoursAndMinutes_SalaryStayTheSame()
        {
            _employees.Add(_employee);
            var report = new EmployeeMonthlyReport
            {
                EmployeeId = _employee.Id,
                Year = 2019,
                Month = 05,
                Salary = 10
            };

            _service.CalculateSalary(_employee.Id, 0, 0, report);

            report.Salary.Should().Be(10);
        }
    }
}