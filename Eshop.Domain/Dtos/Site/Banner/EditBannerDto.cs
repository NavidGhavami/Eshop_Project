namespace Eshop.Domain.Dtos.Site.Banner
{
    public class EditBannerDto : CreateBannerDto
    {
        #region Properties
        public long Id { get; set; }

        #endregion

    }

    public enum EditBannerResult
    {
        Success,
        Error
    }
}
