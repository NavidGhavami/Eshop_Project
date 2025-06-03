using Microsoft.AspNetCore.Mvc;
using ServiceHost.Models;
using System.Diagnostics;
using Eshop.Application.Services.Interfaces;

namespace ServiceHost.Controllers
{
    public class HomeController : SiteBaseController
    {
        private readonly ISiteSettingService _siteSettingService;

        public HomeController(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("about-us")]
        public async Task<IActionResult> AboutUs()
        {
            var about = await _siteSettingService.GetAboutUs();
            return View(about);
        }

    }
}
