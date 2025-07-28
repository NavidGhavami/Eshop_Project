using Eshop.Domain.Dtos.Contact;
using Eshop.Domain.Dtos.Contact.Ticket;

namespace Eshop.Application.Services.Interfaces;

public interface IContactService : IAsyncDisposable
{ 
    Task CreateContactUs(CreateContactUsDto contact, string userIp, long? userId);


    #region Ticket

    Task<FilterTicketDto> TicketList(FilterTicketDto filter);
    Task<AddTicketResult> AddUserTicket(AddTicketDto ticket, long userId);
    Task<TicketDetailDto> GetTicketDetail(long ticketId, long userId);
    Task<string?> GetUserAvatarTicket(long ticketId);
    Task<string?> GetAdminUserAvatarTicket(long ticketId);
    Task<AnswerTicketResult> AnswerTicket(AnswerTicketDto answer, long userId);
    Task<AnswerTicketResult> AdminAnswerTicket(AnswerTicketDto answer, long userId);

    #endregion
}