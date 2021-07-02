using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TeachersUIWeb.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace TeachersUIWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public List<Teacher> ts;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> LoginAsync(User u)
        {
            try
            {
                HttpClient Client = new HttpClient();
                Client.BaseAddress = new Uri("https://localhost:44330");
                u.RememberMe = true;

                string credentials = String.Format("{0}:{1}", u.Email, u.Password);
                byte[] bytes = Encoding.ASCII.GetBytes(credentials);
                string base64 = Convert.ToBase64String(bytes);
                string authorization = String.Concat("Basic ", base64);
                //request.Headers.Add("Authorization", authorization);
                Client.DefaultRequestHeaders.Add("Authorization", authorization);

                HttpResponseMessage response = Client.GetAsync("Authentication/token").Result;
                response.EnsureSuccessStatusCode();
                string apiResponse = await response.Content.ReadAsStringAsync();
                UserLogin plist = JsonConvert.DeserializeObject<UserLogin>(apiResponse);


                //HttpResponseMessage response = Client.GetAsync("login").Result;
                //    response.EnsureSuccessStatusCode();
                //    string apiResponse = await response.Content.ReadAsStringAsync();
                //    UserLogin  plist = JsonConvert.DeserializeObject<UserLogin>(apiResponse);
                // var obj = plist.Where(a => a.Email.Equals(u.Email) && a.Password.Equals(u.Password)).FirstOrDefault();
                if (plist != null)
                {
                    HttpContext.Session.SetString("token", plist.token);
                    return Redirect("TeachersList");
                }
                else
                {
                    return Redirect("Error");
                }
            }
            catch (Exception e) {
                return Redirect("Error");
            }
        }
        public ActionResult Login()
        {          
            return View();

        }
        public ActionResult TeachersList()
        {
            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public ActionResult BindGrid([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                decimal companyId = 0;
                List<Models.Company> lst = new List<Models.Company>();
                lst = GetGridData(Convert.ToInt32(companyId)).ToList();
                DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.Company
                {
                    Id = p.Id,
                    Name = p.Name,
                    CompanyId = p.CompanyId,
                });
                return Json(result);
            }
            catch (Exception ex)
            {
                var errorMsg = ex.Message.ToString();
                return Json(errorMsg);
            }
        }
        public IEnumerable<Models.Company> GetGridData(decimal companyId)
        {
            List<Models.Company> objCmp = new List<Models.Company>();
            List<Models.Company> listCompany = new List<Models.Company>();
            objCmp.Add(new Models.Company() { Id = 1, Name = "Rupesh Kahane", CompanyId = 20 });
            objCmp.Add(new Models.Company() { Id = 2, Name = "Vithal Wadje", CompanyId = 40 });
            objCmp.Add(new Models.Company() { Id = 3, Name = "Jeetendra Gund", CompanyId = 30 });
            objCmp.Add(new Models.Company() { Id = 4, Name = "Ashish Mane", CompanyId = 15 });
            objCmp.Add(new Models.Company() { Id = 5, Name = "Rinku Kulkarni", CompanyId = 18 });
            objCmp.Add(new Models.Company() { Id = 6, Name = "Priyanka Jain", CompanyId = 22 });
            if (companyId > 0)
            {
                listCompany = objCmp.ToList().Where(a => a.CompanyId <= companyId).ToList();
                return listCompany.AsEnumerable();
            }
            return objCmp.ToList().AsEnumerable();
        }

        public ActionResult Select([DataSourceRequest] DataSourceRequest request)
        {
            GetTeachersDataAsync();
            return Json(ts.ToDataSourceResult(request));
        }

        public async Task<IEnumerable<Teacher>> GetTeachersDataAsync()
        {
            HttpClient Client = new HttpClient();
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("token"))){
                //    Client.DefaultRequestHeaders.Authorization =
                //new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJyb2xlIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3ZlcnNpb24iOiJWMy4xIiwibmJmIjoxNjI0OTY4NDE1LCJleHAiOjE2MjUxNDEyMTUsImlhdCI6MTYyNDk2ODQxNX0.nAnb3OXhor4FugVn0V54FMvrvgma5GezsYyaJrKYvkA");

                Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            }
            Client.BaseAddress = new Uri("https://localhost:44330/");
            HttpResponseMessage response = Client.GetAsync("/teacher").Result;
            response.EnsureSuccessStatusCode();
            string apiResponse = await response.Content.ReadAsStringAsync();
            List<Teacher> plist = JsonConvert.DeserializeObject<List<Teacher>>(apiResponse);
            ts = plist;
            return plist.AsEnumerable();
        }
    }
}
