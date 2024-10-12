using Domain.Models.ApplicationModels;
using Domain.Models.Exceptions;
using System.Net;

namespace Application.Models.Exceptions
{
    public class IdInTokenNotFoundException:ServiceException
    {
        public IdInTokenNotFoundException() : 
            base(ErrorsMessages.ID_IN_TOKEN_NOT_FOUND, HttpStatusCode.BadRequest)
        { }
    }
}
