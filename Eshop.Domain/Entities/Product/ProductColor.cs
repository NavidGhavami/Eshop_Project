using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Domain.Entities.Common;

namespace Eshop.Domain.Entities.Product
{
    public class ProductColor : BaseEntity
    {
        #region Properties

        public long ProductId { get; set; }

        [Display(Name = "رنگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ColorName { get; set; }

        [Display(Name = "کد رنگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ColorCode { get; set; }

        [Display(Name = "قیمت رنگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Price { get; set; }

        #endregion

        #region Relations

        public Product Product { get; set; }

        #endregion
    }
}
