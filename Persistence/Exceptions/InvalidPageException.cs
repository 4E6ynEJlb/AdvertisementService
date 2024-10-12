using Domain.Models.ApplicationModels;
using Domain.Models.Exceptions;
using System.Net;

namespace Persistence.Exceptions
{
    public class InvalidPageException : ServiceException
    {
        public InvalidPageException() : 
            base(ErrorsMessages.INVALID_PAGE, HttpStatusCode.BadRequest)
        { }
    }
}
