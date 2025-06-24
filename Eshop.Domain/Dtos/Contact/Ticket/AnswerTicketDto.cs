using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Dtos.Contact.Ticket
{
    public class AnswerTicketDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]        
        [Display(Name = "پاسخ تیکت")]
        public string Text { get; set; }
    }

    public enum AnswerTicketResult
    {
        NotForUser,
        NotFound,
        Success
    }
}
