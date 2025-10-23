﻿using Eshop.Domain.Dtos.Product;
using Eshop.Domain.Dtos.ProductCategory;
using Eshop.Domain.Entities.Product;
using Microsoft.AspNetCore.Http;

namespace Eshop.Application.Services.Interfaces;

public interface IProductService : IAsyncDisposable
{
    #region Product

    Task<FilterProductDto> FilterProducts(FilterProductDto filter);
    Task<CreateProductResult> CreateProduct(CreateProductDto product, IFormFile productImage);
    Task<EditProductDto> GetProductForEdit(long productId);
    Task<EditProductResult> EditProductInAdmin(EditProductDto product, IFormFile productImage);
    Task<List<Product>> GetProductWithMaximumView(int take);
    Task<List<Product>> GetLatestArrivalProducts(int take);

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

    #region Product Color

    Task<List<FilterProductColorDto>> GetAllProductColorInAdminPanel(long productId);
    Task<CreateProductColorResult> CreateProductColor(CreateProductColorDto color, long productId);
    Task<EditProductColorDto> GetProductColorForEdit(long colorId);
    Task<EditProductColorResult> EditProductColor(EditProductColorDto color, long colorId);

    #endregion

    #region Product Feature

    Task<List<FilterProductFeatureDto>> GetAllProductFeatureInAdminPanel(long productId);
    Task<CreateProductFeatureResult> CreateProductFeature(CreateProductFeatureDto feature, long productId);

    #endregion

    #region Product Gallery

    Task<List<ProductGallery>> GetAllProductGalleries(long productId);
    Task<CreateOrEditProductGalleryResult> CreateProductGallery(CreateOrEditProductGalleryDto gallery, long productId, IFormFile galleryImage);
    Task<CreateOrEditProductGalleryDto> GetProductGalleryForEdit(long galleryId);
    Task<CreateOrEditProductGalleryResult> EditProductGallery(CreateOrEditProductGalleryDto gallery, long galleryId, IFormFile galleryImage);

    #endregion
}