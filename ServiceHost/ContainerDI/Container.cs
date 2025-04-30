using Eshop.Application.Services.Implementations;
using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Context;
using Eshop.Domain.Repository;
using MarketPlace.Application.Services.Implementations;
using MarketPlace.Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ServiceHost.ContainerDI
{
    public static class Container
    {
        public static void RegisterService(this IServiceCollection services)
        {
            #region Repositories

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            

            #endregion

            #region General Services

            services.AddTransient<IUserService, UserService>();


            #endregion

            #region Common Services

            services.AddHttpContextAccessor();
            services.AddTransient<IPasswordHasher, PasswordHasher>();

            #endregion
        }
    }
}
