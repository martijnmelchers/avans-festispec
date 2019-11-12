namespace Festispec.Models.Interfaces
{
    public interface IAnswer<out TAnswer>
    {
        TAnswer GetAnswer();
    }
}