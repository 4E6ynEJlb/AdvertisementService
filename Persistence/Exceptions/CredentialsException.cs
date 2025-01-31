using Domain.Models.ApplicationModels;
using Domain.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Exceptions
{
    public class CredentialsException: ServiceException
    {
        public CredentialsException(Exception innerException) :
            base(innerException, ErrorsMessages.Credentials, HttpStatusCode.BadRequest)
        { }
    }
}
