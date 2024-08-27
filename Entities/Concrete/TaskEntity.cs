using Entities.Common;
using Entities.Enum;

namespace Entities.Concrete
{
    public class TaskEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        public TaskEntityStatus TaskEntityStatus { get; set; }
        public PriorityStatus PriorityStatus { get; set; }
        public string CreatedUserId { get; set; }
        public User CreateBy { get; set; }
        public string? UpdatedUserId { get; set; }
        public User? UpdatedBy { get; set; }
        public string? DeletedUserId { get; set; }
        public User? DeletedBy { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
    }
}
