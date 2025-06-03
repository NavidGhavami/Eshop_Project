using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Site;
using Eshop.Domain.Entities.Site;
using Eshop.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Application.Services.Implementations
{
    public class SiteSettingService : ISiteSettingService
    {
        #region Fields

        private readonly IGenericRepository<SiteSetting> _siteSettingRepository;
        private readonly IGenericRepository<AboutUs> _aboutUsRepository;



        #endregion


        #region Constructor

        public SiteSettingService(IGenericRepository<SiteSetting> siteSettingRepository, IGenericRepository<AboutUs> aboutUsRepository)
        {
            _siteSettingRepository = siteSettingRepository;
            _aboutUsRepository = aboutUsRepository;
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

        public async Task<List<AboutUsDto>> GetAboutUs()
        {
            return await _aboutUsRepository.GetQuery().AsQueryable()
                .Select(x => new AboutUsDto
                {
                    HeaderTitle = x.HeaderTitle,
                    Description = x.Description

                }).ToListAsync();
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
