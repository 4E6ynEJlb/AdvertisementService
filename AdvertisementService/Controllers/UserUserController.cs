using API.Extensions;
using Application.Interfaces;
using Application.Models.ApplicationModels;
using Application.Models.ViewModels;
using Domain.Models.ApplicationModels;
using Domain.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller for working with themselves for users
    /// </summary>
    [Route("User")]
    [ApiController]
    [Authorize(Policies.USER)]
    public class UserUserController : ControllerBase
    {
        private readonly IUserUserService _userService;
        public UserUserController(IUserUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets current user
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>User</returns>
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(UserOutputModel), 200)]
        public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
        {
            Guid userId = HttpContext.GetUserId();
            return Ok(await _userService.GetUserAsync(userId, cancellationToken));
        }

        /// <summary>
        /// Edits credentials for current user
        /// </summary>
        /// <param name="credentials">New credentials</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("[action]")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> EditCredentials(Credentials credentials, CancellationToken cancellationToken)
        {
            if (credentials == null)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            Guid userId = HttpContext.GetUserId();
            await _userService.EditCredentialsAsync(userId, credentials, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Edits current user's name
        /// </summary>
        /// <param name="name">New name</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("[action]")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> EditName(string name, CancellationToken cancellationToken)
        {
            if (name == null)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            Guid userId = HttpContext.GetUserId();
            await _userService.EditNameAsync(userId, name, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Deletes current user
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("[action]")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
        {
            Guid userId = HttpContext.GetUserId();
            await _userService.DeleteUserAsync(userId, cancellationToken);
            return Ok();
        }
    }
}
