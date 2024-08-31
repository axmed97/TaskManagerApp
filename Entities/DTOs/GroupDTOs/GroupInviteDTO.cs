using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.GroupDTOs
{
    public class GroupInviteDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public List<string> UserIds { get; set; }
    }
}
