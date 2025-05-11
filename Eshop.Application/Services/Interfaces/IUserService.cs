using Eshop.Domain.Dtos.Account.User;
using Eshop.Domain.Entites.Account.User;

namespace Eshop.Application.Services.Interfaces;

public interface IUserService : IAsyncDisposable
{
    Task<RegisterUserResult> RegisterUser(RegisterUserDto register);
    Task<bool> IsUserExistByMobileNumber(string mobile);
    Task<UserLoginResult> LoginUser(LoginUserDto login);
    Task<User> GetUserByMobile(string mobile);
}