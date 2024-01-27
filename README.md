Think about it as a service for calculating employee salaries based on reported time.
Implement rules which seem logical to you.

## Goal 

Implement Employee salary service in code using the TDD approach

## Requirements

- A company can have any number of employees.
- New employees can be recruited, and existing employees can leave a company.
- The same employee can return to the company.
- An employee is reporting worked hours daily.
- You must calculate the monthly salary for each employee.
- Use C# language and IDE that is convenient for you
- Use TDD approach
- Think about OOP design patterns and S.O.L.I.D. principles
- In case of error, throw different type of exceptions for each situation
- It is not allowed to change the given code.
- Comments and code must be in English language
- No need for UI and database
- As a result we expect the solution with source code

## We are giving some interfaces and classes

```
public interface ICompany
{
    /// <summary>
    /// Name of the company.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// List of the employees that are working for the company.
    /// </summary>
    Employee[] Employees { get; }

    /// <summary>
    /// Adds new employee from the given date. Employee Id must be unique.
    /// </summary>
    /// <param name="employee">Employee to add.</param>
    /// <param name="contractStartDate">Employee work start date and time.</param>
    void AddEmployee(Employee employee, DateTime contractStartDate);

    /// <summary>
    /// Remove employee from the company at the given date.
    /// </summary>
    /// <param name="employeeId">Id of the employee.</param>
    /// <param name="contractEndDate">Employee work end date and time.</param>
    void RemoveEmployee(int employeeId, DateTime contractEndDate);

    /// <summary> 
    /// Report worked time at given day and time. 
    /// If an employee reports 1 hour and 30 minutes at 13:00, it means that the employee was working from 13:00 to 14:30.
    /// </summary>
    /// <param name="employeeId">Id of the employee.</param>
    /// <param name="dateAndTime">Date when work was started.</param>
    /// <param name="hours">Full hours.</param>
    /// <param name="minutes">Full minutes.</param>
    void ReportHours(int employeeId, DateTime dateAndTime, int hours, int minutes);

    /// <summary> 
    /// Get a full report for each employee where data is available for each month. Assume that there is no overtime.
    /// </summary>
    /// <param name="periodStartDate">Report start date.</param>
    /// <param name="periodEndDate">Report end date.</param>
    EmployeeMonthlyReport[] GetMonthlyReport(DateTime periodStartDate, DateTime periodEndDate);
}


public class Employee
{
    /// <summary>
    /// Unique ID of the employee.
    /// </summary> 
    public int Id { get; set; }

    /// <summary>
    /// Employee full name.
    /// </summary> 
    public string FullName { get; set; }

    /// <summary>
    /// Hourly salary of worked full hour. Use proportion for time smaller than 1 hour.
    /// </summary> 
    public decimal HourlySalary { get; set; }
}

public class EmployeeMonthlyReport
{
    public int EmployeeId { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public decimal Salary { get; set; }
}


```
