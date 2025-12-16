namespace Eshop.Domain.Dtos.Product
{
    public class EditProductBrandDto : CreateProductBrandDto
    {
        public long Id { get; set; }
    }

    public enum EditBrandResult
    {
        Error,
        Success,
        NotFound
    }
}
