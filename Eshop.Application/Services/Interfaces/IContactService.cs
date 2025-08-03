using Eshop.Domain.Dtos.Contact;
using Eshop.Domain.Dtos.Contact.Ticket;
using Eshop.Domain.Dtos.Site;
using Eshop.Domain.Entities.Site;

namespace Eshop.Application.Services.Interfaces;

public interface IContactService : IAsyncDisposable
{ 
    Task CreateContactUs(CreateContactUsDto contact, string userIp, long? userId);
    Task<FilterContactUs> FilterContactUs(FilterContactUs filter);
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////
    Task<List<AboutUsDto>> GetAll();
    Task<CreateAboutUsResult> CreateAboutUs(CreateAboutUsDto about);
    Task<EditAboutUsDto> GetAboutUsForEdit(long id);
    Task<EditAboutUsResult> EditAboutUs(EditAboutUsDto edit, string username);

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