using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Festispec.DomainServices.Interfaces;
using Festispec.Models.EntityMapping;

namespace Festispec.DomainServices.Services
{
    [ExcludeFromCodeCoverage]
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