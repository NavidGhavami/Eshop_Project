using Eshop.Domain.Dtos.Account.User;
using Eshop.Domain.Entities.Account.User;
using Microsoft.AspNetCore.Http;

namespace Eshop.Application.Services.Interfaces;

public interface IUserService : IAsyncDisposable
{
    Task<RegisterUserResult> RegisterUser(RegisterUserDto register);
    Task<bool> IsUserExistByMobileNumber(string mobile);
    Task<UserLoginResult> LoginUser(LoginUserDto login);
    Task<User> GetUserByMobile(string mobile);
    Task<User> GetUserById(long userId);
    Task<bool> ActivateMobile(ActiveMobileDto activate);
    Task<ForgotPasswordResult> RecoverUserPassword(ForgotPasswordDto forgot);
    Task<string?> GetUserImage(long userId);
    Task<EditUserProfileDto> GetProfileForEdit(long userId);
    Task<EditUserProfileResult> EditUserProfile(EditUserProfileDto profile, long userId, IFormFile avatarImage);
    Task<bool> ChangeUserPassword(ChangePasswordDto changePassword, long currentUserId);
}