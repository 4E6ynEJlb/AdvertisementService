using Application.Models.Exceptions;
using System.Security.Claims;

namespace API.Extensions
{
    internal static class HttpContextExtensions
    {
        internal static Guid GetUserId(this HttpContext context)
        {
            string? userGuidString = context.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                throw new IdInTokenNotFoundException();
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                throw new InvalidIdInTokenException();
            return userGuid;
        }
    }
}
