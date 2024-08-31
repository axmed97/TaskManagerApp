using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFGroupDAL : EFRepositoryBase<Group, AppDbContext>, IGroupDAL
    {
        public new async Task AddAsync(Group model)
        {
            await using var context = new AppDbContext();
            await context.AddAsync(model);
            await context.SaveChangesAsync();
        }

        public async Task InviteUser(Guid groupId, List<string> userIds)
        {
            using var context = new AppDbContext();
            var group = context.Groups.Find(groupId);

            List<GroupUser> groupUsers = new();

            foreach (var item in userIds)
            {
                GroupUser groupUser = new()
                {
                    GroupId = groupId,
                    UserId = item,
                    IsAccept = false
                };
                groupUsers.Add(groupUser);
            }
            await context.GroupUsers.AddRangeAsync(groupUsers);
            await context.SaveChangesAsync();
        }
    }
}
