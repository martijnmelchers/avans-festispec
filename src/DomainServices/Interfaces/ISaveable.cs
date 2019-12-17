using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface ISaveable
    {
        public Task<int> SaveChangesAsync();
        public int SaveChanges();
    }
}