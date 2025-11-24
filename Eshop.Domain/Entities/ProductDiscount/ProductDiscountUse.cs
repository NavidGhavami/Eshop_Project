

using Eshop.Domain.Entities.Common;

namespace Eshop.Domain.Entities.ProductDiscount
{
    public class ProductDiscountUse : BaseEntity
    {
        #region Properties

        public long DiscountId { get; set; }

        #endregion

        #region Relations

        public ProductDiscount ProductDiscount { get; set; }

        #endregion
    }
}
