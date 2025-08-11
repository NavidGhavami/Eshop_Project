using Eshop.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{

    #region Site Header

    public class SiteHeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteHeader");
        }
    }

    #endregion

    #region Site Footer
    public class SiteFooterViewComponent : ViewComponent
    {
        private readonly ISiteSettingService _siteSettingService;

        public SiteFooterViewComponent(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var siteSetting = await _siteSettingService.GetDefaultSiteSetting();
            return View("SiteFooter", siteSetting);
        }
    }
    #endregion

    #region Mega Menu

    public class MegaMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("MegaMenu");
        }
    }

    #endregion

    #region Slider

    public class HomeSliderViewComponent : ViewComponent
    {
        private readonly ISiteImagesService _siteImagesService;

        public HomeSliderViewComponent(ISiteImagesService siteImagesService)
        {
            _siteImagesService = siteImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders = await _siteImagesService.GetAllActiveSlider();
            return View("HomeSlider", sliders);
        }
    }

    #endregion

}
