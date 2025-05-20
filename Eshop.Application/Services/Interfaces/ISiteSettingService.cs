using Eshop.Domain.Dtos.Site;
using Eshop.Domain.Entites.Site;

namespace Eshop.Application.Services.Interfaces;

public interface ISiteSettingService : IAsyncDisposable
{
    #region Site Setting

    Task<SiteSettingDto> GetDefaultSiteSetting();

    #endregion
}