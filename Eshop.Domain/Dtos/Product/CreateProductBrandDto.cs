using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Dtos.Product
{
    public class CreateProductBrandDto
    {

        #region Properties

        [Display(Name = "نام برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string BrandName { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Display(Name = "تصویر برند")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? BrandImage { get; set; }

        #endregion

    }

    public enum CreateBrandResult
    {
        Error,
        Success,
        NotFound
    }
}
