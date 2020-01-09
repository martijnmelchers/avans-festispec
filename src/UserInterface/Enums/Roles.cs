using System;
using System.Collections.Generic;
using System.Linq;
using Festispec.Models;

namespace Festispec.UI.Enums
{
    public static class Roles
    {
        public static IEnumerable<Role> AvailableRoles => Enum.GetValues(typeof(Role)).OfType<Role>().ToList();
    }
}