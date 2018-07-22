using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroceryAppApi.Models.EntityModels
{
    public class GroupModel
    {
        public string group_uid { get; set; }
        public string group_name { get; set; }
        public string access_code { get; set; }
    }
}