using System;
using System.Collections.Generic;
using ThoughTrace.ContactManager.Data.Model;

namespace ThoughTrace.ContactManager.Data
{
    public interface IContactRepository
    {
        Guid AddContact(Contact newContact);
        void RemoveContact(Guid contactId);
        List<Contact> GetContacts();
        Contact GetContact(Guid contactId);
        void UpdateContact(Guid contactId, Contact contactToUpdate);

    }
}