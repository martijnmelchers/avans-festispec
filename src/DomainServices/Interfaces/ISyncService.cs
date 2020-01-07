using System.Collections.Generic;
using System.Threading.Tasks;
using Festispec.Models;
using Festispec.Models.EntityMapping;

namespace Festispec.DomainServices.Interfaces
{
    public interface ISyncService<T> where T : Entity
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetEntity(int entityId);
        Task<T> GetEntityAsync(int entityId);
        
        void AddEntity(T entity);
        void AddEntities(IEnumerable<T> entities);
        
        void SaveChanges();
        void SaveChangesAsync();
        
        FestispecContext GetSyncContext();
        void Flush();
    }
}