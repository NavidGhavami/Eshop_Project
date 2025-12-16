using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Domain.Entities.Common;
using Eshop.Domain.Entities.ProductDiscount;


namespace Eshop.Domain.Entities.Product
{
    public class Product : BaseEntity
    {
        #region Properties

        public long? ProductBrandId { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "کد محصول")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Code { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        public string? ShortDescription { get; set; }

        [Display(Name = "توضیحات اصلی")]
        public string? Description { get; set; }

        [Display(Name = "فعال / غیر فعال")]
        public bool? IsActive { get; set; }

        [Display(Name = "تصویر محصول")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Image { get; set; }

        [Display(Name = "تعداد بازدید")]
        public int? ViewCount { get; set; }

        [Display(Name = "تعداد فروش")]
        public int? SellCount { get; set; }


        #endregion


        #region Relations

        public ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; }
        public ICollection<ProductColor> ProductColors { get; set; }
        public ICollection<ProductFeature> ProductFeatures { get; set; }
        public ICollection<ProductGallery> ProductGalleries { get; set; }
        public ICollection<ProductDiscount.ProductDiscount> ProductDiscounts { get; set; }
        public ProductBrand ProductBrand { get; set; }

        #endregion
    }
}
