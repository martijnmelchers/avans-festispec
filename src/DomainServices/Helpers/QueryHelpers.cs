using System;

namespace Festispec.DomainServices.Helpers
{
    public static class QueryHelpers
    {

        public static DateTime TruncateTime(DateTime oldDateTime)
        {
            return oldDateTime.Date;
        }
        
    }
}