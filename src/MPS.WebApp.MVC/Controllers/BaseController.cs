using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MPS.Common.DTOs;
using MPS.Data.EF.Interfaces.UOW;
using NToastNotify;

namespace MPS.WebApp.MVC.Controllers
{
    public class BaseController : Controller
    {
        #region constructor
        protected readonly IUnitOfWork _db;
        protected readonly IToastNotification _notification;
        protected readonly ILogger _logger;

        public BaseController(IToastNotification notification, IUnitOfWork db = null, ILogger logger = null)
        {
            _db = db;
            _notification = notification;
            _logger = logger;
        }
        public BaseController()
        {

        }
        #endregion

        protected void AddToastMessage(string message,ToastrNotificationType type)
        { 
            switch (type)
            {
                case ToastrNotificationType.Success:
                    _notification.AddSuccessToastMessage(message);
                    break; 
                case ToastrNotificationType.Warning:
                    _notification.AddWarningToastMessage(message);
                    break;
                case ToastrNotificationType.Error:
                    _notification.AddErrorToastMessage(message);
                    break; 
                case ToastrNotificationType.Info:
                    _notification.AddInfoToastMessage(message);
                    break; 
            }
        }

    }
}