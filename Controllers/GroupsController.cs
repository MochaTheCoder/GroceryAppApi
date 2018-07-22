using GroceryAppApi.Models.EntityModels;
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
        groceryappdbEntities dbContext = new groceryappdbEntities();

        [Authorize]
        [HttpGet]
        [Route("api/groups")]
        public HttpResponseMessage getGroups()
        {
            // Left off returning the entity.. need to return a model obj
            string user_uid = User.Identity.GetUserId();
            List<GroupModel> user_groups = (from groups in dbContext.groups
                         join group_ids in dbContext.user_groups
                         on groups.group_uid equals group_ids.group_uid
                         where group_ids.user_uid == user_uid
                         select groups).ToList();
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
            user_groups n_users_groups = new user_groups()
            {
                group_uid = _group.group_uid,
                user_uid = User.Identity.GetUserId(),
                user_group_PK = Guid.NewGuid().ToString()
            };
            try
            {
                dbContext.user_groups.Add(n_users_groups);
                dbContext.SaveChanges();
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Database could not be saved");
            }
            return Request.CreateResponse(HttpStatusCode.OK, n_users_groups.user_group_PK);
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

            user_groups n_user_groups = new user_groups()
            {
                group_uid = group_uid,
                user_uid = User.Identity.GetUserId(),
                user_group_PK = Guid.NewGuid().ToString()
            };

            dbContext.user_groups.Add(n_user_groups);

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
