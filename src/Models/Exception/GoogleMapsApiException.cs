namespace Festispec.Models.Exception
{
    public class GoogleMapsApiException : System.Exception
    {
        public GoogleMapsApiException(string message) : base(message)
        {
        }

        public GoogleMapsApiException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public GoogleMapsApiException()
        {
        }
    }
}
