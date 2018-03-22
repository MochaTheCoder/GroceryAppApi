using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroceryAppApi.Models
{
    public class ItemModel
    {
        public string item_name { get; set; }
        public string group_uid { get; set; }
        public decimal? item_price { get; set; }
    }
}