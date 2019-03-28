using IssueTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace IssueTrackingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ITSDatabase _db = new ITSDatabase();
        // GET: Admin
        public ActionResult Index()
        {
            return View(_db.spaces.ToList());
        }

        // GET: Admin/ManageSpace/1
        public ActionResult ManageSpace(int id)
        {
            var users = _db.users
                .Where(u =>
                    u.Spaces.Any(s =>
                        s.Id == id))
                .ToList();

            ViewBag.Spacename = _db.spaces
                .Where(s => s.Id == id)
                .Select(s => s.Name).First();

            return View(users);
        }

        // POST: Admin/ManageSpace/1?spacename=test
        [HttpPost]
        public ActionResult RemoveAccessForUser(int id, string spacename)
        {
            int IdSpaceAffected=0;
            var userWithSpaces = _db.users
                .Include("Spaces")
                .Where(u => u.Id == id)
                .SingleOrDefault();

            if (userWithSpaces != null)
            {
                foreach(var space in userWithSpaces.Spaces
                    .Where(s => s.Name.Equals(spacename)).ToList())
                {
                    IdSpaceAffected = space.Id;
                    userWithSpaces.Spaces.Remove(space);
                }
            }

            try
            {
                _db.SaveChanges();
                if (IdSpaceAffected == 0)
                {
                    return View("Error");
                }
                else return RedirectToAction("ManageSpace", new { id = IdSpaceAffected });
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Admin/AddAccessForUser?spacename=test
        public ActionResult AddAccessForUser(string spacename)
        {
            var users = _db.users
                .Where(u =>
                    u.Spaces.All(s =>
                        s.Name != spacename))
                .ToList();

            ViewBag.Spacename = _db.spaces
                .Where(s => s.Name == spacename)
                .Select(s => s.Name).First();

            return View(users);
        }

        // POST: Admin/AddAccessForUser/1?spacename=test
        [HttpPost]
        public ActionResult AddAccessForUser(int id, string spacename)
        {
            var space = _db.spaces
                .Where(s => s.Name == spacename).First();
            var user = _db.users
                .Where(u => u.Id == id).First();

            space.Users.Add(user);
            user.Spaces.Add(space);
            try
            {
                _db.SaveChanges();
                return RedirectToAction("ManageSpace", new { id = space.Id });
            }
            catch
            {
                return View("Error");
            }            
        }

        public ActionResult CreateUser()
        {
            return PartialView("_CreateUser");
        }

        public ActionResult CreateSpace()
        {
            return PartialView("_CreateSpace");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new
                    {
                        model.Email,
                        model.Name,
                        model.Lastname
                    });
                    return RedirectToAction("Index", "Admin");
                }
                catch (MembershipCreateUserException e)
                {
                    ViewBag.ErrorMessage = "Unable to register user because of " + e.Message;
                    return View("_CreateUser", model);
                }
            }
            ViewBag.ErrorMessage = "Unable to Register User";
            return View("_CreateUser", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSpace(Space space)
        {
            try
            {
                _db.spaces.Add(space);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}