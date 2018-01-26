using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imow.Core.cookieModel
{
   public class AdminTicketCookie : ICookieModel
    {
       public string GetCookieKey()
       {
           return "ec.Admin";
       }
    }
}
