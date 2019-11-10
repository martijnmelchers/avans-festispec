namespace Festispec.Models
{
    public class ContactDetails : Entity
    {
        public int Id { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }
    }
}