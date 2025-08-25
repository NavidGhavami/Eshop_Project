using Eshop.Application.Services.Interfaces;
using Eshop.Application.Utilities;
using Eshop.Domain.Dtos.Account.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.Administration.Controllers
{
    [Authorize("UserManagement", Roles = Roles.Administrator)]
    public class AccountController : AdminBaseController
    {

        #region Constructor

        private readonly IUserService _userService;
        private readonly ISiteSettingService _siteService;
        private readonly IContactService _contactService;

        public AccountController(IUserService userService,
            ISiteSettingService siteService,
            IContactService contactService)
        {
            _userService = userService;
            _siteService = siteService;
            _contactService = contactService;
        }

        #endregion

        #region User List


        [HttpGet("user-list")]
        public async Task<IActionResult> UserList(FilterUserDto filter)
        {
            var users = await _userService.FilterUser(filter);

            users.Roles = await _userService.GetRoles();
            ViewBag.roles = users.Roles;

            return View(filter);
        }

        #endregion

        #region Edit User

        [HttpGet("edit-user/{userId}")]
        public async Task<IActionResult> EditUser(long userId)
        {
            var user = await _userService.GetUserForEdit(userId);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.roles = await _userService.GetRoles();


            return View(user);
        }

        [HttpPost("edit-user/{userId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserDto edit)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserById(User.GetUserId());
                var username = user.FirstName + " " + user.LastName;
                var result = await _userService.EditUser(edit, username);

                switch (result)
                {
                    case EditUserResult.UserNotFound:
                        TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
                        break;
                    case EditUserResult.Success:
                        TempData[SuccessMessage] = "ویرایش کاربر با موفقیت انجام شد";
                        return RedirectToAction("UserList", "Account");
                }
            }


            ViewBag.roles = await _userService.GetRoles();
            return View();
        }

        #endregion

        #region Role list
        [HttpGet("role-list")]
        public async Task<IActionResult> RoleList(FilterRoleDto filter)
        {
            var role = await _userService.FilterRole(filter);
            return View(filter);
        }

        #endregion

        #region Add Role

        [HttpGet("create-role")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost("create-role"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleDto role)
        {
            var result = await _userService.CreateRole(role);

            switch (result)
            {
                case CreateRoleResult.Error:
                    TempData[ErrorMessage] = "عملیات مورد نظر با خطا مواجه شد";
                    break;
                case CreateRoleResult.Success:
                    TempData[SuccessMessage] = "افزودن نقش با موفقیت انجام شد";
                    return RedirectToAction("RoleList", "Account");
            }

            return View();

        }

        #endregion

        #region Edit Role

        [HttpGet("edit-role/{roleId}")]
        public async Task<IActionResult> EditRole(long roleId)
        {
            var role = await _userService.GetRoleForEdit(roleId);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost("edit-role/{roleId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(EditRoleDto edit)
        {
            var user = await _userService.GetUserById(User.GetUserId());
            var username = user.FirstName + " " + user.LastName;
            var result = await _userService.EditRole(edit, username);

            switch (result)
            {
                case EditRoleResult.Error:
                    TempData[ErrorMessage] = "در ویرایش اطلاعات خطایی رخ داد";
                    break;
                case EditRoleResult.Success:
                    TempData[SuccessMessage] = "ویرایش نقش با موفقیت انجام شد";
                    return RedirectToAction("RoleList", "Account"); ;
            }

            return View();
        }


        #endregion

        #region SignOut

        [HttpGet("log-out")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
