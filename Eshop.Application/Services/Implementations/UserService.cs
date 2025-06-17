using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Account.User;
using Eshop.Domain.Repository;
using MarketPlace.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Eshop.Application.Extensions;
using Eshop.Application.Utilities;
using Eshop.Domain.Entities.Account.Role;
using Eshop.Domain.Entities.Account.User;
using Microsoft.AspNetCore.Http;

namespace Eshop.Application.Services.Implementations
{
    public class UserService : IUserService
    {

        #region Fields

        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ISmsService _smsService;


        #endregion

        #region Constructor

        public UserService(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository, IPasswordHasher passwordHasher, ISmsService smsService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _smsService = smsService;
        }

        #endregion

        #region Account
        public async Task<RegisterUserResult> RegisterUser(RegisterUserDto register)
        {
            try
            {
                if (!await IsUserExistByMobileNumber(register.Mobile))
                {
                    var user = new User
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        Mobile = register.Mobile,
                        Password = _passwordHasher.EncodePasswordMd5(register.Password),
                        MobileActiveCode = new Random().Next(100000,999999).ToString(),
                        EmailActiveCode = Guid.NewGuid().ToString("N"),
                        Avatar = null,
                        RoleId = 2,
                    };

                    await _userRepository.AddEntity(user);
                    await _userRepository.SaveChanges();

                    await _smsService.SendVerificationSms(register.Mobile, user.MobileActiveCode);

                    return RegisterUserResult.Success;
                }
                
                var userNotActive = await _userRepository.GetQuery()
                    .AsQueryable()
                    .SingleOrDefaultAsync(x => x.Mobile == register.Mobile);

                var newActiveCode = new Random().Next(100000, 999999).ToString();

                if (!userNotActive.IsMobileActive && userNotActive.MobileActiveCode != null)
                {
                    await _smsService.SendVerificationSms(userNotActive.Mobile, newActiveCode);

                     userNotActive.MobileActiveCode = newActiveCode;
                    _userRepository.EditEntity(userNotActive);

                     await _userRepository.SaveChanges();

                     return RegisterUserResult.MobileNotActive;
                }
                else
                {
                    return RegisterUserResult.MobileExists;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<bool> IsUserExistByMobileNumber(string mobile)
        {
            return await _userRepository
                .GetQuery()
                .AsQueryable()
                .AnyAsync(x => x.Mobile == mobile);
        }
        public async Task<UserLoginResult> LoginUser(LoginUserDto login)
        {
            var user = await _userRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Mobile == login.Mobile);

            if (user == null)
            {
                return UserLoginResult.UserNotFound;
            }

            if (!user.IsMobileActive)
            {
                return UserLoginResult.MobileNotActivated;
            }

            return user.Password != _passwordHasher.EncodePasswordMd5(login.Password)
                ? UserLoginResult.UserNotFound : UserLoginResult.Success;
        }
        public async Task<User> GetUserByMobile(string mobile)
        {
            return await _userRepository.GetQuery()
                .AsQueryable().SingleOrDefaultAsync(x => x.Mobile == mobile);
        }
        public async Task<User> GetUserById(long userId)
        {
            return await _userRepository.GetQuery()
                .AsQueryable().SingleOrDefaultAsync(x => x.Id == userId);
        }
        public async Task<bool> ActivateMobile(ActiveMobileDto activate)
        {
            var user = await _userRepository.GetQuery().AsQueryable()
                .SingleOrDefaultAsync(x => x.Mobile == activate.Mobile);

            if (user != null)
            {
                if (user.MobileActiveCode != activate.MobileActivationCode) return false;
                user.IsMobileActive = true;
                user.MobileActiveCode = new Random().Next(100000, 999999).ToString();
                await _userRepository.SaveChanges();

                return true;
            }

            return false;
        }
        public async Task<ForgotPasswordResult> RecoverUserPassword(ForgotPasswordDto forgot)
        {
            var user = await _userRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Mobile == forgot.Mobile);

            if (user == null)
            {
                return ForgotPasswordResult.UserNotFound;
            }

            var newPassword = PasswordGenerator.CreateRandomPassword();
            user.Password = _passwordHasher.EncodePasswordMd5(newPassword);
            _userRepository.EditEntity(user);

            await _smsService.SendRecoverPasswordSms(forgot.Mobile, newPassword);

            await _userRepository.SaveChanges();

            return ForgotPasswordResult.Success;
        }
        public async Task<string?> GetUserImage(long userId)
        {
            var user = await _userRepository.GetQuery().AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == userId);

            return user?.Avatar;
        }
        public async Task<EditUserProfileDto> GetProfileForEdit(long userId)
        {
            var user = await _userRepository.GetEntityById(userId);

            if (user == null)
            {
                return new EditUserProfileDto();
            }

            return new EditUserProfileDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Avatar = user.Avatar
            };
        }
        public async Task<EditUserProfileResult> EditUserProfile(EditUserProfileDto profile, long userId, IFormFile avatarImage)
        {
            var user = await _userRepository.GetEntityById(userId);

            if (user == null)
            {
                return EditUserProfileResult.NotFound;
            }

            if (user.IsBlocked)
            {
                return EditUserProfileResult.IsBlocked;
            }

            if (!user.IsMobileActive)
            {
                return EditUserProfileResult.IsNotActive;
            }

            user.Edit(profile.FirstName, profile.LastName, profile.Email);

            if (avatarImage != null && avatarImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(avatarImage.FileName);

                avatarImage.AddImageToServer(imageName, PathExtension.UserAvatarOriginServer,
                    100, 100, PathExtension.UserAvatarThumbServer, user.Avatar);

                user.Avatar = imageName;
            }

            _userRepository.EditEntity(user);
            await _userRepository.SaveChanges();

            return EditUserProfileResult.Success;
        }

        #endregion



        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (_userRepository != null)
            {
                await _userRepository.DisposeAsync();
            }

            if (_roleRepository != null)
            {
                await _roleRepository.DisposeAsync();
            }
        }

        #endregion



    }
}
