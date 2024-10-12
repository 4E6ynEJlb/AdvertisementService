using API.Extensions;
using Application.Interfaces;
using Application.Models.ApplicationModels;
using Domain.Models.ApplicationModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller for working with advertisements for admins
    /// </summary>
    [Route("Advertisement")]
    [ApiController]
    [Authorize(Policies.ADMIN)]
    public class AdvertisementAdminController : ControllerBase
    {
        private readonly IAdvertisementAdminService _adminService;
        public AdvertisementAdminController(IAdvertisementAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Deleting any advertisement on behalf of an admin
        /// </summary>
        /// <param name="id">Advertisement id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpDelete]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteAdvertisementAdmin(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            await _adminService.DeleteAdvertisementAdminAsync(id, HttpContext.GetUserId(), cancellationToken);
            return Ok();
        }
    }
}
