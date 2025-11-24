using Eshop.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Entities.ProductDiscount
{
    public class ProductDiscount : BaseEntity
    {
        #region Properties

        public long ProductId { get; set; }

        [Range(0, 100)]
        public int Percentage { get; set; }

        public DateTime ExpireDate { get; set; }
        public int? DiscountNumber { get; set; }


        #endregion

        #region Relations

        public Product.Product Product { get; set; }
        public ICollection<ProductDiscountUse> ProductDiscountUse { get; set; }

        #endregion
    }
}
