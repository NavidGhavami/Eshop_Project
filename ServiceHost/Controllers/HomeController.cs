using Microsoft.AspNetCore.Mvc;
using ServiceHost.Models;
using System.Diagnostics;
using Eshop.Application.Services.Interfaces;
using Eshop.Application.Services.Implementations;
using Eshop.Domain.Dtos.Contact;
using GoogleReCaptcha.V3.Interface;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Controllers
{
    public class HomeController : SiteBaseController
    {
        private readonly ISiteSettingService _siteSettingService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IContactService _contactService;

        public HomeController(ISiteSettingService siteSettingService, 
            ICaptchaValidator captchaValidator, IContactService contactService)
        {
            _siteSettingService = siteSettingService;
            _captchaValidator = captchaValidator;
            _contactService = contactService;
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

        #region ContactUs

        [HttpGet("contact-us")]
        public async Task<IActionResult> ContactUs()
        {
            var setting = await _siteSettingService.GetDefaultSiteSetting();
            if (setting == null)
            {
                TempData[ErrorMessage] = "تنظیمات سایت یافت نشد";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.SiteSetting = setting;
            return View();
        }

        [HttpPost("contact-us"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(CreateContactUsDto contact)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(contact.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(contact);
            }

            if (ModelState.IsValid)
            {
                var ip = HttpContext.GetUserIp();
                await _contactService.CreateContactUs(contact, ip, /*User.GetUserId()*/ null);
                TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        #endregion

    }
}
