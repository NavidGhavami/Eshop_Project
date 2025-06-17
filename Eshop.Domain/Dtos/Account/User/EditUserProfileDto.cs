using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Dtos.Account.User
{
    public class EditUserProfileDto
    {
        public long Id { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string LastName { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نمی باشد")]
        public string? Email { get; set; }

        [Display(Name = "نصویر آواتار")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Avatar { get; set; }
    }



    public enum EditUserProfileResult
    {
        Error,
        Success,
        NotFound,
        IsBlocked,
        IsNotActive
    }
}
