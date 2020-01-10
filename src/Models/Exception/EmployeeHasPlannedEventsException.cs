namespace Festispec.Models.Exception
{
    public class EmployeeHasPlannedEventsException : System.Exception
    {
        public EmployeeHasPlannedEventsException()
        {
        }

        public EmployeeHasPlannedEventsException(string message) : base(message)
        {
        }

        public EmployeeHasPlannedEventsException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}