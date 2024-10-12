using Domain.Models.ApplicationModels;
using Domain.Models.Exceptions;
using System.Net;

namespace Application.Models.Exceptions
{
    public class IncorrectFileFormatException : ServiceException
    {
        public IncorrectFileFormatException() : 
            base(ErrorsMessages.INCORRECT_FILE_FORMAT, HttpStatusCode.BadRequest)
        { }
    }
}
