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
        private PhoneBookDbEntities entity = new PhoneBookDbEntities();
        private List<Person> person_temp_lst = new List<Person>();
        private List<Person> person_temp_lst1 = new List<Person>();
        private List<Contact> contact_temp_lst = new List<Contact>();

        // GET: Test
        public ActionResult Index()
        {        
            List<Person> person_list = entity.People.ToList();
            foreach (Person s in person_list)
            {
                if(s.AddedBy == User.Identity.GetUserId())
                {
                    person_temp_lst.Add(s);
                }
            }            
            return View(person_temp_lst);
        }

        //GET: Test
        public ActionResult ContactIndex()
        {
            List<Person> person_list = entity.People.ToList();
            List<Contact> contact_lst = entity.Contacts.ToList();
            foreach (Person s in person_list)
            {
                if (s.AddedBy == User.Identity.GetUserId())
                {
                    foreach(Contact contact in contact_lst)
                    {
                        if (s.PersonId == contact.PersonId)
                        {
                            contact_temp_lst.Add(contact);
                        }
                    }                  
                }
            }
            return View(contact_temp_lst);
        }

        //GET: Test
        public ActionResult Dashboard()
        {
            int counter = 0;
            List<Person> person_list = entity.People.ToList();
            foreach (Person s in person_list)
            {
                if (s.AddedBy == User.Identity.GetUserId())
                {
                    DateTime bd = Convert.ToDateTime(s.DateOfBirth);
                    DateTime ud = Convert.ToDateTime(s.UpdateOn);
                    counter = counter + 1;
                    if ( bd.Day == DateTime.Now.AddDays(1).Day || bd.Day == DateTime.Now.AddDays(10).Day || (bd.Day > DateTime.Now.AddDays(1).Day && bd.Day < DateTime.Now.AddDays(10).Day))
                    {
                        person_temp_lst.Add(s);
                    }
                    if (ud.Date == DateTime.Now.AddDays(-1).Date || ud.Date == DateTime.Now.AddDays(-7).Date || (ud.Date < DateTime.Now.AddDays(-1).Date && ud.Date > DateTime.Now.AddDays(-7).Date))
                    {
                        person_temp_lst1.Add(s);
                    }
                }
            }
            Session["No_of_Person_Added"] = counter;
            var model = new Class3()
            {
               BirthdayList = person_temp_lst,
               UpdateList = person_temp_lst1,
            };
           return View(model);
        }

        // GET: Test/Details/5
        public ActionResult Details(int id)
        {
            using (entity)
            {
                return View(entity.People.Where(x => x.PersonId == id).FirstOrDefault());
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
                using (entity)
                {
                    entity.People.Add(p);
                    entity.SaveChanges();
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
            using (entity)
            {
                return View(entity.People.Where(x => x.PersonId == id).FirstOrDefault());
            }
        }

        // POST: Test/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Person obj)
        {
            try
            {
                // TODO: Add update logic here
                using (entity)
                {
                    entity.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();
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
            List<Contact> contact_list = entity.Contacts.ToList();
            foreach (Contact s in contact_list)
            {
                if (s.PersonId == id)
                {
                    count = count + 1;
                }
            }
            Session["No_of_Contact_Added_Against_this_Person"] = count;
            using (entity)
            {
                return View(entity.People.Where(x => x.PersonId == id).FirstOrDefault());
            }
        }
        
        // POST: Test/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (entity)
                {
                    
                    Person person = entity.People.Where(x => x.PersonId == id).FirstOrDefault();
                    List<Contact> contact_list = entity.Contacts.ToList();
                    foreach (Contact s in contact_list)
                    {
                        if (s.PersonId == id)
                        {
                            entity.Contacts.Remove(s);
                            entity.SaveChanges();
                        }
                    }
                    
                    entity.People.Remove(person);
                    entity.SaveChanges();
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
                using (entity)
                {
                    entity.Contacts.Add(c);
                    entity.SaveChanges();
                }
                return RedirectToAction("ContactIndex");
            }
            catch
            {
                return View();
            }
        }

        // GET: Test/ContactDetails/5
        public ActionResult ContactDetails(int id)
        {
            using (entity)
            {
                return View(entity.Contacts.Where(x => x.ContactId == id).FirstOrDefault());
            }

        }
        // GET: Test/ContactEdit/5
        public ActionResult ContactEdit(int id)
        {
            using (entity)
            {
                return View(entity.Contacts.Where(x => x.ContactId == id).FirstOrDefault());
            }
        }

        // POST: Test/ContactEdit/5
        [HttpPost]
        public ActionResult ContactEdit(int id, Contact obj)
        {
            try
            {
                // TODO: Add update logic here
                using (entity)
                {
                    entity.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();
                }
                return RedirectToAction("ContactIndex");
            }
            catch
            {
                return View();
            }
        }

        // GET: Test/ContactDelete/5
        public ActionResult ContactDelete(int id)
        {
            using (entity)
            {
                return View(entity.Contacts.Where(x => x.ContactId == id).FirstOrDefault());
            }
        }

        // POST: Test/ContactDelete/5
        [HttpPost]
        public ActionResult ContactDelete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                using (entity)
                {
                    Contact contact = entity.Contacts.Where(x => x.ContactId == id).FirstOrDefault();
                    entity.Contacts.Remove(contact);
                    entity.SaveChanges();
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
