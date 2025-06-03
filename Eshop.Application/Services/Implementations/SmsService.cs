using Eshop.Application.Services.Interfaces;
using Kavenegar;
using Microsoft.Extensions.Configuration;

namespace Eshop.Application.Services.Implementations
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendVerificationSms(string mobile, string activationCode)
        {
            var apiKey = _configuration.GetSection("KavenegarSmsApiKey")["apiKey"];
            var api = new Kavenegar.KavenegarApi(apiKey);

            await api.VerifyLookup(mobile, activationCode, "VerifyWebsiteAccount");
        }

        public async Task SendRecoverPasswordSms(string mobile, string newPassword)
        {

            var apiKey = _configuration.GetSection("KavenegarSmsApiKey")["apiKey"];

            var api = new Kavenegar.KavenegarApi(apiKey);

            await api.VerifyLookup(mobile, newPassword, "VerifyRecoverPassword");
        }
    }
}
