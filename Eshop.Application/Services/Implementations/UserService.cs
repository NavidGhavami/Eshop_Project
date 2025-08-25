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
using Eshop.Domain.Dtos.Paging;

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

            if (user.IsBlocked)
            {
                return UserLoginResult.UserBlocked;
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
        public async Task<bool> ChangeUserPassword(ChangePasswordDto changePassword, long currentUserId)
        {
            var user = await _userRepository.GetEntityById(currentUserId);

            if (user != null)
            {
                var newPassword = _passwordHasher.EncodePasswordMd5(changePassword.NewPassword);

                if (newPassword != user.Password)
                {
                    user.Password = newPassword;
                    _userRepository.EditEntity(user);

                    await _userRepository.SaveChanges();
                    return true;
                }
            }
            return false;

        }
        public async Task<FilterUserDto> FilterUser(FilterUserDto filter)
        {
            var query = _userRepository
                .GetQuery()
                .Include(x => x.Role)
                .AsQueryable();

            if (filter.RoleId > 0)
            {
                query = query.Where(x => x.RoleId == filter.RoleId);
            }
            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                query = query.Where(x => EF.Functions.Like(x.FirstName, $"%{filter.FirstName}%"));
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                query = query.Where(x => EF.Functions.Like(x.LastName, $"%{filter.LastName}%"));
            }
            if (!string.IsNullOrEmpty(filter.Mobile))
            {
                query = query.Where(x => EF.Functions.Like(x.Mobile, $"%{filter.Mobile}%"));
            }
            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(x => EF.Functions.Like(x.Email, $"%{filter.Email}%"));
            }

            #region Paging


            var userCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, userCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).OrderByDescending(x => x.Id).ToListAsync();


            #endregion

            return filter.SetPaging(pager).SetUsers(allEntities);
        }
        public async Task<EditUserDto> GetUserForEdit(long userId)
        {
            var user = await _userRepository.GetQuery()
                .AsQueryable()
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return null;
            }

            return new EditUserDto
            {
                Id = user.Id,
                RoleId = user.Role.Id,
                Email = user.Email,
                Mobile = user.Mobile,
                IsBlocked = user.IsBlocked,
                IsMobileActivated = user.IsMobileActive,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }
        public async Task<EditUserResult> EditUser(EditUserDto edit, string username)
        {
            var mainUser = await _userRepository
                .GetQuery()
                .AsQueryable()
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id == edit.Id);

            if (mainUser == null)
            {
                return EditUserResult.UserNotFound;
            }

            mainUser.Id = edit.Id;
            mainUser.RoleId = edit.RoleId;
            mainUser.FirstName = edit.FirstName;
            mainUser.LastName = edit.LastName;
            mainUser.Mobile = edit.Mobile;
            mainUser.Email = edit.Email;
            mainUser.IsBlocked = edit.IsBlocked;
            mainUser.IsMobileActive = edit.IsMobileActivated;

            _userRepository.EditEntityByUser(mainUser, username);
            await _userRepository.SaveChanges();

            return EditUserResult.Success;
        }
        #endregion

        #region Role

        public async Task<FilterRoleDto> FilterRole(FilterRoleDto filter)
        {
            var query = _roleRepository
                .GetQuery()
                .Include(x => x.Users)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.RoleName))
            {
                query = query.Where(x => EF.Functions.Like(x.RoleName, $"%{filter.RoleName}%"));
            }

            #region Paging

            var roleCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, roleCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).ToListAsync();


            #endregion

            return filter.SetPaging(pager).SetRoles(allEntities);
        }
        public async Task<CreateRoleResult> CreateRole(CreateRoleDto role)
        {
            var newRole = new Role
            {
                RoleName = role.RoleName,
            };

            await _roleRepository.AddEntity(newRole);
            await _roleRepository.SaveChanges();

            return CreateRoleResult.Success;
        }
        public async Task<EditRoleDto> GetRoleForEdit(long roleId)
        {
            var role = await _roleRepository.GetEntityById(roleId);
            if (role == null)
            {
                return null;
            }

            return new EditRoleDto
            {
                Id = role.Id,
                RoleName = role.RoleName
            };
        }
        public async Task<EditRoleResult> EditRole(EditRoleDto edit, string username)
        {
            var mainRole = await _roleRepository
                .GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == edit.Id);

            if (mainRole == null)
            {
                return EditRoleResult.Error;
            }

            mainRole.Id = edit.Id;
            mainRole.RoleName = edit.RoleName;

            _roleRepository.EditEntityByUser(mainRole, username);
            await _roleRepository.SaveChanges();

            return EditRoleResult.Success;
        }
        public async Task<List<Role>> GetRoles()
        {
            return await _roleRepository
                .GetQuery()
                .AsQueryable()
                .Select(x => new Role
                {
                    Id = x.Id,
                    RoleName = x.RoleName
                })
                .ToListAsync();

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
