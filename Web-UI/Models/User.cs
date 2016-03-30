using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_UI.Models
{
    public class User
    {
        //users desired name/handle
        public string Username { get; set; }
        //users email
        public string Email { get; set; }
        //users password
        public string Password { get; set; }
    }
}