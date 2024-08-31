using Core.Utilities.Results.Abstract;
using Entities.DTOs.GroupDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IGroupService
    {
        Task<IResult> CreateAsync(string userId, AddGroupDTO model);
        Task<IResult> UpdateAsync(Guid id, string userId, UpdateGroupDTO model);
        Task<IResult> InviteUserAsync(string userId, GroupInviteDTO model);
    }
}
