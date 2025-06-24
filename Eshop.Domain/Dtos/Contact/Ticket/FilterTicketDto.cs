using Eshop.Domain.Entities.Contact.Ticket;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Dtos.Contact.Ticket
{
    public class FilterTicketDto
    {
        public long Id { get; set; }
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

        public string CreateDate { get; set; }

    }
}
