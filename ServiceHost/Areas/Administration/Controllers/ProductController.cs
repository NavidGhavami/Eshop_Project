using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.Controllers
{
    public class ProductController : AdminBaseController
    {
        #region Constructor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion


        #region Product

        #region Filter Product

        [HttpGet("product-list")]
        public async Task<IActionResult> FilterProduct(FilterProductDto filter)
        {
            var product = await _productService.FilterProducts(filter);
            return View(product);
        }

        #endregion

        #endregion
    }
}
