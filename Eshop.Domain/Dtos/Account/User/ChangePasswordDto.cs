using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Dtos.Account.User
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "رمز عبور فعلی")]
        [DataType(DataType.Password)]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string CurrentPassword { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Compare("NewPassword", ErrorMessage = "کلمه های عبور مغایرت دارند")]
        public string ConfirmNewPassword { get; set; }
    }
}
