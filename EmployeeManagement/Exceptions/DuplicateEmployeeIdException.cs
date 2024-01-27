namespace if_employee.Exceptions
{
    public class DuplicateEmployeeIdException : Exception
    {
        public DuplicateEmployeeIdException() : base("This employee Id already exists") { }
    }
}
