using System.Security.Claims;
using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Account.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region Fields

        public static string ReturnUrl { get; set; }


        private readonly IUserService _userService;



        #endregion

        #region Constructor

        public AccountController(IUserService userService)
        {
            _userService = userService;
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
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUser(register);

                switch (result)
                {
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
                        return RedirectToAction("ActivateMobile", "Account");
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

        #endregion
    }
}
