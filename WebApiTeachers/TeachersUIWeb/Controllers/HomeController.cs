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
        public List<Teacher> teachers;
        public List<Teacher> tchrs;

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

        public ActionResult Select([DataSourceRequest] DataSourceRequest request)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("skillsApply")))
            {
                GetTeachersDataAsync();
            }
            List<Teacher> _sessionList = SessionHelper.GetObjectFromJson<List<Teacher>>(HttpContext.Session, "teachersList");

            return Json(_sessionList.ToDataSourceResult(request));
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
            SessionHelper.SetObjectAsJson(HttpContext.Session, "teachersList", plist);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "tempteachersList", plist);
            ts = plist;
            teachers = plist;
            return teachers.AsEnumerable();
        }

        public ActionResult SelectSkills([DataSourceRequest] DataSourceRequest request)
        {
            GetTeachersDataAsync();
            List<Teacher> lt= SessionHelper.GetObjectFromJson<List<Teacher>>(HttpContext.Session, "tempteachersList");
            return Json(lt.Select(s => s.Skills).ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult MultselctPostback([DataSourceRequest] DataSourceRequest request, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                GetTeachersDataAsync();
               teachers = new List<Teacher>();
        List<string> sdt = collection["Skills"].ToList();
                if (sdt.Count > 0) {
                    //ts = ts.Select(s => s.Skills.Contains(sdt));
                    foreach (string st in sdt)
                    {
                        foreach (Teacher ta in ts)
                        {
                            if (ta.Skills == st)
                            {
                                teachers.Add(ta);
                            }

                        }
                    }
                  
                }
                else { teachers = ts; }

        }

             return Redirect("TeachersList");
        }
        [HttpPost]
        public List<Teacher> selectedItems(List<string> sdt)
        {
           
            List<Teacher> _sessionList = SessionHelper.GetObjectFromJson<List<Teacher>>(HttpContext.Session, "tempteachersList");
           
            teachers = new List<Teacher>();
           
            if (sdt.Count > 0)
            {
                //ts = ts.Select(s => s.Skills.Contains(sdt));
                foreach (string st in sdt)
                {
                    foreach (Teacher ta in _sessionList)
                    {
                        if (ta.Skills == st)
                        {
                            teachers.Add(ta);
                        }

                    }
                }

            }
            if (sdt.Count() > 0)
            {
                HttpContext.Session.SetString("skillsApply", "yes");
                SessionHelper.SetObjectAsJson(HttpContext.Session, "teachersList", teachers);
            }
            else {
                SessionHelper.SetObjectAsJson(HttpContext.Session, "teachersList", _sessionList);
                 }
           
            return teachers;
        }

    }
}
