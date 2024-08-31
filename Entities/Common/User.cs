using Core.Entities.Concrete;
using Entities.Concrete;

namespace Entities.Common
{
    public class User : AppUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public ICollection<TaskEntity> TaskEntities { get; set; }
        public ICollection<GroupUser> GroupUsers { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
