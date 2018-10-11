using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            using (PhoneBookDbEntities obj = new PhoneBookDbEntities())
            {
                return View(obj.People.ToList());
            }
        }

        // GET: Test/Details/5
        public ActionResult Details(int id)
        {
            using (PhoneBookDbEntities obj = new PhoneBookDbEntities())
            {
                return View(obj.People.Where(x => x.PersonId == id).FirstOrDefault());
            }
            
        }

        // GET: Test/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Test/Create
        [HttpPost]
        public ActionResult Create(Class1 obj)
        {
            string chk = "hello";
            string lol = "conn";
            try
            {
                // TODO: Add insert logic here
                Person p = new Person();
                p.FirstName = obj.FirstName;
                p.MiddleName = obj.MiddleName;
                p.LastName = obj.LastName;
                p.DateOfBirth = obj.DateofBirth;
                p.AddedOn = DateTime.Now;
                p.AddedBy = User.Identity.GetUserId();
                p.HomeAddress = obj.HomeAddress;
                p.HomeCity = obj.HomeCity;
                p.FaceBookAccountId = obj.FaceBookAccountId;
                p.LinkedInId = chk;
                p.UpdateOn = DateTime.Now;
                p.ImagePath = lol;
                p.TwitterId = obj.TwitterId;
                p.EmailId = obj.EmailId;
                using (PhoneBookDbEntities ent = new PhoneBookDbEntities())
                {
                    ent.People.Add(p);
                    ent.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Test/Edit/5
        public ActionResult Edit(int id)
        {
            using (PhoneBookDbEntities obj = new PhoneBookDbEntities())
            {
                return View(obj.People.Where(x => x.PersonId == id).FirstOrDefault());
            }
        }

        // POST: Test/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Person obj)
        {
            try
            {
                // TODO: Add update logic here
                using (PhoneBookDbEntities ent = new PhoneBookDbEntities())
                {
                    ent.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    ent.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Test/Delete/5
        public ActionResult Delete(int id)
        {
            using (PhoneBookDbEntities obj = new PhoneBookDbEntities())
            {
                return View(obj.People.Where(x => x.PersonId == id).FirstOrDefault());
            }
        }

        // POST: Test/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (PhoneBookDbEntities obj = new PhoneBookDbEntities())
                {
                   Person person = obj.People.Where(x => x.PersonId == id).FirstOrDefault();
                    obj.People.Remove(person);
                    obj.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
