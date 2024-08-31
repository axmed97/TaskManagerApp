using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Abstract;
using Entities.DTOs.GroupDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class GroupUserManager : IGroupUserService
    {
        private readonly IGroupUserDAL _groupUserDAL;

        public GroupUserManager(IGroupUserDAL groupUserDAL)
        {
            _groupUserDAL = groupUserDAL;
        }

        public async Task<IResult> AcceptGroupUserAsync(string userId, AcceptGroupUserDTO model)
        {
            var data = await _groupUserDAL.GetAsync(x => x.GroupId == model.GroupId && x.UserId == userId);
            if (data == null)
                return new ErrorResult(HttpStatusCode.NotFound);

            data.IsAccept = model.IsAccept;
            _groupUserDAL.Update(data);
            return new SuccessResult(HttpStatusCode.OK);
        }

        public IDataResult<GetGroupUserDTO> GetGroupUser(Guid id)
        {
            var data = _groupUserDAL.Get(x => x.Id == id);
            if (data == null)
                return new ErrorDataResult<GetGroupUserDTO>(HttpStatusCode.NotFound);

            GetGroupUserDTO model = new()
            {
                Id = id,
                UserId = data.UserId,
                GroupId = data.GroupId,
                IsAccept = data.IsAccept
            };

            return new SuccessDataResult<GetGroupUserDTO>(data: model, HttpStatusCode.OK);
        }
    }
}
