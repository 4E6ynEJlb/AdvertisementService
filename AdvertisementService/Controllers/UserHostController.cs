using Application.Interfaces;
using Application.Models.ApplicationModels;
using Domain.Models.ApplicationModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller for controlling users for host
    /// </summary>
    [Route("User")]
    [ApiController]
    [Authorize(Policies.HOST)]
    public class UserHostController : ControllerBase
    {
        private readonly IUserHostService _userService;
        public UserHostController(IUserHostService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Assigns user as admin
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("[action]")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AssignAsAdmin(Guid userId, CancellationToken cancellationToken)
        {
            if (userId == Guid.Empty)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            await _userService.AssignAsAdminAsync(userId, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Unassigns user as admin
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("[action]")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UnassignAsAdmin(Guid userId, CancellationToken cancellationToken)
        {
            if (userId == Guid.Empty)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            await _userService.UnassignAsAdminAsync(userId, cancellationToken);
            return Ok();
        }
    }
}
