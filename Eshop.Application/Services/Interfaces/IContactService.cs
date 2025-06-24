using Eshop.Domain.Dtos.Contact;
using Eshop.Domain.Dtos.Contact.Ticket;

namespace Eshop.Application.Services.Interfaces;

public interface IContactService : IAsyncDisposable
{ 
    Task CreateContactUs(CreateContactUsDto contact, string userIp, long? userId);


    #region Ticket

    Task<List<FilterTicketDto>> TicketList();
    Task<AddTicketResult> AddUserTicket(AddTicketDto ticket, long userId);
    Task<TicketDetailDto> GetTicketDetail(long ticketId, long userId);

    #endregion
}