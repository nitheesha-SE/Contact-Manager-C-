using System;
using System.Collections.Generic;
using System.Web.Http;
using ThoughTrace.ContactManager.Data;
using ThoughTrace.ContactManager.Data.Model;

namespace ThoughtTrace.ContactManager.WebApi.Controllers
{
    public class ContactController : ApiController
    {
        private readonly IContactRepository _contactRepository;

        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public IEnumerable<Contact> Get()
        {
            return _contactRepository.GetContacts();
        }

        public IHttpActionResult Get(Guid id)
        {
            var contact = _contactRepository.GetContact(id);
            if (contact == null) return NotFound();

            return Ok(contact);
        }

        public IHttpActionResult Post([FromBody]Contact value)
        {
            var newContactId = _contactRepository.AddContact(value);
            return Ok(newContactId);
        }

        public IHttpActionResult Put(Guid id, [FromBody]Contact value)
        {
            _contactRepository.UpdateContact(id, value);
            return Ok();
        }

        public IHttpActionResult Delete(Guid id)
        {
            _contactRepository.RemoveContact(id);
            return Ok();
        }


    }
}