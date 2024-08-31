using Entities.Common;

namespace Entities.Concrete
{
    public class GroupUser : BaseEntity
    {
        public bool IsAccept { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
