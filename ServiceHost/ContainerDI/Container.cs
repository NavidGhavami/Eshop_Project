using Eshop.Domain.Repository;

namespace ServiceHost.ContainerDI
{
    public static class Container
    {
        public static void RegisterService(this IServiceCollection services)
        {
            #region Repositories

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            #endregion

            #region General Services

            services.AddHttpContextAccessor();


            #endregion
        }
    }
}
