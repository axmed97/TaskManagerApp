using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ToId { get; set; }
        public User To { get; set; }
        public string SenderId { get; set; }
        public User Sender { get; set; }
    }
}
