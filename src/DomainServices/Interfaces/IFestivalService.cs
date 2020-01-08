using System.Collections.Generic;
using System.Threading.Tasks;
using Festispec.Models;

namespace Festispec.DomainServices.Interfaces
{
    public interface IFestivalService
    {
        Task<Festival> CreateFestival(Festival festival);
        Task<Festival> GetFestivalAsync(int festivalId);
        Festival GetFestival(int festivalId);
        ICollection<Festival> GetFestivals();
        Task UpdateFestival(Festival festival);
        Task RemoveFestival(int festivalId);
    }
}