using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.GroupDTOs
{
    public class AcceptGroupUserDTO
    {
        public Guid GroupId { get; set; }
        public bool IsAccept { get; set; }
    }
}
