using System;

namespace ThoughTrace.ContactManager.Data
{
    public class GroupRepository
    {
        private readonly ConcurrentDictionary<Guid, Group> _groups;
        public GroupRepository(ConcurrentDictionary<Guid, Group> groups)
        {
            _groups = groups;
        }
        public Guid AddGroup(Group newGroup)
        {
            newGroup.Id = Guid.NewGuid();
            var added = _groups.TryAdd(newGroup.Id, newGroup);
            if (!added) throw new Exception("Failed to add contact");

            return newGroup.Id;
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
            if (group == null) throw new ArgumentException($"Contact does not exist: {groupId.ToString()}", nameof(groupId));

            groupToUpdate.Id = groupId;
            _contacts.AddOrUpdate(groupId, groupToUpdate, (key, oldValue) => groupToUpdate);
        }

    }
}
