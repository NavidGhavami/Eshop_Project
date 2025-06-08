using Eshop.Domain.Dtos.Contact;

namespace Eshop.Application.Services.Interfaces;

public interface IContactService : IAsyncDisposable
{
    Task CreateContactUs(CreateContactUsDto contact, string userIp, long? userId);
}