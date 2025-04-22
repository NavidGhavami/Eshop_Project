using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Account.User;
using Eshop.Domain.Entites.Account.Role;
using Eshop.Domain.Entites.Account.User;
using Eshop.Domain.Repository;

namespace Eshop.Application.Services.Implementations
{
    public class UserService : IUserService
    {

        #region Fields

        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;


        #endregion

        #region Constructor

        public UserService(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        #endregion

        #region Account
        public Task<RegisterUserResult> RegisterUser(RegisterUserDto register)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserExistByMobileNumber(string mobile)
        {
            throw new NotImplementedException();
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
