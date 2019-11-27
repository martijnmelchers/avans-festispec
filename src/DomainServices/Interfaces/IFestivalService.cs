using Festispec.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IFestivalService
    {
        Task<Festival> CreateFestival(string festivalName, string description, Customer customer);
        Festival GetFestival(int festivalId);
        Task SaveChanges();
        Task RemoveQuestionnaire(int festivalId);
    }
}
