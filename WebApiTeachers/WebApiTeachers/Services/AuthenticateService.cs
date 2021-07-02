using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiTeachers.Helpers;
using WebApiTeachers.Models;

namespace WebApiTeachers.Services
{


    public class AuthenticateService : IAuthenticateService
    {

        private readonly AppSettings _appSettings;


        public  AuthenticateService(IOptions<AppSettings> appSettings){

            _appSettings = appSettings.Value;
        }

        private List<User> users = new List<User>() { 
        new User{Id=1,Username="mahesh@gmail.com",Password="mahi@477" }
        };


        public User Athenticate(string UserName, string password)
        {
            var user = users.SingleOrDefault(x => x.Username == UserName && x.Password == password);

            if(user == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDesciper = new SecurityTokenDescriptor
            {

                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Version, "V3.1")
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDesciper);
            user.Token = tokenHandler.WriteToken(token);

            user.Password = null;
            return user;


               
            }

        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
             return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
