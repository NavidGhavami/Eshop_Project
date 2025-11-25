

using Eshop.Domain.Dtos.ProductDiscount;

namespace Eshop.Application.Services.Interfaces
{
    public interface IProductDiscountService : IAsyncDisposable
    {
        Task<FilterProductDiscountDto> FilterProductDiscount(FilterProductDiscountDto filter);
        Task<CreateDiscountResult> CreateDiscount(CreateDiscountDto discount, long productId);
        Task<EditDiscountDto> GetDiscountForEdit(long discountId);
        Task<EditDiscountResult> EditDiscount(EditDiscountDto edit);

    }
}
