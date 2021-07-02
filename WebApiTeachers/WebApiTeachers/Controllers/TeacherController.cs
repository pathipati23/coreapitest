using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTeachers.Filters;
using WebApiTeachers.Repository;
using WebApiTeachers.Repository.Models;

namespace WebApiTeachers.Controllers
{
    [CustomEmployeeAuthentication]
    [EnableCors]
    [ApiController]
    [Route("[controller]")]

    public class TeacherController : Controller
    {

        private PersonDbContext _db;


        public TeacherController (PersonDbContext db)
        {

            _db = db;
        }

      
        // GET: TeacherController
        public IEnumerable<Teacher> Index()
        {
            return _db.Teachers;
        }
        [Route("/login")]
        public IEnumerable<UserLogin> login()
        {
            return _db.UserLogins;
        }

        // GET: TeacherController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: TeacherController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: TeacherController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: TeacherController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: TeacherController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: TeacherController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: TeacherController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
