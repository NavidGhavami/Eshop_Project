using Eshop.Application.Services.Implementations;
using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.ProductDiscount;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.Controllers
{
    public class ProductDiscountController : AdminBaseController
    {
        private readonly IProductDiscountService _productDiscountService;

        public ProductDiscountController(IProductDiscountService productDiscountService)
        {
            _productDiscountService = productDiscountService;
        }

        #region Filter Product Discount

        [HttpGet("discounts")]
        [HttpGet("discounts/{productId}")]
        public async Task<IActionResult> FilterDiscounts(FilterProductDiscountDto filter, long productId)
        {
            var productDiscount = await _productDiscountService.FilterProductDiscount(filter);

            ViewBag.ProductId = productId;

            return View(filter);
        }

        #endregion

        #region Create Discount

        [HttpGet("create-discount/{productId}")]
        public async Task<IActionResult> CreateDiscount(long productId)
        {
            return View();
        }

        [HttpPost("create-discount/{productId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDiscount(CreateDiscountDto discount, long productId)
        {
            if (ModelState.IsValid)
            {
                var result = await _productDiscountService.CreateDiscount(discount, productId);

                switch (result)
                {
                    case CreateDiscountResult.Error:
                        TempData[ErrorMessage] = "عملیات ثبت تخفیف مورد نظر با شکست مواجه شد";
                        break;
                    case CreateDiscountResult.ProductNotFound:
                        TempData[WarningMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case CreateDiscountResult.ProductIsNotForSeller:
                        TempData[WarningMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case CreateDiscountResult.Success:
                        TempData[SuccessMessage] = "عملیات ثبت تخفیف برای محصول مورد نظر با موفقیت انجام شد";
                        return RedirectToAction("FilterDiscounts", new { area = "Administration", ProductId = productId });
                }
            }
            return View(discount);
        }

        #endregion
    }
}
