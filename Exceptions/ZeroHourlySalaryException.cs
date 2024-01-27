namespace if_employee.Exceptions
{
    public class ZeroHourlySalaryException : Exception
    {
        public ZeroHourlySalaryException() : base("Hourly salary cannot be zero") { }
    }
}
