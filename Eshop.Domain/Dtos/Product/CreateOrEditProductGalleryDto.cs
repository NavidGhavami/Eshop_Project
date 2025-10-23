using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Dtos.Product
{
    public class CreateOrEditProductGalleryDto
    {
        [Display(Name = "الویت نمایش")]
        public int? DisplayPriority { get; set; }

        [Display(Name = "تصویر گالری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string? Image { get; set; }

    }

    public enum CreateOrEditProductGalleryResult
    {
        Success,
        Error,
        NotForUserProduct,
        ImageIsNull,
        ProductNotFound
    }
}
