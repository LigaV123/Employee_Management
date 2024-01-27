namespace if_employee.Exceptions
{
    public class NegativeTimeException : Exception
    {
        public NegativeTimeException() : base("Hoers and minutes should be positive number") { }
    }
}
