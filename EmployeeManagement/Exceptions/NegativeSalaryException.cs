namespace if_employee.Exceptions
{
    public class NegativeSalaryException : Exception
    {
        public NegativeSalaryException() : base("Salary can't be negative number") { }
    }
}
