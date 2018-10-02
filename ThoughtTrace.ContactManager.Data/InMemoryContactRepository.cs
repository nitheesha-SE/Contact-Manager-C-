using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoughTrace.ContactManager.Data.Model;

namespace ThoughTrace.ContactManager.Data
{
    public class InMemoryContactRepository : IContactRepository
    {
        private readonly ConcurrentDictionary<Guid, Contact> _contacts;

        public InMemoryContactRepository(ConcurrentDictionary<Guid, Contact> contacts)
        {
                _contacts = contacts;
        }

        public Guid AddContact(Contact newContact)
        {
            newContact.Id = Guid.NewGuid();
            var added = _contacts.TryAdd(newContact.Id, newContact);
            if(!added) throw new Exception("Failed to add contact");

            return newContact.Id;
        }

        public void RemoveContact(Guid contactId)
        {
            _contacts.TryRemove(contactId, out var removedContact);
        }

        public List<Contact> GetContacts()
        {
            return _contacts.Values.OrderBy(x => x.FirstName).ToList();
        }

        public Contact GetContact(Guid contactId)
        {
            _contacts.TryGetValue(contactId, out var retrievedContact);

            return retrievedContact;
        }

        public void UpdateContact(Guid contactId, Contact contactToUpdate)
        {
            var contact = GetContact(contactId);
            if (contact == null) throw new ArgumentException($"Contact does not exist: {contactId.ToString()}", nameof(contactId));

            contactToUpdate.Id = contactId;
            _contacts.AddOrUpdate(contactId, contactToUpdate, (key, oldValue) => contactToUpdate);
        }
    }
}
