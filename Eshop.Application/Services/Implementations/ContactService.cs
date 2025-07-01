using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Contact;
using Eshop.Domain.Dtos.Contact.Ticket;
using Eshop.Domain.Entities.Contact;
using Eshop.Domain.Entities.Contact.Ticket;
using Eshop.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Application.Services.Implementations
{
    public class ContactService : IContactService
    {

        #region Fields

        private readonly IGenericRepository<ContactUs> _contactUsRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<TicketMessage> _ticketMessageRepository;


        #endregion

        #region Constructor

        public ContactService(IGenericRepository<ContactUs> contactUsRepository, IGenericRepository<Ticket> ticketRepository, IGenericRepository<TicketMessage> ticketMessageRepository)
        {
            _contactUsRepository = contactUsRepository;
            _ticketRepository = ticketRepository;
            _ticketMessageRepository = ticketMessageRepository;
        }

        #endregion


        #region Contact Us

        public async Task CreateContactUs(CreateContactUsDto contact, string userIp, long? userId)
        {
            // todo : Use Sanitizer to sanitize input data

            var newContact = new ContactUs
            {
                UserId = (userId != null && userId.Value != 0) ? userId.Value : (long?)null,
                UserIp = userIp,
                Email = contact.Email,
                Fullname = contact.Fullname,
                MessageSubject = contact.MessageSubject,
                MessageText = contact.MessageText,
            };

            await _contactUsRepository.AddEntity(newContact);
            await _contactUsRepository.SaveChanges();
        }

        public async Task<List<FilterTicketDto>> TicketList()
        {
            
        }

        #endregion

        #region Ticket

        public async Task<AddTicketResult> AddUserTicket(AddTicketDto ticket, long userId)
        {
            if (string.IsNullOrWhiteSpace(ticket.Text))
            {
                return AddTicketResult.Error;
            }

            var newTicket = new Ticket
            {
                OwnerId = userId,
                Title = ticket.Title,
                IsReadByOwner = true,
                TicketSection = ticket.TicketSection,
                TicketPriority = ticket.TicketPriority,
                TicketState = TicketState.UnderProgress,
            };

            await _ticketRepository.AddEntity(newTicket);
            await _ticketRepository.SaveChanges();


            var newMessage = new TicketMessage
            {
                TicketId = newTicket.Id,
                SenderId = userId,
                Text = ticket.Text,
            };

            await _ticketMessageRepository.AddEntity(newMessage);
            await _ticketMessageRepository.SaveChanges();   

            return AddTicketResult.Success;
        }
        public async Task<TicketDetailDto> GetTicketDetail(long ticketId, long userId)
        {
            var ticket = await _ticketRepository
                .GetQuery()
                .AsQueryable()
                .Include(x => x.Owner)
                .OrderByDescending(x => x.CreateDate)
                .SingleOrDefaultAsync(x => x.Id == ticketId && !x.IsDelete);

            var ticketMessage = await _ticketMessageRepository
                .GetQuery()
                .AsQueryable()
                .Include(x => x.Ticket)
                .Where(x => x.TicketId == ticketId && !x.IsDelete)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            if (ticket == null || ticket.OwnerId != userId)
            {
                return null;
            }

            return new TicketDetailDto
            {
                Ticket = ticket,
                TicketMessage = ticketMessage,
            };

        }

        #endregion



        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (_contactUsRepository != null)
            {
                await _contactUsRepository.DisposeAsync();
            }
        }

        

        #endregion
    }
}
