using Eshop.Domain.Dtos.Account.User;

namespace Eshop.Application.Services.Interfaces;

public interface IUserService : IAsyncDisposable
{
    Task<RegisterUserResult> RegisterUser(RegisterUserDto register);
    Task<bool> IsUserExistByMobileNumber(string mobile);
}