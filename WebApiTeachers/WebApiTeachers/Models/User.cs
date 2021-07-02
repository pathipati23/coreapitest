using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiTeachers.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Username { get; set; }

       
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
