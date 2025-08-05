using Eshop.Domain.Dtos.Account.User;
using Eshop.Domain.Entities.Account.Role;
using Eshop.Domain.Entities.Account.User;
using Microsoft.AspNetCore.Http;

namespace Eshop.Application.Services.Interfaces;

public interface IUserService : IAsyncDisposable
{
    #region User

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
    Task<FilterUserDto> FilterUser(FilterUserDto filter);
    Task<EditUserDto> GetUserForEdit(long userId);
    Task<EditUserResult> EditUser(EditUserDto edit, string username);

    #endregion



    #region Role

    Task<FilterRoleDto> FilterRole(FilterRoleDto filter);
    Task<CreateRoleResult> CreateRole(CreateRoleDto role);
    Task<EditRoleDto> GetRoleForEdit(long roleId);
    Task<EditRoleResult> EditRole(EditRoleDto edit, string username);
    Task<List<Role>> GetRoles();


    #endregion
}