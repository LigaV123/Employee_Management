namespace if_employee.Exceptions
{
    public class NoNameException : Exception
    {
        public NoNameException() : base("Employee should give it's name") { }
    }
}
