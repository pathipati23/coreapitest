using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApiTeachers.Filters
{

    [AttributeUsage(AttributeTargets.Class)]
    public class CustomEmployeeAuthentication : Attribute, IAuthorizationFilter
    {
        private IHeaderDictionary headers;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Microsoft.Extensions.Primitives.StringValues authTokens;
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out authTokens);
            headers = context.HttpContext.Request.Headers;

            var _token = authTokens.FirstOrDefault();

            if (_token != null)
            {
                string authToken = _token;
                if (authToken != null)
                {
                    if (checkTokenValid(authToken))
                    {
                        return;
                    }
                    else
                    {
                        try
                        {
                            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Unauthorized";
                            context.Result = new JsonResult("Unauthorized")
                            {
                                Value = new
                                {
                                    error = "Invalid Token"
                                },
                            };
                        }
                        catch (Exception ex) {
                           
                    }
                }
            }
            else
            {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide Authorization Token";
                    context.Result = new JsonResult("Please Provide Authorization Token")
                {
                    Value = new
                    {
                        error = "Please Provide Authorization Token"
                    },
                };
            }
            }
            else {

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide Authorization Token";
                context.Result = new JsonResult("Please Provide Authorization Token")
                {
                    Value = new
                    {
                        error = "Please Provide Authorization Token"
                    },
                };
            }
    }


        public bool checkTokenValid(string token) {
            if (token.Contains("Bearer"))
                token = token.Replace("Bearer ", "");

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var mySecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Hi this is mahesh"));

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,


                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                
                return false;
            }

            return true;
        }

    }
}
