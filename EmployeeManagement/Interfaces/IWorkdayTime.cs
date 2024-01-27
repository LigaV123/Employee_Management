namespace if_employee.Interfaces
{
    public interface IWorkdayTime
    {
        int Id { get; set; }
        DateTime WorkdayStart { get; set; }
        DateTime? WorkdayEnd { get; set; }
    }
}
