using Eshop.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Entities.Site
{
    public class Slider : BaseEntity
    {
        #region Properties

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "لینک")]
        public string Link { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام تصویر")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = " نام تصویر موبایل")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? MobileImageName { get; set; }

        [Display(Name = "فعال / غیرفعال")]
        public bool IsActive { get; set; }

        #endregion
    }
}
