using Domain.Models.ApplicationModels;
using Domain.Models.Exceptions;
using System.Net;

namespace Application.Models.Exceptions
{
    public class IncorrectCredentialsException : ServiceException
    {
        public IncorrectCredentialsException() : 
            base(ErrorsMessages.INCORRECT_CREDENTIALS, HttpStatusCode.Unauthorized)
        { }
    }
}
