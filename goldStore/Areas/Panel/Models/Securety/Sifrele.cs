using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace goldStore.Areas.Panel.Models.Securety
{
    public class Sifrele
    {
        public static string Hash(string value)
        {
            return Convert.ToBase64String(
                      System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value))
                      );

        }
      
    }
}