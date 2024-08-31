using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Abstract;
using Entities.DTOs.NotificationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class NotificationManager : INotificationService
    {
        private readonly INotificationDAL _notificationDAL;

        public NotificationManager(INotificationDAL notificationDAL)
        {
            _notificationDAL = notificationDAL;
        }

        public async Task<IResult> CreateAsync(AddNotificationDTO model)
        {
            await _notificationDAL.AddAsync(model);
            return new SuccessResult(HttpStatusCode.Created);
        }
    }
}
