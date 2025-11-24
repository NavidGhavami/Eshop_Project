namespace Eshop.Domain.Dtos.ProductDiscount
{
    public class EditDiscountDto : CreateDiscountDto
    {
        public long Id { get; set; }
        public string ProductTitle { get; set; }
    }

    public enum EditDiscountResult
    {
        Success,
        ProductIsNotForSeller,
        ProductNotFound,
        Error
    }
}