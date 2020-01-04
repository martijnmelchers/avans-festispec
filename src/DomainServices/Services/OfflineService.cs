using System.Data.Entity;
using System.Data.SqlClient;
using Festispec.Models.EntityMapping;

namespace Festispec.DomainServices.Services
{
    public class OfflineService
    {
        public bool IsOnline { get; }
        
        public OfflineService(FestispecContext context)
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