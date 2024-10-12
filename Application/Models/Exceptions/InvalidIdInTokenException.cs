using Domain.Models.ApplicationModels;
using Domain.Models.Exceptions;
using System.Net;

namespace Application.Models.Exceptions
{
    public class InvalidIdInTokenException : ServiceException
    {
        public InvalidIdInTokenException() : 
            base(ErrorsMessages.INVALID_ID_IN_TOKEN, HttpStatusCode.BadRequest)
        { }
    }
}
