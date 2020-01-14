using Festispec.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface ISicknessService
    {
        Task<Availability> AddAbsence(int employeeId, string reason, DateTime? endDate);
        Task EndAbsence(int employeeId);
        bool IsSick(int employeeId);

    }
}
