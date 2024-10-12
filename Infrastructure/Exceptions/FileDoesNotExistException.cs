using Domain.Models.ApplicationModels;
using Domain.Models.Exceptions;
using System.Net;

namespace Infrastructure.Exceptions
{
    public class FileDoesNotExistException : ServiceException
    {
        public FileDoesNotExistException(Exception innerException) :
            base(innerException, ErrorsMessages.FILE_DOES_NOT_EXIST, HttpStatusCode.NotFound)
        { }
    }
}
