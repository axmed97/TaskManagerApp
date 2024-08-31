using Core.Utilities.Results.Abstract;
using Entities.DTOs.GroupDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IGroupUserService
    {
        Task<IResult> AcceptGroupUserAsync(string userId, AcceptGroupUserDTO model);


    }
}
