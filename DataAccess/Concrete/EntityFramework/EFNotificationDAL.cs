using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.NotificationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFNotificationDAL : EFRepositoryBase<Notification, AppDbContext>, INotificationDAL
    {
        public async Task AddAsync(AddNotificationDTO model)
        {
            using var context = new AppDbContext();

            foreach (var item in model.ToIds)
            {
                Notification notification = new()
                {
                    Description = model.Description,
                    SenderId = model.SenderId,
                    Title = model.Title,
                    ToId = item
                };
                await context.Notifications.AddAsync(notification);
            }
            await context.SaveChangesAsync();
        }
    }
}
