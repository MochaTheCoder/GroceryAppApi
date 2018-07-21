using GroceryAppApi.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GroceryAppApi.Controllers
{
    //[Authorize]
    public class ItemsController : ApiController
    {
        groceryappEntities dbContext = new groceryappEntities();

        
        //Add item to shopping list
        [HttpPost]
        [Route("api/Items/new")]
        public HttpResponseMessage postItem([FromBody] ItemModel jsonBody)
        {
            group _group = dbContext.groups.Where(e => e.group_uid == jsonBody.group_uid).FirstOrDefault();
            if (_group == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Group Not Found");
            }
            item n_item = new item()
            {
                group_uid = jsonBody.group_uid,
                item_name = jsonBody.item_name,
                item_price = jsonBody.item_price,
                item_uid = Guid.NewGuid().ToString(),
                user_uid = User.Identity.GetUserId(),
                group_name = _group.group_name
            };
            dbContext.items.Add(n_item);
            try
            {
                dbContext.SaveChanges();

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return Request.CreateResponse(HttpStatusCode.OK, n_item);
        }
        
        //Cross off item from shopping list
        [HttpPost]
        [Route("api/Items/cross")]
        public HttpResponseMessage crossItem([FromBody] item itemCrossed)
        {
            item dbItem = dbContext.items.Where(e => e.item_uid == itemCrossed.item_uid).FirstOrDefault();
            dbItem.crossed_off = true;
            try
            {
                dbContext.SaveChanges();

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //UnCross off item from shopping list
        [HttpPost]
        [Route("api/Items/uncross")]
        public HttpResponseMessage uncrossItem([FromBody] item itemCrossed)
        {
            item dbItem = dbContext.items.Where(e => e.item_uid == itemCrossed.item_uid).FirstOrDefault();
            dbItem.crossed_off = false;
            try
            {
                dbContext.SaveChanges();

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [HttpGet]
        [Route("api/Items/")]
        public HttpResponseMessage getItems([FromBody] ItemModel jsonBody)
        {
            string user_uid = User.Identity.GetUserId();
            List<item> items = dbContext.items.Where(e => e.user_uid == user_uid).ToList();
            if(items.Count < 1)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No items");
            }
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }


        [HttpGet]
        [Route("api/Items/{group_uid}")]
        public HttpResponseMessage getItemsFiltered(string group_uid)
        {
            string user_uid = User.Identity.GetUserId();
            List<item> items = dbContext.items.Where(e => e.user_uid == user_uid && e.group_uid == group_uid).ToList();
            if (items.Count < 1)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No items");
            }
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

    }




}

