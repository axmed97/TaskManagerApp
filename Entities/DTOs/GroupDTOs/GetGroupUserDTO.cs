using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.GroupDTOs
{
    public class GetGroupUserDTO
    {
        public Guid Id { get; set; }
        public bool IsAccept { get; set; }
        public Guid GroupId { get; set; }
        public string UserId { get; set; }
    }
}
