namespace if_employee.Interfaces;

public interface ISalaryService
{
    List<EmployeeContractDates> GetContractDates();
    void AddWorkdayStart(int id, DateTime workdayStart);
    void SetWorkdayEnd(int employeeId, int hours, int minutes);
    List<WorkdayTime> GetWorkdayList();
    List<EmployeeMonthlyReport> GetMonthlyReports();
    List<Employee> GetEmployees();
    void CalculateSalary(int id, int hours, int minutes, EmployeeMonthlyReport employeeReport);
}