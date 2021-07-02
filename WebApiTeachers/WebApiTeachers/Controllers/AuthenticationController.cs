using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTeachers.Models;
using WebApiTeachers.Services;

namespace WebApiTeachers.Controllers
{


    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IHeaderDictionary headers;
        private IAuthenticateService _service;
        public AuthenticationController(IAuthenticateService iauth) {

            _service = iauth;
        }

        [HttpPost]
        public IActionResult Post([FromBody] User model)
        {

            var user = _service.Athenticate(model.Username, model.Password);
            if (user == null)
            {

                return BadRequest(new { mesaage = "User name and Password not correct " });
            }
            else
            {

                return Ok(user);
            }


        }

       
        [HttpGet("token")]
        public IActionResult Token()
        {
            Microsoft.Extensions.Primitives.StringValues authTokens;
            Request.Headers.TryGetValue("Authorization", out authTokens);
            headers = Request.Headers;

            var _token = authTokens.FirstOrDefault();
            if (_token != null)
            {
                if (_token.Contains("Basic"))
                    _token = _token.Replace("Basic ", "");
                string st = _service.Base64Decode(_token);

                var user = _service.Athenticate(st.Split(":").FirstOrDefault(), st.Split(":").LastOrDefault());
                if (user == null)
                {

                    return BadRequest(new { mesaage = "User name and Password not correct " });
                }
                else
                {

                    return Ok(user);
                }
            }
            else{ return BadRequest(new { mesaage = "User name and Password not correct " }); }


        }
        
           
    }
}
