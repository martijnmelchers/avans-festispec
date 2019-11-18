namespace Festispec.Models
{
    public class ContactDetails : Entity
    {
        public int Id { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ContactPerson ContactPerson { get; set; }

        public virtual Employee Employee { get; set; }
    }
}