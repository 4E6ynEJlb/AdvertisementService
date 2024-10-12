using Application.Interfaces;
using Application.Models.ApplicationModels;
using Application.Models.ViewModels;
using Domain.Models.ApplicationModels;
using Domain.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Controller for logging in and registration
    /// </summary>
    [Route("Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Loggs in host
        /// </summary>
        /// <param name="credentials">Host credentials</param>
        /// <returns>JWT token</returns>
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult AuthHost(Credentials credentials)
        {
            if (credentials == null)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            _authService.AuthHost(credentials);
            return Ok(GetToken("Host", Roles.HOST));
        }

        /// <summary>
        /// Loggs in user/admin
        /// </summary>
        /// <param name="credentials">User/admin credentials</param>
        /// <param name="cancellationToken"></param>
        /// <returns>JWT token</returns>
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> AuthUser(Credentials credentials, CancellationToken cancellationToken)
        {
            if (credentials == null)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            UserOutputModel user = await _authService.AuthUserAsync(credentials, cancellationToken);
            string token = user.IsAdmin ? GetToken(user.Id.ToString(), Roles.ADMIN) : GetToken(user.Id.ToString(), Roles.USER);
            return Ok(token);
        }

        /// <summary>
        /// Registers user
        /// </summary>
        /// <param name="user">User registration data</param>
        /// <param name="cancellationToken"></param>
        /// <returns>JWT token</returns>
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> Register(RegisterUserModel user, CancellationToken cancellationToken)
        {
            if (user == null)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            Guid id = await _authService.RegisterUserAsync(user, cancellationToken);
            return Ok(GetToken(id.ToString(), Roles.USER));
        }

        private string GetToken(string id, string role)
        {
            DateTime utcNow = DateTime.UtcNow;
            JwtSecurityToken? token = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Client,
                notBefore: utcNow,
                claims: GetIdentity(id, role).Claims,
                expires: utcNow.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private ClaimsIdentity GetIdentity(string id, string role)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, id),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
