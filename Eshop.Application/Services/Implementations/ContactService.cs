using System.Runtime.CompilerServices;
using Eshop.Application.Services.Interfaces;
using Eshop.Domain.Dtos.Contact;
using Eshop.Domain.Entities.Contact;
using Eshop.Domain.Repository;

namespace Eshop.Application.Services.Implementations
{
    public class ContactService : IContactService
    {

        #region Fields

        private readonly IGenericRepository<ContactUs> _contactUsRepository;


        #endregion

        #region Constructor

        public ContactService(IGenericRepository<ContactUs> contactUsRepository)
        {
            _contactUsRepository = contactUsRepository;
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
