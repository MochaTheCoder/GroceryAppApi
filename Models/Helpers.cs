using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GroceryAppApi.Models
{
    public static class Helpers
    {
        /// <summary>
        /// Returns a random character string. Maximum of 11 characters
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomString(int length)
        {
            string randomString = Path.GetRandomFileName();
            randomString = randomString.Replace(".", ""); // Remove period
            return randomString.Substring(0, length);
        }
    }
}