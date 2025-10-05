namespace Eshop.Domain.Dtos.Product
{
    public class EditProductColorDto : CreateProductColorDto
    {
        public long Id { get; set; }
    }

    public enum EditProductColorResult
    {
        Error,
        ColorNotFound,
        DuplicateColor,
        Success,
    }
}
