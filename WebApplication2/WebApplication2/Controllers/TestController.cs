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
            PhoneBookDbEntities db = new PhoneBookDbEntities();
            List<Person> list = db.People.ToList();
            List<Person> temp = new List<Person>();
            //List<StudentViewModel> viewList = new List<StudentViewModel>();
            foreach (Person s in list)
            {
                if(s.AddedBy == User.Identity.GetUserId())
                {
                    temp.Add(s);
                }
            }
            
            return View(temp);
            //using (PhoneBookDbEntities obj = new PhoneBookDbEntities())
            //{
            //    return View(obj.People.ToList());
            //}
        }

        //GET: Test
        public ActionResult ContactIndex()
        {
            PhoneBookDbEntities db = new PhoneBookDbEntities();
            List<Person> list = db.People.ToList();
            List<Contact> contact_lst = db.Contacts.ToList();
            List<Contact> temp = new List<Contact>();
            //List<StudentViewModel> viewList = new List<StudentViewModel>();
            foreach (Person s in list)
            {
                if (s.AddedBy == User.Identity.GetUserId())
                {
                    foreach(Contact contact in contact_lst)
                    {
                        if (s.PersonId == contact.PersonId)
                        {
                            temp.Add(contact);
                        }
                    }
                    
                }
            }
            return View(temp);
        }
        public ActionResult Dashboard()
        {
            int counter = 0;
            PhoneBookDbEntities db = new PhoneBookDbEntities();
            List<Person> list = db.People.ToList();
            List<Person> temp = new List<Person>();
            List<Person> temp1 = new List<Person>();
            //List<StudentViewModel> viewList = new List<StudentViewModel>();
            foreach (Person s in list)
            {
                if (s.AddedBy == User.Identity.GetUserId())
                {
                    DateTime bd = Convert.ToDateTime(s.DateOfBirth);
                    counter = counter + 1;
                    if ( bd.Day == DateTime.Now.AddDays(1).Day || bd.Day == DateTime.Now.AddDays(10).Day || (bd.Day > DateTime.Now.AddDays(1).Day && bd.Day < DateTime.Now.AddDays(10).Day))
                    {
                        temp.Add(s);
                    }
                    if (s.UpdateOn == DateTime.Now.AddDays(-1) || s.UpdateOn == DateTime.Now.AddDays(-7) || (s.UpdateOn < DateTime.Now.AddDays(-1) && s.UpdateOn > DateTime.Now.AddDays(-7)))
                    {
                        temp1.Add(s);
                    }
                }
            }
            Session["No_of_Person_Added"] = counter;
            var model = new Class3()
            {
               BirthdayList = temp,
               UpdateList = temp1,
            };
           return View(model);
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
                p.LinkedInId = obj.LinkInId;
                p.UpdateOn = DateTime.Now;
                p.ImagePath = obj.ImagePath;
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
            int count = 0;
            using (PhoneBookDbEntities ent = new PhoneBookDbEntities())
            {
                List<Contact> list = ent.Contacts.ToList();
                foreach (Contact s in list)
                {
                    if (s.PersonId == id)
                    {
                        count = count + 1;
                    }
                }
            }
            Session["No_of_Contact_Added_Against_this_Person"] = count;
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
                using (PhoneBookDbEntities ent = new PhoneBookDbEntities())
                {
                    
                    Person person = ent.People.Where(x => x.PersonId == id).FirstOrDefault();
                    //Contact contact = ent.Contacts.Where(x => x.PersonId == id).FirstOrDefault();
                    //ent.Contacts.Remove(contact);
                    List<Contact> list = ent.Contacts.ToList();
                    foreach (Contact s in list)
                    {
                        if (s.PersonId == id)
                        {
                            ent.Contacts.Remove(s);
                            ent.SaveChanges();
                        }
                    }
                    
                    ent.People.Remove(person);
                    ent.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Test/Contact
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(int id, Class2 obj)
        {
            try
            {
                Contact c = new Contact();
                c.ContactNumber = obj.ContactNumber;
                c.Type = obj.Type;
                c.PersonId = id;
                using (PhoneBookDbEntities ent = new PhoneBookDbEntities())
                {
                    ent.Contacts.Add(c);
                    ent.SaveChanges();
                }
                return RedirectToAction("ContactIndex");
            }
            catch
            {
                return View();
            }
        }

        // GET: Test/Details/5
        public ActionResult ContactDetails(int id)
        {
            using (PhoneBookDbEntities obj = new PhoneBookDbEntities())
            {
                return View(obj.Contacts.Where(x => x.ContactId == id).FirstOrDefault());
            }

        }
        // GET: Test/Edit/5
        public ActionResult ContactEdit(int id)
        {
            using (PhoneBookDbEntities obj = new PhoneBookDbEntities())
            {
                return View(obj.Contacts.Where(x => x.ContactId == id).FirstOrDefault());
            }
        }

        // POST: Test/Edit/5
        [HttpPost]
        public ActionResult ContactEdit(int id, Contact obj)
        {
            try
            {
                // TODO: Add update logic here
                using (PhoneBookDbEntities ent = new PhoneBookDbEntities())
                {
                    ent.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    ent.SaveChanges();
                }
                return RedirectToAction("ContactIndex");
            }
            catch
            {
                return View();
            }
        }

        // GET: Test/Delete/5
        public ActionResult ContactDelete(int id)
        {
            using (PhoneBookDbEntities obj = new PhoneBookDbEntities())
            {
                return View(obj.Contacts.Where(x => x.ContactId == id).FirstOrDefault());
            }
        }

        // POST: Test/Delete/5
        [HttpPost]
        public ActionResult ContactDelete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (PhoneBookDbEntities ent = new PhoneBookDbEntities())
                {
                    Contact contact = ent.Contacts.Where(x => x.ContactId == id).FirstOrDefault();
                    ent.Contacts.Remove(contact);
                    ent.SaveChanges();
                }
                return RedirectToAction("ContactIndex");
            }
            catch
            {
                return View();
            }
        }
    }
}
