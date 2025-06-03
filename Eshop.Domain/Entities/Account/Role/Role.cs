using System.ComponentModel.DataAnnotations;
using Eshop.Domain.Entities.Common;

namespace Eshop.Domain.Entities.Account.Role;

public class Role : BaseEntity
{
    #region Properties

    [Display(Name = "نام نقش")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string RoleName { get; set; }

    #endregion

    #region Relations

    public ICollection<User.User> Users { get; set; }


    #endregion
}