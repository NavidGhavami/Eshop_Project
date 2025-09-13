using Eshop.Domain.Dtos.Product;
using Eshop.Domain.Dtos.ProductCategory;
using Eshop.Domain.Entities.Product;
using Microsoft.AspNetCore.Http;

namespace Eshop.Application.Services.Interfaces;

public interface IProductService : IAsyncDisposable
{
    #region Product

    Task<FilterProductDto> FilterProducts(FilterProductDto filter);
    Task<CreateProductResult> CreateProduct(CreateProductDto product, IFormFile productImage);

    #endregion


    #region Product Category

    Task<FilterProductCategoryDto> FilterProductCategory(FilterProductCategoryDto filter);
    Task<FilterProductCategoryDto> FilterProductSubCategory(FilterProductCategoryDto filter, long? parentId);
    Task<List<ProductCategory>> GetAllProductCategoriesBy(long? parentId);
    Task<List<ProductCategory>> GetAllActiveProductCategories();
    Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryDto category,
        IFormFile image);
    Task<EditProductCategoryDto> GetProductCategoryForEdit(long categoryId);
    Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryDto edit, IFormFile image);

    #endregion
}