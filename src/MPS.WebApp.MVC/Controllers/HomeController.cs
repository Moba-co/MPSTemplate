using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MPS.WebApp.MVC.Models;
using NToastNotify;

namespace MPS.WebApp.MVC.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(ILogger<HomeController> logger,IToastNotification notofication) : base(notofication,logger : logger)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
