using System.Collections.Generic;
using System.Threading.Tasks;
using Festispec.Models;

namespace Festispec.DomainServices.Interfaces
{
    public interface IFestivalService : ISyncable
    {
        Task<Festival> CreateFestival(Festival festival, int customerId);
        Festival GetFestival(int festivalId);
        ICollection<Festival> GetFestivals();
        Task UpdateFestival(Festival festival);
        Task RemoveFestival(int festivalId);
    }
}