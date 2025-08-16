using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Contact;
using Eshop.Domain.Dtos.Site;
using Eshop.Domain.Dtos.Site.Banner;
using Eshop.Domain.Dtos.Site.Slider;
using Microsoft.AspNetCore.Components.Forms;
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
        private readonly ISiteImagesService _siteImagesService;

        public HomeController(IUserService userService, ISiteSettingService siteService,
            IContactService contactService, ISiteImagesService siteImagesService)
        {
            _userService = userService;
            _siteService = siteService;
            _contactService = contactService;
            _siteImagesService = siteImagesService;
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

        #region Slider

        #region Slider List

        [HttpGet("slider-list")]
        public async Task<IActionResult> SliderList()
        {
            var sliders = await _siteImagesService.GetAllSlider();
            return View(sliders);
        }

        #endregion

        #region Create Slider

        [HttpGet("create-slider")]
        public async Task<IActionResult> CreateSlider()
        {
            return View();
        }

        [HttpPost("create-slider")]
        public async Task<IActionResult> CreateSlider(CreateSliderDto slider, IFormFile sliderImage,
            IFormFile mobileSliderImage)
        {
            var result = await _siteImagesService.CreateSlider(slider, sliderImage, mobileSliderImage);

            switch (result)
            {
                case CreateSliderResult.Error:
                    TempData[ErrorMessage] = "در افزودن اسلایدر خطایی رخ داد";
                    break;
                case CreateSliderResult.Success:
                    TempData[SuccessMessage] = "عملیات ثبت اسلایدر با موفقیت انجام شد";
                    return RedirectToAction("SliderList", "Home");
            }

            return View();
        }


        #endregion

        #region Edit Slider

        [HttpGet("edit-slider/{sliderId}")]
        public async Task<IActionResult> EditSlider(long sliderId)
        {
            var slider = await _siteImagesService.GetSliderForEdit(sliderId);
            return View(slider);
        }

        [HttpPost("edit-slider/{sliderId}")]
        public async Task<IActionResult> EditSlider(EditSliderDto edit, IFormFile sliderImage,
            IFormFile mobileSliderImage)
        {
            var user = await _userService.GetUserById(User.GetUserId());
            var result = await _siteImagesService.EditSlider(edit, sliderImage, mobileSliderImage);
            switch (result)
            {
                case EditSliderResult.Error:
                    TempData[ErrorMessage] = "در ویرایش اسلایدر خطایی رخ داد";
                    break;
                case EditSliderResult.NotFound:
                    TempData[WarningMessage] = "اسلایدر مورد نظر یافت نشد";
                    break;
                case EditSliderResult.Success:
                    TempData[SuccessMessage] = "ویرایش اسلایدر با موفقیت انجام شد";
                    return RedirectToAction("SliderList", "Home");
            }

            return View();
        }

        #endregion

        #region Active Slider

        [HttpGet("active-slider/{sliderId}")]
        public async Task<IActionResult> ActiveSlider(long sliderId)
        {
            var result = await _siteImagesService.ActiveSlider(sliderId);
            if (result)
            {
                TempData[SuccessMessage] = "اسلایدر با موفقیت فعال شد";
                return RedirectToAction("SliderList", "Home", new { area = "Administration" });
            }

            TempData[ErrorMessage] = "خطا در فعال سازی اسلایدر";
            return RedirectToAction("SliderList", "Home", new { area = "Administration" });
        }

        #endregion

        #region Deactive Slider

        [HttpGet("deactive-slider/{sliderId}")]
        public async Task<IActionResult> DeactiveSlider(long sliderId)
        {
            var result = await _siteImagesService.DeActiveSlider(sliderId);
            if (result)
            {
                TempData[SuccessMessage] = "اسلایدر با موفقیت فعال شد";
                return RedirectToAction("SliderList", "Home", new { area = "Administration" });
            }

            TempData[ErrorMessage] = "خطا در فعال سازی اسلایدر";
            return RedirectToAction("SliderList", "Home", new { area = "Administration" });


        }
        #endregion

        #endregion

        #region Banners

        [HttpGet("banners-list")]
        public async Task<IActionResult> BannerList()
        {
            var banner = await _siteImagesService.GetAllBanners();

            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        [HttpGet("create-banner")]
        public async Task<IActionResult> CreateBanner()
        {
            return View();
        }


        [HttpPost("create-banner"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBanner(CreateBannerDto banner, IFormFile bannerImage)
        {
            var result = await _siteImagesService.CreateBanner(banner, bannerImage);

            switch (result)
            {
                case CreateBannerResult.Error:
                    TempData[ErrorMessage] = "در عملیات افزودن بنر خطایی رخ داد";
                    break;
                case CreateBannerResult.Success:
                    TempData[SuccessMessage] = "بنر با موفقیت ایجاد گردید";
                    return RedirectToAction("BannerList", "Home");
            }
            return View();
        }

        [HttpGet("edit-banner/{bannerId}")]
        public async Task<IActionResult> EditBanner(long bannerId)
        {
            var banner = await _siteImagesService.GetBannerForEdit(bannerId);
            return View(banner);
        }

        [HttpPost("edit-banner/{bannerId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBanner(EditBannerDto edit, IFormFile bannerImage)
        {
            var user = await _userService.GetUserById(User.GetUserId());
            var username = user.FirstName + " " + user.LastName;
            var result = await _siteImagesService.EditBanner(edit, bannerImage);

            switch (result)
            {
                case EditBannerResult.Error:
                    TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشد";
                    break;
                case EditBannerResult.Success:
                    TempData[SuccessMessage] = "ویرایش بنر با موفقیت انجام شد";
                    return RedirectToAction("BannerList", "Home");

            }

            return View();

        }


        [HttpGet("active-banner/{bannerId}")]
        public async Task<IActionResult> ActiveBanner(long bannerId)
        {
       
            var banner = await _siteImagesService.ActiveBanner(bannerId);
            return RedirectToAction("BannerList", "Home");
        }

        [HttpGet("deactive-banner/{bannerId}")]
        public async Task<IActionResult> DeactiveBanner(long bannerId)
        {

            var banner = await _siteImagesService.DeActiveBanner(bannerId);
            return RedirectToAction("BannerList", "Home");
        }

        #endregion

    }


}
