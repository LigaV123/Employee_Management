using FluentAssertions;
using if_employee;
using if_employee.Exceptions;
using if_employee.Interfaces;
using Moq;
using Moq.AutoMock;

namespace Company.Test
{
    [TestClass]
    public class CompanyTests
    {
        private AutoMocker _mocker;
        private ICompany _company;
        private List<Employee> _employeeList;
        private List<EmployeeContractDates> _employeeContractDates;
        private List<WorkdayTime> _workdayTimeList;
        private List<EmployeeMonthlyReport> _monthlyReports;
        private Employee _employeeDefault;
        private const string COMPANY_NAME = "default";

        [TestInitialize]
        public void SetUp()
        {
            _mocker = new AutoMocker();
            _employeeList = new List<Employee>();
            _employeeContractDates = new List<EmployeeContractDates>();
            _workdayTimeList = new List<WorkdayTime>();
            _monthlyReports = new List<EmployeeMonthlyReport>();
            var salaryService = _mocker.GetMock<ISalaryService>();
            _company = new if_employee.Company(COMPANY_NAME, salaryService.Object);

            _employeeDefault = new Employee
            {
                Id = 1,
                FullName = "Default Name",
                HourlySalary = 5m
            };

            _mocker.GetMock<ISalaryService>()
                .Setup(s => s.GetEmployees())
                .Returns(_employeeList);

            _mocker.GetMock<ISalaryService>()
                .Setup(s => s.GetContractDates())
                .Returns(_employeeContractDates);

            _mocker.GetMock<ISalaryService>()
                .Setup(s => s.GetWorkdayList())
                .Returns(_workdayTimeList);

            _mocker.GetMock<ISalaryService>()
                .Setup(s => s.GetMonthlyReports())
                .Returns(_monthlyReports);
        }

