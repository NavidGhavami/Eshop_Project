using Eshop.Domain.Dtos.Site.Slider;
using Eshop.Domain.Entities.Site;
using Microsoft.AspNetCore.Http;

namespace Eshop.Application.Services.Interfaces;

public interface ISiteImagesService : IAsyncDisposable
{
    #region Slider

    Task<List<Slider>> GetAllActiveSlider();
    Task<List<Slider>> GetAllSlider();
    Task<CreateSliderResult> CreateSlider(CreateSliderDto slider, IFormFile sliderImage, IFormFile mobileSliderImage);
    Task<EditSliderDto> GetSliderForEdit(long sliderId);
    Task<EditSliderResult> EditSlider(EditSliderDto edit, IFormFile sliderImage, IFormFile mobileSliderImage);
    Task<bool> ActiveSlider(long sliderId);
    Task<bool> DeActiveSlider(long sliderId);

    #endregion
}