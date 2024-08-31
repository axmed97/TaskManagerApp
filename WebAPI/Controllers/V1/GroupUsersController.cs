using Asp.Versioning;
using Business.Abstract;
using Entities.DTOs.GroupDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupUsersController : ControllerBase
    {
        private readonly IGroupUserService _groupUserService;

        public GroupUsersController(IGroupUserService groupUserService)
        {
            _groupUserService = groupUserService;
        }

        [MapToApiVersion("1.0")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AcceptGroupUser(AcceptGroupUserDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _groupUserService.AcceptGroupUserAsync(userId, model);
            if(result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
