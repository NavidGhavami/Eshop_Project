using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Contact;
using Eshop.Domain.Dtos.Site;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.Administration.Controllers
{
    public class HomeController : AdminBaseController
    {
        #region Constructor

        private readonly IUserService _userService;
        private readonly ISiteSettingService _siteService;
        private readonly IContactService _contactService;

        public HomeController(IUserService userService, ISiteSettingService siteService,
            IContactService contactService)
        {
            _userService = userService;
            _siteService = siteService;
            _contactService = contactService;
        }

        #endregion


        #region Index

        public async Task<IActionResult> Index()
        {
            return View();
        }
        #endregion

        #region Site Setting

        [HttpGet("site-setting")]
        public async Task<IActionResult> SiteSetting()
        {
            var setting = await _siteService.GetDefaultSiteSetting();
            return View(setting);
        }

        [HttpGet("site-setting/{settingId}")]
        public async Task<IActionResult> EditSiteSetting(long settingId)
        {
            var setting = await _siteService.GetSiteSettingForEdit(settingId);
            return View(setting);
        }

        [HttpPost("site-setting/{settingId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSiteSetting(EditSiteSettingDto edit)
        {
            var user = await _userService.GetUserById(User.GetUserId());
            var username = user.FirstName + " " + user.LastName;
            var result = await _siteService.EditSiteSetting(edit, username);

            switch (result)
            {
                case false:
                    return null;
                case true:
                    return RedirectToAction("SiteSetting", "Home");
            }


        }



        #endregion

        #region ContactUs

        [HttpGet("contact-us-list")]
        public async Task<IActionResult> ContactUsList(FilterContactUs filter)
        {
            var contactUs = await _contactService.FilterContactUs(filter);
            return View(contactUs);
        }

        #endregion

        #region AboutUs

        [HttpGet("about-us")]
        public async Task<IActionResult> AboutUsList()
        {
            var aboutUs = await _contactService.GetAll();
            return View(aboutUs);
        }

        [HttpGet("create-about-us")]
        public async Task<IActionResult> CreateAboutUs()
        {
            return View();
        }

        [HttpPost("create-about-us"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAboutUs(CreateAboutUsDto create)
        {
            var result = await _contactService.CreateAboutUs(create);

            switch (result)
            {
                case CreateAboutUsResult.Error:
                    TempData[ErrorMessage] = "در افزودن اطلاعات خطایی رخ داد";
                    break;
                case CreateAboutUsResult.Success:
                    TempData[SuccessMessage] = "عملیات ثبت اطلاعات با موفقیت انجام شد";
                    return RedirectToAction("AboutUsList", "Home");
            }

            return View();
        }

        [HttpGet("edit-about-us/{id}")]
        public async Task<IActionResult> EditAboutUs(long id)
        {
            var result = await _contactService.GetAboutUsForEdit(id);
            return View(result);
        }

        [HttpPost("edit-about-us/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAboutUs(EditAboutUsDto edit, string headerTitle)
        {
            var user = await _userService.GetUserById(User.GetUserId());
            var username = user.FirstName + " " + user.LastName;
            var result = await _contactService.EditAboutUs(edit, username);

            switch (result)
            {
                case EditAboutUsResult.Error:
                    TempData[ErrorMessage] = "فرمت تصویر نادرست می باشد";
                    break;
                case EditAboutUsResult.NotFound:
                    TempData[WarningMessage] = "اطلاعات مورد نظر یافت نشد";
                    break;
                case EditAboutUsResult.Success:
                    TempData[SuccessMessage] = "ویرایش اطلاعات با موفقیت انجام شد";
                    return RedirectToAction("AboutUsList", "Home");

            }

            return View();
        }

        #endregion
    }
}
