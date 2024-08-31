using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs.NotificationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface INotificationDAL : IRepositoryBase<Notification>
    {
        Task AddAsync(AddNotificationDTO model);
    }
}
