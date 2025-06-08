using System.Drawing.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Eshop.Application.Services.Implementations;
using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Repository;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using MarketPlace.Application.Services.Implementations;
using MarketPlace.Application.Services.Interfaces;

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

            services.AddTransient<ISiteSettingService, SiteSettingService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISmsService, SmsService>();
            services.AddTransient<IContactService, ContactService>();



            #endregion

            #region Common Services

            services.AddHttpContextAccessor();
            services.AddSingleton<HtmlEncoder>(
                HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

            #endregion
        }
    }
}
