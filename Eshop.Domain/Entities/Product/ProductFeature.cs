using Eshop.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Entities.Product
{
    public class ProductFeature : BaseEntity
    {
        #region Properties

        public long ProductId { get; set; }

        [Display(Name = "عنوان ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FeatureTitle { get; set; }

        [Display(Name = "مقدار ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FeatureValue { get; set; }

        public Product Product { get; set; }

        #endregion
    }
}
