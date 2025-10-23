

using Eshop.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Entities.Product
{
    public class ProductGallery : BaseEntity
    {
        #region Properties

        public long ProductId{ get; set; }

        [Display(Name = "اولویت نمایش")]
        public int? DisplayPriority { get; set; }

        [Display(Name = "تصویر گالری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageName { get; set; }

        #endregion

        #region Relation

        public Product Product { get; set; }

        #endregion
    }
}
