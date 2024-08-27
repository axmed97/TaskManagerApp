using Entities.Common;

namespace Entities.Concrete
{
    public class Group : BaseEntity
    {
        public string Name { get; set; }
        public string CreatedUserId { get; set; }
        public User User { get; set; }
        public ICollection<TaskEntity> TaskEntities { get; set; }
        public ICollection<GroupUser> GroupUsers { get; set; }
    }
}
