using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Areas.Panel.Controllers
{
    [Authorize]
    [Area("Panel")]
    [Route("/Admin/[action]")]
    public class AdminCPController : Controller
    {
        private readonly ILogger<AdminCPController> _logger;

        public AdminCPController(ILogger<AdminCPController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}