using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Account.User;
using Eshop.Domain.Entites.Account.Role;
using Eshop.Domain.Entites.Account.User;
using Eshop.Domain.Repository;
using MarketPlace.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Application.Services.Implementations
{
    public class UserService : IUserService
    {

        #region Fields

        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IPasswordHasher _passwordHasher;


        #endregion

        #region Constructor

        public UserService(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
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
                        MobileActiveCode = new Random().Next(10000,99999).ToString(),
                        EmailActiveCode = Guid.NewGuid().ToString("N"),
                        Avatar = null,
                        RoleId = 2,
                    };

                    await _userRepository.AddEntity(user);
                    await _userRepository.SaveChanges();

                    //todo : Send Message to Mobile

                    return RegisterUserResult.Success;
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
