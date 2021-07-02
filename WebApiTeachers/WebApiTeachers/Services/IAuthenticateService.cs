using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTeachers.Models;


namespace WebApiTeachers.Services
{
    public interface IAuthenticateService
    {

        User Athenticate(string Username, string Password);

        string Base64Decode(string base64EncodedData);
    }
}
