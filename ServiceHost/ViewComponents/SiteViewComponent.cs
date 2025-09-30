using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Entities.Site;
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
        private readonly IProductService _productService;

        public MegaMenuViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var category = await _productService.GetAllActiveProductCategories();
            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategories();
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

    #region Home Banner 1

    public class SiteBannerHome1ViewComponent : ViewComponent
    {
        private readonly ISiteImagesService _siteImagesService;

        public SiteBannerHome1ViewComponent(ISiteImagesService siteImagesService)
        {
            _siteImagesService = siteImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.banners = await _siteImagesService.GetSiteBannersByLocations(new List<BannersLocations>
            {
                BannersLocations.Home1,
                BannersLocations.Home2,
                BannersLocations.Home3,
                BannersLocations.Home4


            });
            return View("SiteBannerHome1");
        }
    }

    #endregion

    #region Home Banner 2

    public class SiteBannerHome2ViewComponent : ViewComponent
    {
        private readonly ISiteImagesService _siteImagesService;

        public SiteBannerHome2ViewComponent(ISiteImagesService siteImagesService)
        {
            _siteImagesService = siteImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.banners = await _siteImagesService.GetSiteBannersByLocations(new List<BannersLocations>
            {
                BannersLocations.Home1,
                BannersLocations.Home2,
                BannersLocations.Home3,
                BannersLocations.Home4


            });
            return View("SiteBannerHome2");
        }
    }

    #endregion

    #region Home Banner 3

    public class SiteBannerHome3ViewComponent : ViewComponent
    {
        private readonly ISiteImagesService _siteImagesService;

        public SiteBannerHome3ViewComponent(ISiteImagesService siteImagesService)
        {
            _siteImagesService = siteImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.banners = await _siteImagesService.GetSiteBannersByLocations(new List<BannersLocations>
            {
                BannersLocations.Home1,
                BannersLocations.Home2,
                BannersLocations.Home3,
                BannersLocations.Home4


            });
            return View("SiteBannerHome3");
        }
    }

    #endregion

    #region Latest Arrivals

    public class LatestArrivalProductViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public LatestArrivalProductViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var latestArrival = await _productService.GetLatestArrivalProducts(15);
            return View("LatestArrivalProduct", latestArrival);
        }
    }

    #endregion


}
