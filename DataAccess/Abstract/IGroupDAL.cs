using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IGroupDAL : IRepositoryBase<Group>
    {
        new Task AddAsync(Group model);
        Task InviteUser(Guid groupId, List<string> userIds);
    }
}
