namespace if_employee.Exceptions
{
    public class NonexistentEmployeeIdException : Exception
    {
        public NonexistentEmployeeIdException() : base("There is no employee with that Id") { }
    }
}
