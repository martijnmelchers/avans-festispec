using Festispec.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface ISicknessService
    {
        Task AddAbsense(Employee employee, string reason, DateTime endDate);
        Task EndAbsense(int employeeId);
        bool IsSick(int employeeId);
    }
}
