using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.GroupDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class GroupManager : IGroupService
    {
        private readonly IGroupDAL _groupDAL;
        private readonly INotificationService _notificationService;
        public GroupManager(IGroupDAL groupDAL, INotificationService notificationService)
        {
            _groupDAL = groupDAL;
            _notificationService = notificationService;
        }

        public async Task<IResult> CreateAsync(string userId, AddGroupDTO model)
        {
            Group group = new()
            {
                Id = Guid.NewGuid(),
                CreatedUserId = userId,
                Name = model.Name,
                GroupUsers =
                [
                    new(){ UserId = userId, IsAccept = true }
                ]
            };

            await _groupDAL.AddAsync(group);
            return new SuccessResult(HttpStatusCode.Created);
        }

        public async Task<IResult> InviteUserAsync(string userId, GroupInviteDTO model)
        {
            var data = _groupDAL.Get(x => x.Id == model.Id);
            if (data == null)
                return new ErrorResult(HttpStatusCode.NotFound);

            await _groupDAL.InviteUser(model.Id, model.UserIds);
            await _notificationService.CreateAsync(new()
            {
                SenderId = userId,
                Description = "Test",
                Title = "Test",
                ToIds = model.UserIds
            });
            return new SuccessResult(HttpStatusCode.OK);
        }

        public async Task<IResult> UpdateAsync(Guid id, string userId, UpdateGroupDTO model)
        {
            var data = await _groupDAL.GetAsync(x => x.Id == id);
            if (data == null)
                return new ErrorResult(HttpStatusCode.NotFound);

            if(data.CreatedUserId == userId)
            {
                data.Name = model.Name;
                _groupDAL.Update(data);
                return new SuccessResult(HttpStatusCode.OK);
            }
            return new ErrorResult(statusCode: HttpStatusCode.BadRequest, message: "Access Denied");
        }
    }
}
