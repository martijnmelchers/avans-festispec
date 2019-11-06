using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Festispec.Models;

namespace Festispec.Models
{
    public class Certificate
    {
        public int Id { get; set; }

        public string CertificateTitle { get; set; }

        public DateTime CertificationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}