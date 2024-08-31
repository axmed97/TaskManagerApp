using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.NotificationDTOs
{
    public class AddNotificationDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> ToIds { get; set; }
        public string SenderId { get; set; }
    }
}
