﻿using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Dtos.Product
{
    public class CreateProductFeatureDto
    {

        public long ProductId { get; set; }

        [Display(Name = "عنوان ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FeatureTitle { get; set; }

        [Display(Name = "مقدار ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FeatureValue { get; set; }

        public List<CreateProductFeatureDto> ProductFeatures { get; set; }
    }

    public enum CreateProductFeatureResult
    {
        Error,
        Success,
        ProductNotFound,
        NotForUserProduct,
        DuplicateFeature
    }
}
