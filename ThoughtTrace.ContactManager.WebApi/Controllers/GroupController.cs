using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ThoughTrace.ContactManager.Data;
using ThoughTrace.ContactManager.Data.Model;

namespace ThoughtTrace.ContactManager.WebApi.Controllers
{
    public class GroupController : ApiController
    {
        private readonly IGroupRepository _groupRepository;
        public GroupController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public IHttpActionResult Get()
        {
            return Ok(_groupRepository.GetGroups()) ;
        }
        public IHttpActionResult Get(Guid id)
        {
            var group = _groupRepository.GetGroup(id);
            if (group == null) return NotFound();

            return Ok(group);
        }
        public IHttpActionResult Post([FromBody]Group value)
        {
            if (value == null) return BadRequest();
            var newGroupId = _groupRepository.AddGroup(value);
            return Ok(newGroupId);
        }
        
        public IHttpActionResult Post(Guid groupId, [FromBody]BatchIdList contactIds)
        {
            Guid? newGroup = _groupRepository.AddGroupContactByIds(groupId, contactIds);
            if (newGroup == null) return NotFound();
            return Ok(newGroup);
        }

        public IHttpActionResult Put(Guid id, [FromBody]Group value)
        {
            _groupRepository.UpdateGroup(id, value);
            return Ok();
        }

        public IHttpActionResult Delete(Guid id)
        {
            _groupRepository.RemoveGroup(id);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }
        
            public IHttpActionResult Delete(Guid groupId, [FromBody] BatchIdList contactIds)
        {
            _groupRepository.DeleteGroupContactByIds(groupId, contactIds);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

    }
}
