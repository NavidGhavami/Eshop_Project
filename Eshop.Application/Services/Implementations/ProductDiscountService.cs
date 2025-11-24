using Eshop.Application.Services.Interfaces;
using Eshop.Application.Utilities;
using Eshop.Domain.Dtos.Paging;
using Eshop.Domain.Dtos.ProductDiscount;
using Eshop.Domain.Entities.Product;
using Eshop.Domain.Entities.ProductDiscount;
using Eshop.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;


namespace Eshop.Application.Services.Implementations
{
    public class ProductDiscountService : IProductDiscountService
    {
        #region Constructor

        private readonly IGenericRepository<ProductDiscount> _productDiscountRepository;
        private readonly IGenericRepository<ProductDiscountUse> _productDiscountUseRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public ProductDiscountService(IGenericRepository<ProductDiscount> productDiscountRepository, 
            IGenericRepository<ProductDiscountUse> productDiscountUseRepository,
            IGenericRepository<Product> productRepository)
        {
            _productDiscountRepository = productDiscountRepository;
            _productDiscountUseRepository = productDiscountUseRepository;
            _productRepository = productRepository;
        }


        #endregion


        public async Task<FilterProductDiscountDto> FilterProductDiscount(FilterProductDiscountDto filter)
        {
            var query = _productDiscountRepository
                .GetQuery()
                .Include(x => x.Product)
                .AsQueryable();

            #region Filter

            if (filter.ProductId != null && filter.ProductId != 0)
            {
                query = query.Where(x => x.ProductId == filter.ProductId.Value);
            }

            if (!string.IsNullOrEmpty(filter.ProductTitle))
            {
                query = query.Where(x => EF.Functions.Like(x.Product.Title, $"%{filter.ProductTitle}%")).OrderByDescending(x => x.CreateDate);
            }

            #endregion

            #region Paging

            var productDiscountCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, productDiscountCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).OrderByDescending(x => x.Id).ToListAsync();

            #endregion


            return filter.SetPaging(pager).SetProductDiscount(allEntities);
        }

        public async Task<CreateDiscountResult> CreateDiscount(CreateDiscountDto discount)
        {
            var product = await _productRepository.GetEntityById(discount.ProductId);

            if (product == null) { 
            
                return CreateDiscountResult.ProductNotFound;
            }

            var newDiscount = new ProductDiscount
            {
                ProductId = product.Id,
                DiscountNumber = discount.DiscountNumber,
                ExpireDate = discount.ExpireDate.ToMiladiDateTime(),
                Percentage = discount.Percentage,
            };

            
            await _productDiscountRepository.AddEntity(newDiscount);
            await _productDiscountRepository.SaveChanges();

            return CreateDiscountResult.Success;
        }

        public Task<EditDiscountDto> GetDiscountForEdit(long discountId)
        {
            throw new NotImplementedException();
        }

        public Task<EditDiscountResult> EditDiscount(EditDiscountDto edit)
        {
            throw new NotImplementedException();
        }

        

        


        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (_productDiscountRepository != null)
            {
                await _productDiscountRepository.DisposeAsync();
            }

            if (_productDiscountUseRepository != null)
            {
                await _productDiscountUseRepository.DisposeAsync();
            }
        }

        #endregion
    }
}
