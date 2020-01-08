using System;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models
{
    public class Certificate : Entity
    {
        public int Id { get; set; }

        [Required, MaxLength(45)]
        public string CertificateTitle { get; set; }

        [Required]
        public DateTime CertificationDate { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        public virtual Employee Employee { get; set; }

        public override bool Validate()
        {
            return CertificationDate < ExpirationDate && base.Validate();
        }
    }
}