using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Account.User;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
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


    }
}
