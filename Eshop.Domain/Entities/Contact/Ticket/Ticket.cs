using System.ComponentModel.DataAnnotations;
using Eshop.Domain.Entities.Account.User;
using Eshop.Domain.Entities.Common;

namespace Eshop.Domain.Entities.Contact.Ticket
{
    public class Ticket : BaseEntity
    {
        #region Properties

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(350, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        public long OwnerId { get; set; }

        [Display(Name = "بخش مورد نظر")]
        public TicketSection TicketSection { get; set; }

        [Display(Name = "اولویت")]
        public TicketPriority TicketPriority { get; set; }

        [Display(Name = "وضعیت تیکت")]
        public TicketState TicketState { get; set; }

        public bool IsReadByOwner { get; set; }
        public bool IsReadByAdmin { get; set; }

        #endregion

        #region Relation

        public User Owner { get; set; }
        public ICollection<TicketMessage> TicketMessages { get; set; }

        #endregion
    }

    public enum TicketSection
    {
        [Display(Name = "پشتیبانی")]
        Support,

        [Display(Name = "فنی")]
        Technical,

        [Display(Name = "آموزشی")]
        Academic,

    }

    public enum TicketPriority
    {
        [Display(Name = "کم")]
        Low,

        [Display(Name = "متوسط")]
        Medium,

        [Display(Name = "زیاد")]
        High
    }

    public enum TicketState
    {
        [Display(Name = "همه")]
        All,

        [Display(Name = "درحال بررسی")]
        UnderProgress,

        [Display(Name = "پاسخ داده شده")]
        Answered,

        [Display(Name = "بسته شده")]
        Closed

    }
}
