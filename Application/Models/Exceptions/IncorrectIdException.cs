using Domain.Models.ApplicationModels;
using Domain.Models.Exceptions;
using System.Net;

namespace Application.Models.Exceptions
{
    public class IncorrectIdException : ServiceException
    {
        public IncorrectIdException() : 
            base(ErrorsMessages.INCORRECT_ID, HttpStatusCode.NotFound)
        { }
    }
}
