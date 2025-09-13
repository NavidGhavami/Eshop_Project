using Eshop.Application.Extensions;
using Eshop.Application.Services.Interfaces;
using Eshop.Application.Utilities;
using Eshop.Domain.Dtos.Paging;
using Eshop.Domain.Dtos.Product;
using Eshop.Domain.Dtos.ProductCategory;
using Eshop.Domain.Entities.Product;
using Eshop.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Application.Services.Implementations
{
    public class ProductService : IProductService
    {

        #region Constructor

        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductCategory> _productCategoryRepository;

        public ProductService(IGenericRepository<Product> productRepository, IGenericRepository<ProductCategory> productCategoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        #endregion

        #region Product

        public async Task<FilterProductDto> FilterProducts(FilterProductDto filter)
        {
            var query = _productRepository
                .GetQuery()
                .AsQueryable();

            #region State

            switch (filter.ProductState)
            {
                case FilterProductState.All:
                    query = query.Where(x => !x.IsDelete);
                    break;
                case FilterProductState.Active:
                    query = query.Where(x => x.IsActive.Value);
                    break;
                case FilterProductState.NotActive:
                    query = query.Where(x => !x.IsActive.Value);
                    break;
            }

            switch (filter.OrderBy)
            {
                case FilterProductOrderBy.CreateDateDescending:
                    query = query.OrderByDescending(x => x.CreateDate);
                    break;
                case FilterProductOrderBy.CreateDateAscending:
                    query = query.OrderBy(x => x.CreateDate);
                    break;
                case FilterProductOrderBy.PriceDescending:
                    query = query.OrderByDescending(x => x.Price);
                    break;
                case FilterProductOrderBy.PriceAscending:
                    query = query.OrderBy(x => x.Price);
                    break;
                case FilterProductOrderBy.ViewDescending:
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case FilterProductOrderBy.CountDescending:
                    query = query.OrderByDescending(x => x.SellCount);
                    break;
                case FilterProductOrderBy.CountAscending:
                    query = query.OrderBy(x => x.SellCount);
                    break;
            }

            #region Filter

            if (!string.IsNullOrWhiteSpace(filter.ProductTitle))
            {
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.ProductTitle}%"));
            }

            #endregion

            #region Paging

            var productCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, productCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetProduct(allEntities);

            #endregion
        }

        #region Create

        public async Task<CreateProductResult> CreateProduct(CreateProductDto product, IFormFile productImage)
        {
            if (productImage == null)
            {
                return CreateProductResult.HasNoImage;
            }

            if (!productImage.IsImage())
            {
                return CreateProductResult.ImageErrorType;
            }

            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);
            productImage.AddImageToServer(imageName, PathExtension.ProductOriginServer,
                100, 100, PathExtension.ProductThumbServer);

            //create Product

            var newProduct = new Product
            {
                Title = product.Title,
                Code = product.Code,
                Price = product.Price,
                Image = imageName,
                IsActive = product.IsActive,
                Description = product.Description,
                ShortDescription = product.ShortDescription,
                ViewCount = 0,
                SellCount = 0

            };


            await _productRepository.AddEntity(newProduct);
            await _productRepository.SaveChanges();

            return CreateProductResult.Success;

        }



        #endregion



        #endregion

        #region Product Category

        public async Task<FilterProductCategoryDto> FilterProductCategory(FilterProductCategoryDto filter)
        {
            var query = _productCategoryRepository
                .GetQuery()
                .Include(x => x.ProductSelectedCategories)
                .Where(x => x.ParentId == null && !x.IsDelete)
                .AsQueryable();

            #region Filter

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.Title}%")).OrderByDescending(x => x.CreateDate);
            }

            #endregion

            #region Paging

            var productCategoryCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, productCategoryCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).ToListAsync();


            #endregion

            return filter.SetPaging(pager).SetProduct(allEntities);
        }
        public async Task<FilterProductCategoryDto> FilterProductSubCategory(FilterProductCategoryDto filter, long? parentId)
        {
            var query = _productCategoryRepository
                .GetQuery()
                .Include(x => x.ProductSelectedCategories)
                .Where(x => x.ParentId == parentId && !x.IsDelete)
                .AsQueryable();

            #region Filter

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.Title}%")).OrderByDescending(x => x.CreateDate);
            }

            #endregion

            #region Paging

            var productCategoryCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, productCategoryCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).ToListAsync();


            #endregion

            return filter.SetPaging(pager).SetProduct(allEntities);
        }
        public async Task<List<ProductCategory>> GetAllProductCategoriesBy(long? parentId)
        {
            if (parentId == null || parentId == 0)
            {
                return await _productCategoryRepository
                    .GetQuery()
                    .AsQueryable()
                    .Where(x => !x.IsDelete && x.IsActive && x.ParentId == null)
                    .ToListAsync();
            }

            return await _productCategoryRepository
                .GetQuery()
                .AsQueryable()
                .Where(x => !x.IsDelete && x.IsActive && x.ParentId == parentId)
                .ToListAsync();
        }
        public async Task<List<ProductCategory>> GetAllActiveProductCategories()
        {
            return await _productCategoryRepository
                .GetQuery()
                .AsQueryable()
                .Where(x => x.IsActive && !x.IsDelete && x.ParentId == null)
                .ToListAsync();
        }
        public async Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryDto category, IFormFile image)
        {
            if (string.IsNullOrWhiteSpace(category.Title) && string.IsNullOrWhiteSpace(category.UrlName))
            {
                return CreateProductCategoryResult.Error;
            }

            var newCategory = new ProductCategory
            {
                Title = category.Title,
                UrlName = category.UrlName.Replace(" ", "-"),
                Icon = category.Icon,
                ParentId = category.ParentId ?? null,
                IsActive = true
            };

            if (image.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                image.AddImageToServer(imageName, PathExtension.ProductCategoryOriginServer,
                    100, 100, PathExtension.ProductCategoryThumbServer);

                newCategory.Image = imageName;
            }
            else
            {
                return CreateProductCategoryResult.ImageErrorType;
            }

            await _productCategoryRepository.AddEntity(newCategory);
            await _productCategoryRepository.SaveChanges();

            return CreateProductCategoryResult.Success;
        }
        public async Task<EditProductCategoryDto> GetProductCategoryForEdit(long categoryId)
        {
            var category = await _productCategoryRepository
                .GetQuery()
                .FirstOrDefaultAsync(x => x.Id == categoryId);

            if (category == null)
            {
                return null;
            }

            return new EditProductCategoryDto
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Title = category.Title,
                Image = category.Image,
                IsActive = category.IsActive,
                Icon = category.Icon,
                UrlName = category.UrlName,
            };
        }
        public async Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryDto category, IFormFile image)
        {
            var mainCategory = await _productCategoryRepository
                .GetQuery()
                .FirstOrDefaultAsync(x => x.Id == category.Id);

            if (mainCategory == null)
            {
                return EditProductCategoryResult.NotFound;
            }

            if (image != null)
            {

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                image.AddImageToServer(imageName, PathExtension.ProductCategoryOriginServer, 100, 100,
                    PathExtension.ProductCategoryThumbServer, mainCategory.Image);

                mainCategory.Image = imageName;

            }

            mainCategory.Title = category.Title;
            mainCategory.UrlName = category.UrlName.Replace(" ", "-");
            mainCategory.Icon = category.Icon;
            mainCategory.ParentId = category.ParentId;

            _productCategoryRepository.EditEntity(mainCategory);
            await _productCategoryRepository.SaveChanges();

            return EditProductCategoryResult.Success;
        }

        #endregion

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (_productRepository != null)
            {
                await _productRepository.DisposeAsync();
            }

            if (_productCategoryRepository != null)
            {
                await _productCategoryRepository.DisposeAsync();
            }
        }


        #endregion

    }
}
