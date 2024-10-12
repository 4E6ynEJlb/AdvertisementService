using Domain.Models.ApplicationModels;
using Domain.Models.Exceptions;
using System.Net;

namespace Persistence.Exceptions
{
    public class CriterionNotImplementedException : ServiceException
    {
        public CriterionNotImplementedException() : 
            base(ErrorsMessages.CRITERION_NOT_IMPLEMENTED, HttpStatusCode.NotImplemented)
        { }
    }
}
