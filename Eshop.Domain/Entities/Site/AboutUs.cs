﻿using System.ComponentModel.DataAnnotations;
using Eshop.Domain.Entities.Common;

namespace Eshop.Domain.Entities.Site
{
    public class AboutUs : BaseEntity
    {
        #region Properties

        [Display(Name = "عنوان هدر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(550, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]

        public string HeaderTitle { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        #endregion
    }
}
