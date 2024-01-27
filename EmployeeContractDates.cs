using if_employee.Interfaces;

namespace if_employee
{
    public class EmployeeContractDates : IEmployeeContractDates
    {
        public EmployeeContractDates(int id, DateTime contractStartDate)
        {
            Id = id;
            ContractStartDate = contractStartDate;
        }

        public int Id { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
    }
}
