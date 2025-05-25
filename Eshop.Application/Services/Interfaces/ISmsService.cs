namespace Eshop.Application.Services.Interfaces;

public interface ISmsService
{
    Task SendVerificationSms(string mobile, string activationCode);

}