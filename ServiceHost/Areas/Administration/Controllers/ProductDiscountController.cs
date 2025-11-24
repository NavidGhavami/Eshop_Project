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
        public async Task<IActionResult> FilterDiscounts(FilterProductDiscountDto filter)
        {
            var productDiscount = await _productDiscountService.FilterProductDiscount(filter);

            return View(filter);
        }

        #endregion

        #region Create Discount

        [HttpGet("create-discount")]
        public async Task<IActionResult> CreateDiscount()
        {
            return View();
        }

        [HttpPost("create-discount"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDiscount(CreateDiscountDto discount)
        {
            if (ModelState.IsValid)
            {
                var result = await _productDiscountService.CreateDiscount(discount);

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
                        return RedirectToAction("FilterDiscounts");
                }
            }
            return View(discount);
        }

        #endregion
    }
}
