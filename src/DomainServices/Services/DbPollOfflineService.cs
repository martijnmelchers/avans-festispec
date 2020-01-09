using System.Data.SqlClient;
using Festispec.Models.EntityMapping;

namespace Festispec.DomainServices.Services
{
    public class DbPollOfflineService : IOfflineService
    {
        public bool IsOnline { get; }
        
        public DbPollOfflineService(FestispecContext context)
        {
            try
            {
                context.Database.Connection.Open();
                IsOnline = true;
            }
            catch (SqlException)
            {
                IsOnline = false;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}