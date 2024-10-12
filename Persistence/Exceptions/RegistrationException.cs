using Domain.Models.ApplicationModels;
using Domain.Models.Exceptions;
using System.Net;

namespace Persistence.Exceptions
{
    public class RegistrationException : ServiceException
    {
        public RegistrationException(Exception innerException) :
            base(innerException, ErrorsMessages.REGISTRATION, HttpStatusCode.BadRequest)
        { }
    }
}
