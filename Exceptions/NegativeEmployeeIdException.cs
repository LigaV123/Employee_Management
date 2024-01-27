namespace if_employee.Exceptions
{
    public class NegativeEmployeeIdException : Exception
    {
        public NegativeEmployeeIdException() : base("Employee Id should be positive number") { }
    }
}
