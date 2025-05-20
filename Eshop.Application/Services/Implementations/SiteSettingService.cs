using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Site;
using Eshop.Domain.Entites.Site;
using Eshop.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Application.Services.Implementations
{
    public class SiteSettingService : ISiteSettingService
    {
        #region Fields

        private readonly IGenericRepository<SiteSetting> _siteSettingRepository;



        #endregion


        #region Constructor

        public SiteSettingService(IGenericRepository<SiteSetting> siteSettingRepository)
        {
            _siteSettingRepository = siteSettingRepository;
        }


        #endregion


        #region Site Setting

        public async Task<SiteSettingDto> GetDefaultSiteSetting()
        {


           var siteSetting = await _siteSettingRepository.GetQuery().AsQueryable()
                .Select(x=> new SiteSettingDto
                {
                    SiteName = x.SiteName,
                    Email = x.Email,
                    Address = x.Address,
                    CopyRight = x.CopyRight,
                    FooterText = x.FooterText,
                    IsDefault = x.IsDefault,
                    MapScript = x.MapScript,
                    Mobile = x.Mobile,
                    Phone = x.Phone
                })
                .FirstOrDefaultAsync(x => x.IsDefault);

           return siteSetting ?? new SiteSettingDto();
        }

        #endregion








        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (_siteSettingRepository != null)
            {
                await _siteSettingRepository.DisposeAsync();
            }
        }

       

        #endregion
    }
}
