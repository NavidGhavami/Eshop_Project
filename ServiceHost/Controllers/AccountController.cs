using System.Security.Claims;
using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Account.User;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace ServiceHost.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region Fields

        public static string ReturnUrl { get; set; }


        private readonly IUserService _userService;
        private readonly ICaptchaValidator _captchaValidator;



        #endregion

        #region Constructor

        public AccountController(IUserService userService, ICaptchaValidator captchaValidator)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
        }

        #endregion


        #region Actions

        #region Register User

        [HttpGet("register")]
        public async Task<IActionResult> RegisterUser()
        {

            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserDto register)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(register.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                TempData[InfoMessage] = "لطفا از اتصال اینترنت خود مطمئن شوید";
                return View(register);
            }

            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUser(register);

                var user = await _userService.GetUserByMobile(register.Mobile);

                if (user.IsMobileActive)
                {
                    TempData[InfoMessage] = "حساب کاربری شما قبلا فعال شده است";
                    return RedirectToAction("Login", "Account");
                }

                switch (result)
                {
                    case RegisterUserResult.MobileNotActive:
                        TempData[WarningMessage] = "شماره همراه شما فعال نشده است. لطفا ابتدا شماره همراه خود را فعال نمایید.";
                        TempData[InfoMessage] = "کد شش رقمی به شماره همراه شما ارسال گردید.";
                        ModelState.AddModelError("Mobile", "شماره همراه شما فعال نشده است.");
                        return RedirectToAction("ActivateMobile", "Account", new {mobile = register.Mobile});

                    case RegisterUserResult.MobileExists:
                        TempData[WarningMessage] = $"شماره همراه : {register.Mobile} تکراری می باشد.";
                        ModelState.AddModelError("Mobile", "شماره همراه تکراری می باشد.");
                        break;
                    case RegisterUserResult.Error:
                        TempData[ErrorMessage] = "در ثبت اطلاعات خطایی رخ داد. لطفا دوباره تلاش نمایید.";
                        break;
                    case RegisterUserResult.Success:
                        TempData[SuccessMessage] = "کاربر با موفقیت ثبت گردید.";
                        TempData[InfoMessage] = $"کد تایید، جهت فعالسازی حساب کاربری به شماره همراه {register.Mobile} ارسال گردید.";
                        return RedirectToAction("ActivateMobile", "Account", new { mobile = register.Mobile });
                }
            }
            return View(register);
        }

        #endregion

        #region Login

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto login)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                TempData[InfoMessage] = "لطفا از اتصال اینترنت خود مطمئن شوید";
                return View(login);
            }
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUser(login);

                switch (result)
                {
                    case UserLoginResult.UserNotFound:
                        TempData[WarningMessage] = "کاربری با این مشخصات یافت نشد.";
                        ModelState.AddModelError("Mobile", "کاربری با این مشخصات یافت نشد.");
                        break;
                    case UserLoginResult.MobileNotActivated:
                        TempData[WarningMessage] = "شماره همراه شما فعال نشده است.";
                        ModelState.AddModelError("Mobile", "شماره همراه شما فعال نشده است.");
                        break;
                    case UserLoginResult.Error:
                        TempData[ErrorMessage] = "در ورود به حساب کاربری خطایی رخ داد. لطفا دوباره تلاش نمایید.";
                        break;
                    case UserLoginResult.Success:
                        var user = await _userService.GetUserByMobile(login.Mobile);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.MobilePhone, user.Mobile),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                            new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = login.RememberMe,
                            RedirectUri = HttpContext.Request.Query["RedirectUri"]
                        };

                        await HttpContext.SignInAsync(principal, properties);

                        TempData[SuccessMessage] = "شما با موفقیت وارد سایت شدید";

                        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                            return Redirect(ReturnUrl);
                        else
                            return Redirect("/");
                }
            }

            return View(login);
        }

        #endregion

        #region Logout

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            TempData[WarningMessage] = "شما از حساب کاربری خارج شدید";
            return Redirect("/");
        }

        #endregion

        #region Activate Mobile

        [HttpGet("activate-mobile/{mobile}")]
        public async Task<IActionResult> ActivateMobile(string mobile)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            var activateMobile = new ActiveMobileDto { Mobile = mobile };
            return View(activateMobile);
        }

        [HttpPost("activate-mobile/{mobile}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateMobile(ActiveMobileDto activate)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(activate.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(activate);
            }

            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByMobile(activate.Mobile);

                if (!user.IsMobileActive)
                {
                    var result = await _userService.ActivateMobile(activate);

                    if (result)
                    {
                        TempData[SuccessMessage] = "حساب کاربری شما با موفقیت فعال شد";
                        TempData[InfoMessage] = "جهت ورود به حساب خود شماره موبایل و رمز عبور خود را وارد نمایید";
                        return RedirectToAction("Login", "Account");
                    }
                }else if (user.IsMobileActive)
                {
                    TempData[InfoMessage] = "حساب کاربری شما قبلا فعال شده است";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    TempData[ErrorMessage] = "کد فعالسازی اشتباه است";
                }

                TempData[ErrorMessage] = "کاربری با مشخصات وارد شده یافت نشد";
            }
            return View(activate);
        }

        #endregion

        #region Recover User Password

        [HttpGet("recover-password")]
        public async Task<IActionResult> RecoverUserPassword()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            //var forgetPassword = new ForgotPasswordDto { Mobile = mobile };
            return View();
        }

        [HttpPost("recover-password"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoverUserPassword(ForgotPasswordDto forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(forgot);
            }

            if (ModelState.IsValid)
            {
                var result = await _userService.RecoverUserPassword(forgot);
                switch (result)
                {
                    case ForgotPasswordResult.UserNotFound:
                        TempData[WarningMessage] = "کاربری با این مشخصات یافت نشد.";
                        ModelState.AddModelError("Mobile", "کاربری با این مشخصات یافت نشد.");
                        break;
                    case ForgotPasswordResult.Error:
                        TempData[ErrorMessage] = "در بازیابی رمز عبور خطایی رخ داد. لطفا دوباره تلاش نمایید.";
                        break;
                    case ForgotPasswordResult.Success:
                        TempData[SuccessMessage] = "رمز عبور جدید به شماره همراه شما ارسال شد.";
                        TempData[InfoMessage] = "لطفا پس از ورود به حساب کاربری، رمز عبور خود را تغییر دهید";
                        return RedirectToAction("Login", "Account");
                }
            }

            return View(forgot);
        }

        #endregion

        #endregion


    }
}
