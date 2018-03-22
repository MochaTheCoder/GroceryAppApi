using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GroceryAppApi.Controllers
{
    public class GroupsController : ApiController
    {
        groceryappEntities dbContext = new groceryappEntities();

        [Authorize]
        [HttpGet]
        [Route("api/groups")]
        public HttpResponseMessage getGroups()
        {
            string user_uid = User.Identity.GetUserId();
            List<string> group_uids = dbContext.users_groups.Where(e => e.user_uid == user_uid).Select(s => s.group_uid).ToList();
            List<group> user_groups = dbContext.groups.Where(e => group_uids.Contains(e.group_uid)).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, user_groups);
        }

        [Authorize]
        [HttpPost]
        [Route("api/groups/{access_code}")]
        public HttpResponseMessage addGroup(string access_code)
        {
            string user_uid = User.Identity.GetUserId();
            group _group = dbContext.groups.Where(e => e.access_code == access_code).FirstOrDefault();
            if(_group == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);

            }
            users_groups n_users_groups = new users_groups()
            {
                group_uid = _group.group_uid,
                user_uid = User.Identity.GetUserId(),
                USER_GROUP_JUNCTION = Guid.NewGuid().ToString()
            };
            try
            {
                dbContext.users_groups.Add(n_users_groups);
                dbContext.SaveChanges();
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Database could not be saved");
            }
            return Request.CreateResponse(HttpStatusCode.OK, n_users_groups.USER_GROUP_JUNCTION);
        }

        [Authorize]
        [HttpPost]
        [Route("api/groups/new/{_group_name}")]
        public HttpResponseMessage newGroup(string _group_name)
        {
            string group_uid = Guid.NewGuid().ToString();
            group n_group = new group()
            {
                group_name = _group_name,
                group_uid = group_uid
            };
            dbContext.groups.Add(n_group);

            users_groups n_users_groups = new users_groups()
            {
                group_uid = group_uid,
                user_uid = User.Identity.GetUserId(),
                USER_GROUP_JUNCTION = Guid.NewGuid().ToString()
            };

            dbContext.users_groups.Add(n_users_groups);

            try
            {
                dbContext.SaveChanges();

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Database could not be saved");

            }
            var returnObject = new { group_name = n_group.group_name, group_uid = n_group.group_uid }; // need to do this since group has database fields can't serialize
            return Request.CreateResponse(HttpStatusCode.OK, returnObject);
        }
    }
}
