using Asp.Versioning;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.DTOs.GroupDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;
        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(AddGroupDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _groupService.CreateAsync(userId, model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [MapToApiVersion("1.0")]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(Guid id, UpdateGroupDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _groupService.UpdateAsync(id, userId, model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("invite")]
        [Authorize]
        public async Task<IActionResult> Invite(GroupInviteDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _groupService.InviteUserAsync(userId, model);
            if(result.Success)
                return Ok(result);
            return BadRequest(result);
        }


    }
}
