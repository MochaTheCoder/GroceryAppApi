using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GroceryAppApi.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        groceryappdbEntities dbContext = new groceryappdbEntities();
        [HttpGet]
        [Route("api/User/")]
        public HttpResponseMessage getUserInfo()
        {
            string user_uid = User.Identity.GetUserId();
            user _user = dbContext.users.Find(user_uid);
            if(_user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No user");
            }
            return Request.CreateResponse(HttpStatusCode.OK, _user);
        }

    }

}
