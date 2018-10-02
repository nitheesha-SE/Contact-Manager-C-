using System;
using System.Collections.Generic;
using ThoughTrace.ContactManager.Data.Model;

namespace ThoughTrace.ContactManager.Data
{
    public interface IGroupRepository
    {
        Guid AddGroup(Group newGroup);
        void RemoveGroup(Guid groupId);
        List<Group> GetGroups();
        Group GetGroup(Guid groupId);
        void UpdateGroup(Guid groupId, Group groupToUpdate);
        Guid? AddGroupContactByIds(Guid id, BatchIdList contactIds);
        void DeleteGroupContactByIds(Guid groupId, BatchIdList contactIds);
    }
}
