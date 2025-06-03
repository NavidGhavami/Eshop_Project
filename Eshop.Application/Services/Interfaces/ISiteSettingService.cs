using Eshop.Domain.Dtos.Site;

namespace Eshop.Application.Services.Interfaces;

public interface ISiteSettingService : IAsyncDisposable
{
    #region Site Setting

    Task<SiteSettingDto> GetDefaultSiteSetting();
    Task<List<AboutUsDto>> GetAboutUs();

    #endregion
}