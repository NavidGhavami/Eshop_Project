using System.ComponentModel.DataAnnotations;
using Eshop.Domain.Dtos.Paging;
using Eshop.Domain.Entities.Product;

namespace Eshop.Domain.Dtos.Product
{
    public class FilterProductBrandDto : BasePaging
    {
        #region Constructor


        #endregion

        #region Properties

        public long ProductId { get; set; }

        [Display(Name = "نام برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string BrandName { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ProductName { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Display(Name = "تصویر برند")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? BrandImage { get; set; }

        public List<ProductBrand> ProductBrands { get; set; }
        public FilterProductBrandOrder ProductBrandOrder { get; set; }
        public FilterProductBrandState ProductBrandState { get; set; }

        #endregion

        #region Methods

        public FilterProductBrandDto SetProductBrand(List<ProductBrand> productBrands)
        {
            this.ProductBrands = productBrands;
            return this;
        }

        public FilterProductBrandDto SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;

            return this;
        }

        

        #endregion
    }

    public enum FilterProductBrandState
    {
        [Display(Name = "همه")]
        All,

    }

    public enum FilterProductBrandOrder
    {
        CreateDateDescending,
        CreateDateAscending,
    }

}
