using System.Net;

namespace Domain.Models.Exceptions
{
    public abstract class ServiceException:Exception
    {
        public HttpStatusCode StatusCode;
        public ServiceException(string message, HttpStatusCode code) : base(message)
        {
            StatusCode = code;
        }
        public ServiceException(Exception innerException, string message, HttpStatusCode code) : base(message, innerException)
        {
            StatusCode = code;
        }
    }
}
