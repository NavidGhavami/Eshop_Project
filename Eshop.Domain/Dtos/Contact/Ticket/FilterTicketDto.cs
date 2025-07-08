using Eshop.Domain.Dtos.Paging;
using Eshop.Domain.Entities.Contact.Ticket;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Domain.Dtos.Contact.Ticket
{
    public class FilterTicketDto : BasePaging
    {
        #region Properties

        public string Title { get; set; }
        public long? UserId { get; set; }
        public TicketSection? TicketSection { get; set; }
        public TicketPriority? TicketPriority { get; set; }
        public TicketState? TicketState { get; set; }
        public FilterTicketState FilterTicketState { get; set; }
        public FilterTicketOrder OrderBy { get; set; }
        public List<Entities.Contact.Ticket.Ticket> Tickets { get; set; }


        #endregion

        #region Methods

        public FilterTicketDto SetTickets(List<Entities.Contact.Ticket.Ticket> tickets)
        {
            Tickets = tickets;
            return this;
        }

        public FilterTicketDto SetPaging(BasePaging paging)
        {
            PageId = paging.PageId;
            AllEntitiesCount = paging.AllEntitiesCount;
            StartPage = paging.StartPage;
            EndPage = paging.EndPage;
            HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            TakeEntity = paging.TakeEntity;
            SkipEntity = paging.SkipEntity;
            PageCount = paging.PageCount;

            return this;
        }

        #endregion
    }

    public enum FilterTicketState
    {
        [Display(Name = "همه")]
        All,

        [Display(Name = "درحال بررسی")]
        UnderProgress,


        [Display(Name = "بسته شده")]
        Closed,

        [Display(Name = "پاسخ داده شده")]
        Answered,


    }

    public enum FilterTicketOrder
    {
        CreateDateDescending,
        CreateDateAscending,
    }

}

