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
    /// Controller for working with users' personal advertisements
    /// </summary>
    [Route("Advertisement")]
    [ApiController]
    [Authorize(Policies.USER)]
    public class AdvertisementUserController : ControllerBase
    {
        private readonly IAdvertisementUserService _advertisementService;
        public AdvertisementUserController(IAdvertisementUserService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        /// <summary>
        /// Gets advertisement by its id
        /// </summary>
        /// <param name="id">Advertisement id</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Advertisement</returns>
        [AllowAnonymous]
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(AdvertisementOutputModel), 200)]
        public async Task<IActionResult> GetAdvertisement(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            return Ok(await _advertisementService.GetAdvertisementAsync(id, cancellationToken));
        }

        /// <summary>
        /// Gets advertisements and their pages count by options 
        /// </summary>
        /// <param name="options">Search options</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Advertisements and pages count</returns>
        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(AdvertisementsPagesOutput), 200)]
        public async Task<IActionResult> GetAdvertisements(GetAdvertisementsOptions options, CancellationToken cancellationToken)
        {
            if (options == null)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            return Ok(await _advertisementService.GetAdvertisementsAsync(options, cancellationToken));
        }

        /// <summary>
        /// Gets advertisements by current user
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Advertisements</returns>
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(AdvertisementOutputModel[]), 200)]
        public async Task<IActionResult> GetUserAdvertisements(CancellationToken cancellationToken)
        {
            return Ok(await _advertisementService.GetUserAdvertisementsAsync(HttpContext.GetUserId(), cancellationToken));
        }

        /// <summary>
        /// Gets attached to advertisement image by its name
        /// </summary>
        /// <param name="name">Image name</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Image</returns>
        [AllowAnonymous]
        [Route("images/{name}")]
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetImage(string name, CancellationToken cancellationToken)
        {
            if (name == null)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            Response.Headers.TryAdd("content-disposition", "inline");
            MemoryStream stream = await _advertisementService.GetImageAsync(name, cancellationToken);
            byte[] bytes = stream.ToArray();
            string? contentType = await _advertisementService.GetImageContentTypeAsync(name, cancellationToken);
            if (contentType == null)
                return Problem(ErrorsMessages.MISSING_CONTENT_TYPE);
            return new FileStreamResult(stream, contentType);
        }

        /// <summary>
        /// Creates advertisement
        /// </summary>
        /// <param name="advertisement">Advertisement data</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Advertisement id</returns>
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> CreateAdvertisement(AdvertisementInputModel advertisement, CancellationToken cancellationToken)
        {
            if (advertisement == null)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            return Ok(await _advertisementService.CreateAdvertisementAsync(HttpContext.GetUserId(), advertisement, cancellationToken));
        }

        /// <summary>
        /// Edits advertisement
        /// </summary>
        /// <param name="id">Advertisement id</param>
        /// <param name="advertisement">New advertisement data</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPatch]
        [ProducesResponseType(200)]
        public async Task<IActionResult> EditAdvertisement(Guid id, AdvertisementInputModel advertisement, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || advertisement == null)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            await _advertisementService.EditAdvertisementAsync(id, HttpContext.GetUserId(), advertisement, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Changes advertisement average rating
        /// </summary>
        /// <param name="id">Advertisement id</param>
        /// <param name="mark">Mark [1-5]</param>
        /// <param name="cancellationToken"></param>
        /// <returns>New average rating</returns>
        [Route("[action]")]
        [HttpPatch]
        [ProducesResponseType(typeof(double), 200)]
        public async Task<IActionResult> RateAdvertisement(Guid id, int mark, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            return Ok(await _advertisementService.RateAdvertisementAsync(id, mark, cancellationToken));
        }

        /// <summary>
        /// Attaches image to advertisement
        /// </summary>
        /// <param name="id">Advertisement id</param>
        /// <param name="file">Image</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Link to image</returns>
        [Route("[action]")]
        [HttpPatch]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> AttachImage(Guid id, IFormFile file, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            return Ok(await _advertisementService.AttachImageAsync(id, HttpContext.GetUserId(), file, cancellationToken));
        }

        /// <summary>
        /// Deletes attached to advertisement image
        /// </summary>
        /// <param name="id">Advertisement id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpDelete]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteImage(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            await _advertisementService.DeleteImageAsync(id, HttpContext.GetUserId(), cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Deletes advertisement
        /// </summary>
        /// <param name="id">Advertisement id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpDelete]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteAdvertisement(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return BadRequest(ErrorsMessages.EMPTY_ARGUMENT);
            await _advertisementService.DeleteAdvertisementAsync(id, HttpContext.GetUserId(), cancellationToken);
            return Ok();
        }
    }
}
