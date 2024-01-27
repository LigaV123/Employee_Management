using if_employee.Interfaces;

namespace if_employee
{
    public class WorkdayTime : IWorkdayTime
    {
        public WorkdayTime(int id, DateTime workdayStart)
        {
            Id = id;
            WorkdayStart = workdayStart;
        }

        public int Id { get; set; }
        public DateTime WorkdayStart { get; set; }
        public DateTime? WorkdayEnd { get; set; }
    }
}
