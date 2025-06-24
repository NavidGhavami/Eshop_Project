using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Account.User;
using MarketPlace.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;

        public AccountController(IUserService userService, IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
        }


        #region EditProfile

        [HttpGet("edit-profile")]

        public async Task<IActionResult> EditProfile(long userId)
        {
            var userProfile = await _userService.GetProfileForEdit(User.GetUserId());
            if (userProfile == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            return View(userProfile);
        }

        [HttpPost("edit-profile"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditUserProfileDto editProfile, IFormFile avatar)
        {
            if (ModelState.IsValid || editProfile.Avatar == null)
            {
                var result = await _userService.EditUserProfile(editProfile, User.GetUserId(), avatar);

                switch (result)
                {
                    case EditUserProfileResult.IsBlocked:
                        TempData[ErrorMessage] = "حساب کاربری شما بلاک شده است";
                        break;
                    case EditUserProfileResult.IsNotActive:
                        TempData[ErrorMessage] = "حساب کاربری شما فعال نشده است";
                        TempData[InfoMessage] = "لطفا با پشتیبانی سایت تماس حاصل فرمایید تا حساب کاربری تان را فعال نمایند";
                        break;
                    case EditUserProfileResult.NotFound:
                        TempData[ErrorMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case EditUserProfileResult.Success:
                        TempData[SuccessMessage] = $"{editProfile.FirstName} عزیز، اطلاعات شما با موفقیت ویرایش گردید";
                        return RedirectToAction("Dashboard", "Home");
                }
            }

            return View(editProfile);
        }

        #endregion

        #region Change User Password

        [HttpGet("change-password")]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost("change-password"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto passwordDto)
        {

            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserById(User.GetUserId());
                var currentPassword = _passwordHasher.EncodePasswordMd5(passwordDto.CurrentPassword);

                if (currentPassword == user.Password)
                {
                    var res = await _userService.ChangeUserPassword(passwordDto, User.GetUserId());
                    if (res)
                    {
                        TempData[SuccessMessage] = "رمز عبور شما تغییر یافت";
                        TempData[InfoMessage] = "لطفا جهت تکمیل فرایند تغییر رمز عبور ، مجددا وارد سایت شوید";
                        await HttpContext.SignOutAsync();
                        return RedirectToAction("Login", "Account", new { area = "" });
                    }
                    else
                    {
                        TempData[ErrorMessage] = "لطفا از کلمه ی عبور جدیدی استفاده کنید";
                    }
                }
                else
                {
                    TempData[ErrorMessage] = "رمز عبور فعلی شما اشتباه می باشد";
                }



            }

            TempData[WarningMessage] = "در ثبت اطلاعات خطایی رخ داد !";
            TempData[InfoMessage] = "لطفا مجددا تلاش فرمایید";
            return View(passwordDto);
        }

        #endregion


    }
}
