using System.Runtime.CompilerServices;
using Eshop.Application.Services.Implementations;
using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Contact.Ticket;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.Controllers;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.User.Controllers
{
    public class TicketController : UserBaseController
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IContactService _contactService;

        public TicketController(IUserService userService, IContactService contactService)
        {
            _userService = userService;
            _contactService = contactService;
        }

        #endregion


        #region Actions

        [HttpGet("user-tickets")]
        public async Task<IActionResult> TicketList(FilterTicketDto filter)
        {
            filter.UserId = User.GetUserId();
            filter.OrderBy = FilterTicketOrder.CreateDateDescending;

            var result = await _contactService.TicketList(filter);
            return View(result);
        }

        #region Add Ticket

        [HttpGet("add-ticket")]
        public async Task<IActionResult> AddTicket()
        {
            return View();
        }

        [HttpPost("add-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicket(AddTicketDto ticket)
        {
            if (ModelState.IsValid)
            {
                var result = await _contactService.AddUserTicket(ticket, User.GetUserId());

                switch (result)
                {
                    case AddTicketResult.Error:
                        TempData[ErrorMessage] = "در افزودن تیکت خطایی رخ داد";
                        break;
                    case AddTicketResult.Success:
                        TempData[SuccessMessage] = "تیکت شما با موفقیت ثبت شد";
                        TempData[InfoMessage] = "همکاران ما به زودی پاسخ شما را خواهند داد";
                        return RedirectToAction("TicketList", "Ticket", new { area = "User" });
                }
            }

            return View(ticket);
        }



        #endregion

        //#region Show Ticket Details

        //[HttpGet("tickets/{ticketId}")]
        //public async Task<IActionResult> TicketDetail(long ticketId)
        //{
        //    var sanitize = new HtmlSanitizer();
        //    var ticket = await _contactService.GetTicketDetail(ticketId, User.GetUserId());
        //    ViewBag.AvatarImage = await _contactService.GetUserAvatarTicket(ticketId) ?? string.Empty;
        //    ViewBag.AvatarImageAdmin = await _contactService.GetAdminUserAvatarTicket(ticketId) ?? string.Empty;

        //    if (ticket == null)
        //    {
        //        return RedirectToAction("PageNotFound", "Home");
        //    }

        //    return View(ticket);
        //}

        //#endregion

        #endregion
    }
}
