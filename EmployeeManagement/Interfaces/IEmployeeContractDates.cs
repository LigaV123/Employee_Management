namespace if_employee.Interfaces;

public interface IEmployeeContractDates
{
    int Id { get; set; }
    DateTime ContractStartDate { get; set; }
    DateTime? ContractEndDate { get; set; }
}