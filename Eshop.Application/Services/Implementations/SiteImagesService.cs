using Eshop.Application.Extensions;
using Eshop.Application.Services.Interfaces;
using Eshop.Application.Utilities;
using Eshop.Domain.Dtos.Site.Slider;
using Eshop.Domain.Entities.Site;
using Eshop.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;

namespace Eshop.Application.Services.Implementations
{
    public class SiteImagesService : ISiteImagesService
    {
        #region Constructor

        private readonly IGenericRepository<Slider> _sliderRepository;

        public SiteImagesService(IGenericRepository<Slider> sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }

        #endregion

        #region Slider

        public async Task<List<Slider>> GetAllActiveSlider()
        {
            return await _sliderRepository.GetQuery()
                .Where(s => s.IsActive && !s.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .ToListAsync();
        }
        public async Task<List<Slider>> GetAllSlider()
        {
            return await _sliderRepository.GetQuery()
                .Where(s => !s.IsDelete)
                .OrderByDescending(s => s.Id)
                .ToListAsync();
        }
        public async Task<CreateSliderResult> CreateSlider(CreateSliderDto slider, IFormFile sliderImage, IFormFile mobileSliderImage)
        {
            if (sliderImage.IsImage() && mobileSliderImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(sliderImage.FileName);
                sliderImage.AddImageToServer(imageName, PathExtension.SliderOriginServer,
                    100, 100, PathExtension.SliderThumbServer);

                var mobileImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(mobileSliderImage.FileName);
                sliderImage.AddImageToServer(mobileImageName, PathExtension.MobileSliderOriginServer,
                    100, 100, PathExtension.MobileSliderThumbServer);


                var newSlider = new Slider
                {
                    Link = slider.Link,
                    Description = slider.Description,
                    ImageName = imageName,
                    MobileImageName = mobileImageName,
                    IsActive = true,
                    CreateDate = DateTime.Now
                };

                await _sliderRepository.AddEntity(newSlider);
                await _sliderRepository.SaveChanges();
            }

            return CreateSliderResult.Success;

        }
        public async Task<EditSliderDto> GetSliderForEdit(long sliderId)
        {
            var slider = await _sliderRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == sliderId);

            if (slider == null)
            {
                return null;
            }

            return new EditSliderDto
            {
                Id = slider.Id,
                ImageName = slider.ImageName,
                MobileImageName = slider.MobileImageName,
                Link = slider.Link,
                Description = slider.Description,
                IsActive = slider.IsActive
            };
        }
        public async Task<EditSliderResult> EditSlider(EditSliderDto edit, IFormFile sliderImage, IFormFile mobileSliderImage)
        {

            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(sliderImage.FileName);
            sliderImage.AddImageToServer(imageName, PathExtension.SliderOriginServer,
                100, 100, PathExtension.SliderThumbServer);

            var mobileImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(mobileSliderImage.FileName);
            sliderImage.AddImageToServer(mobileImageName, PathExtension.MobileSliderOriginServer,
                100, 100, PathExtension.MobileSliderThumbServer);


            var mainSlider = await _sliderRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == edit.Id);

            if (mainSlider == null)
            {
                return EditSliderResult.NotFound;
            }

            mainSlider.Link = edit.Link;
            mainSlider.Description = edit.Description;
            mainSlider.ImageName = imageName;
            mainSlider.MobileImageName = mobileImageName;
            mainSlider.IsActive = edit.IsActive;
            mainSlider.LastUpdateDate = DateTime.Now;

            _sliderRepository.EditEntity(mainSlider);
            await _sliderRepository.SaveChanges();

            return EditSliderResult.Success;
        }
        public async Task<bool> ActiveSlider(long sliderId)
        {
            var slider = _sliderRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == sliderId);

            if (slider == null)
            {
                return false;
            }

            slider.Result.IsActive = true;
            slider.Result.LastUpdateDate = DateTime.Now;

            _sliderRepository.EditEntity(slider.Result);
            await _sliderRepository.SaveChanges();

            return true;
        }
        public async Task<bool> DeActiveSlider(long sliderId)
        {
            var slider = _sliderRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == sliderId);

            if (slider == null)
            {
                return false;
            }

            slider.Result.IsActive = false;
            slider.Result.LastUpdateDate = DateTime.Now;

            _sliderRepository.EditEntity(slider.Result);
            await _sliderRepository.SaveChanges();

            return true;
        }

        #endregion

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (_sliderRepository != null)
            {
                await _sliderRepository.DisposeAsync();
            }
        }

        #endregion
    }
}
