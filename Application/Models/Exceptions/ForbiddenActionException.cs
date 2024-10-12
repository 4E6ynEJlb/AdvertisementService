using Domain.Models.ApplicationModels;
using Domain.Models.Exceptions;
using System.Net;

namespace Application.Models.Exceptions
{
    public class ForbiddenActionException : ServiceException
    {
        public ForbiddenActionException() : 
            base(ErrorsMessages.FORBIDDEN_ACTION, HttpStatusCode.Forbidden)
        { }
    }
}
