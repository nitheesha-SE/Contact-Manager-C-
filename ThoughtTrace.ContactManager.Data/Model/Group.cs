using System;
using System.Collections.Generic;

namespace ThoughTrace.ContactManager.Data.Model
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }      
        public List<Contact> contacts { get; set; }

        public Group()
        {
            contacts = new List<Contact>();
        }
    }

}
