using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Account.User;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region Fields

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
                        TempData[InfoMessage] = $"کد تایید، جهت فعالسازی حساب کاربری به شماره همراه {register.Mobile} ارسال گردید.";
                        return RedirectToAction("ActivateMobile", "Account");
                }

       
            }
            return View(register);
        }

        #endregion

        #endregion
    }
}