        [TestMethod]
        public void Company_GivenNameWhenCreated_HasGivenName()
        {
            _company.Name.Should().Be(COMPANY_NAME);
            _company.Name.Should().NotBeNullOrEmpty().And.NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void AddEmployee_ValidEmployeeDataGiven_EmployeeAdded()
        {
            _company.AddEmployee(_employeeDefault, DateTime.Now);

            _company.Employees.Length.Should().Be(1);
        }

        [TestMethod]
        public void AddEmployee_GivenEmployeeWithId1_EmployeeAddedWithId1()
        {
            _company.AddEmployee(_employeeDefault, DateTime.Now);

            _company.Employees.First().Id.Should().Be(1);
        }

        [TestMethod]
        public void AddEmployee_GivenEmployeeWithFullName_EmployeeAddedWithFullName()
        {
            _company.AddEmployee(_employeeDefault, DateTime.Now);

            _company.Employees.First().FullName.Should().Be("Default Name");
        }

        [TestMethod]
        public void AddEmployee_GivenEmployeeWithHourlySalary5Euro_EmployeeAddedWithHourlySalary5Euro()
        {
            _company.AddEmployee(_employeeDefault, DateTime.Now);

            _company.Employees.First().HourlySalary.Should().Be(5);
        }

        [TestMethod]
        public void AddEmployee_GivenContractStartDate_ContractStartDateIsSet()
        {
            var date = DateTime.Now;

            _company.AddEmployee(_employeeDefault, date);

            _employeeContractDates.Count.Should().Be(1);
            _employeeContractDates[0].ContractStartDate.Should().Be(date);
        }

        [TestMethod]
        public void AddEmployee_GivenDuplicateEmployeeId_ThrowsDuplicateEmployeeIdException()
        {
            _company.AddEmployee(_employeeDefault, DateTime.Now);

            var secondEmployee = new Employee
            {
                Id = _employeeDefault.Id,
                FullName = "Default Name",
                HourlySalary = 5m
            };

            Action action = () => _company.AddEmployee(secondEmployee, DateTime.Now);

            action.Should().Throw<DuplicateEmployeeIdException>();
        }

        [TestMethod]
        public void AddEmployee_WithNegativeEmployeeId_ThrowsNegativeEmployeeIdException()
        {
            _employeeDefault.Id = -1;

            Action action = () => _company.AddEmployee(_employeeDefault, DateTime.Now);

            action.Should().Throw<NegativeEmployeeIdException>();
        }

        [TestMethod]
        public void AddEmployee_WithoutFullName_ThrowNoNameException()
        {
            _employeeDefault.FullName = "";

            Action action = () => _company.AddEmployee(_employeeDefault, DateTime.Now);

            action.Should().Throw<NoNameException>();
        }

        [TestMethod]
        public void AddEmployee_WithNegativeSalary_ThrowNegativeSalaryException()
        {
            _employeeDefault.HourlySalary = -5m;

            Action action = () => _company.AddEmployee(_employeeDefault, DateTime.Now);

            action.Should().Throw<NegativeSalaryException>();
        }

        [TestMethod]
        public void AddEmployee_WithZeroHourlySalary_ThrowZeroHourlySalaryException()
        {
            _employeeDefault.HourlySalary = 0m;

            Action action = () => _company.AddEmployee(_employeeDefault, DateTime.Now);

            action.Should().Throw<ZeroHourlySalaryException>();
        }

        [TestMethod]
        public void RemoveEmployee_WithProvidedEmployeeId_EmployeeIsRemoved()
        {
            var date = DateTime.Now;
            _employeeList.Add(_employeeDefault);
            _employeeContractDates.Add(new EmployeeContractDates(_employeeDefault.Id, date));

            _company.RemoveEmployee(_employeeDefault.Id, date);

            _company.Employees.Length.Should().Be(0);
        }

        [TestMethod]
        public void RemoveEmployee_WithNonexistentId_ThrowsNonexistentEmployeeIdException()
        {
            _employeeList.Add(_employeeDefault);

            Action action = () => _company.RemoveEmployee(10, DateTime.Now);

            _company.Employees.Length.Should().Be(1);
            action.Should().Throw<NonexistentEmployeeIdException>();
        }

        [TestMethod]
        public void RemoveEmployee_ProvidesContractEndDate_ContractEndDateIsSet()
        {
            var date = DateTime.Now;
            _employeeList.Add(_employeeDefault);
            _employeeContractDates.Add(new EmployeeContractDates(_employeeDefault.Id, date.AddYears(-1)));

            _company.RemoveEmployee(_employeeDefault.Id, date);

            _employeeContractDates.Count.Should().Be(1);
            _employeeContractDates[0].ContractEndDate.Should().NotBeNull();
            _employeeContractDates[0].ContractEndDate.Should().Be(date);
        }

        [TestMethod]
        public void ReportHours_EmployeeProvidesWorkdayStartDate_WorkdayStartDateIsSet()
        {
            var date = DateTime.Now;
            _employeeList.Add(_employeeDefault);

            _company.ReportHours(_employeeDefault.Id, date, 0, 0);

            _mocker.GetMock<ISalaryService>().Verify(s => 
                s.AddWorkdayStart(_employeeDefault.Id, date), Times.Once);
        }

        [TestMethod]
        public void ReportHours_WithWorkdayStartDate_StartsReport()
        {
            var date = DateTime.Now;
            _employeeList.Add(_employeeDefault);

            _company.ReportHours(_employeeDefault.Id, date, 0, 0);

            _monthlyReports.Count.Should().Be(1);
        }

        [TestMethod]
        public void ReportHours_EmployeeProvidesWorkedHoursAndMinutes_WorkdayEndTimeIsSet()
        {
            var hours = 1;
            var minutes = 10;
            var date = DateTime.Now;
            _employeeList.Add(_employeeDefault);

            _company.ReportHours(_employeeDefault.Id, date, hours, minutes);

            _mocker.GetMock<ISalaryService>().Verify(s =>
                s.AddWorkdayStart(_employeeDefault.Id, date), Times.Once);
        }

        [TestMethod]
        public void ReportHours_EmployeeProvidesWorked1HourAnd30Minutes_CalculatesSalary()
        {
            var hours = 1;
            var minutes = 30;
            var date = DateTime.Now;
            _employeeList.Add(_employeeDefault);

            _company.ReportHours(_employeeDefault.Id, date, hours, minutes);

            _mocker.GetMock<ISalaryService>().Verify(s => 
                s.CalculateSalary(_employeeDefault.Id, hours, minutes, It.IsAny<EmployeeMonthlyReport>()), Times.Once);
        }

        [TestMethod]
        public void ReportHours_EmployeeProvidesNegativeWorkedHoursAndMinutes_ThrowsNegativeTimeException()
        {
            var hours = -1;
            var minutes = -10;
            var date = DateTime.Now;
            _employeeList.Add(_employeeDefault);

            Action action = () => _company.ReportHours(_employeeDefault.Id, date, hours, minutes);
            
            action.Should().Throw<NegativeTimeException>();
        }

        [TestMethod]
        public void GetMonthlyReport_StartAndEndPeriodWithOneEmployee_ReturnsEmployeeReport()
        {
            var date = DateTime.Now;
            var startPeriod = date.AddDays(-7);

            var report = new EmployeeMonthlyReport
            {
                EmployeeId = 1,
                Year = date.Year,
                Month = date.Month,
                Salary = 0
            };

            _mocker.GetMock<ISalaryService>().Setup(s => s.GetMonthlyReports())
                .Returns(_monthlyReports);

            _monthlyReports.Add(report);

            var result = _company.GetMonthlyReport(startPeriod, date);

            result.Length.Should().Be(1);
        }

        [TestMethod]
        public void GetMonthlyReport_StartAndEndPeriodWithOneEmployee_ReturnsEmployeeReportFor3Months()
        {
            var date = DateTime.Now;
            var startPeriod = date.AddMonths(-3);

            var report1 = new EmployeeMonthlyReport
            {
                EmployeeId = 1,
                Year = date.Year,
                Month = date.AddMonths(-4).Month,
                Salary = 0
            };

            var report2 = new EmployeeMonthlyReport
            {
                EmployeeId = 1,
                Year = date.Year,
                Month = date.AddMonths(-3).Month,
                Salary = 0
            };

            var report3 = new EmployeeMonthlyReport
            {
                EmployeeId = 1,
                Year = date.Year,
                Month = date.AddMonths(-2).Month,
                Salary = 0
            };

            var report4 = new EmployeeMonthlyReport
            {
                EmployeeId = 1,
                Year = date.Year,
                Month = date.AddMonths(-1).Month,
                Salary = 0
            };

            _mocker.GetMock<ISalaryService>().Setup(s => s.GetMonthlyReports())
                .Returns(_monthlyReports);

            _monthlyReports.Add(report1);
            _monthlyReports.Add(report2);
            _monthlyReports.Add(report3);
            _monthlyReports.Add(report4);

            var result = _company.GetMonthlyReport(startPeriod, date);

            result.Length.Should().Be(3);
        }

        [TestMethod]
        public void GetMonthlyReport_SameStartAndEndPeriodForTwoEmployee_ReturnsEmployeeReportForTwoEmployees()
        {
            var date = DateTime.Now;
            var startPeriod = date.AddMonths(-3);

            var report1 = new EmployeeMonthlyReport
            {
                EmployeeId = 1,
                Year = date.Year,
                Month = date.AddMonths(-1).Month,
                Salary = 0
            };

            var report2 = new EmployeeMonthlyReport
            {
                EmployeeId = 2,
                Year = date.Year,
                Month = date.AddMonths(-1).Month,
                Salary = 0
            };

            var report3 = new EmployeeMonthlyReport
            {
                EmployeeId = 1,
                Year = date.Year,
                Month = date.AddMonths(-2).Month,
                Salary = 0
            };

            var report4 = new EmployeeMonthlyReport
            {
                EmployeeId = 2,
                Year = date.Year,
                Month = date.AddMonths(-2).Month,
                Salary = 0
            };

            _mocker.GetMock<ISalaryService>().Setup(s => s.GetMonthlyReports())
                .Returns(_monthlyReports);

            _monthlyReports.Add(report1);
            _monthlyReports.Add(report2);
            _monthlyReports.Add(report3);
            _monthlyReports.Add(report4);

            var result = _company.GetMonthlyReport(startPeriod, date);
            var uniqueEmployeeId = result.ToList().Select(r => r.EmployeeId).Distinct().ToList();

            result.Length.Should().Be(4);
            uniqueEmployeeId.Count.Should().Be(2);
            uniqueEmployeeId[0].Should().Be(1);
            uniqueEmployeeId[1].Should().Be(2);
        }
    }
}