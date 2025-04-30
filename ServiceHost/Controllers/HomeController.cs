using Microsoft.AspNetCore.Mvc;
using ServiceHost.Models;
using System.Diagnostics;

namespace ServiceHost.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }

    }
}
