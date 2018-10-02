using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ThoughTrace.ContactManager.Data.Model;

namespace ThoughTrace.ContactManager.Data
{
    public class InMemoryGroupRepository: IGroupRepository
    {
        private readonly ConcurrentDictionary<Guid, Group> _groups;
        private IContactRepository _contactRepository;


        public InMemoryGroupRepository(ConcurrentDictionary<Guid, Group> groups, IContactRepository contactRepository)
        {
            _groups = groups;
            _contactRepository = contactRepository;
        }

        public Guid AddGroup(Group newGroup)
        {
            newGroup.Id = Guid.NewGuid();
            var added = _groups.TryAdd(newGroup.Id, newGroup);
            if (!added) throw new Exception("Failed to add group");

            return newGroup.Id;
        }

        public void RemoveGroup(Guid groupId)
        {
            _groups.TryRemove(groupId, out var removedGroup);
        }

        public List<Group> GetGroups()
        {
            return _groups.Values.OrderBy(x => x.Name).ToList();
        }

        public Group GetGroup(Guid groupId)
        {
            _groups.TryGetValue(groupId, out var retrievedGroup);

            return retrievedGroup;
        }

        public void UpdateGroup(Guid groupId, Group groupToUpdate)
        {
            var group = GetGroup(groupId);
            if (group == null) throw new ArgumentException($"Group does not exist: {groupId.ToString()}", nameof(groupId));

            groupToUpdate.Id = groupId;
            _groups.AddOrUpdate(groupId, groupToUpdate, (key, oldValue) => groupToUpdate);
        }

        public Guid? AddGroupContactByIds(Guid groupId, BatchIdList contactIds)
        {
            Group group = GetGroup(groupId);
            if (group == null) throw new ArgumentException($"Group does not exist: {groupId.ToString()}", nameof(groupId));
            foreach (Guid contactId in contactIds.Ids)
            {
                Contact contact = _contactRepository.GetContact(contactId);
                if (contact == null) throw new ArgumentException($"contact does not exist: {contactId.ToString()}", nameof(contactId));
                group.contacts.Add(contact);
            }
            return group.Id;
        }

        public void DeleteGroupContactByIds(Guid groupId, BatchIdList contactIds)
        {
            Group group = GetGroup(groupId);
            if (group == null) throw new ArgumentException($"Group does not exist: {groupId.ToString()}", nameof(groupId));
            foreach (Guid contactId in contactIds.Ids)
            {
                Contact contact = _contactRepository.GetContact(contactId);
                if (contact == null) throw new ArgumentException($"contact does not exist: {contactId.ToString()}", nameof(contactId));
                group.contacts.Remove(contact);
            }

        }
    }
}
