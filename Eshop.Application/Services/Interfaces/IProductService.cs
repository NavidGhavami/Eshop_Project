using Eshop.Domain.Dtos.Product;

namespace Eshop.Application.Services.Interfaces;

public interface IProductService : IAsyncDisposable
{
    #region Product

    Task<FilterProductDto> FilterProducts(FilterProductDto filter);

    #endregion
}