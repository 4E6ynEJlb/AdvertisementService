using Domain.Models.ApplicationModels;
using Domain.Models.Exceptions;
using System.Net;

namespace Application.Models.Exceptions
{
    public class IncorrectMarkException : ServiceException
    {
        public IncorrectMarkException() :
            base(ErrorsMessages.INCORRECT_MARK, HttpStatusCode.BadRequest)
        { }
    }
}
