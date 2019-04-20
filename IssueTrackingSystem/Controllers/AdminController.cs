using IssueTrackingSystem.Models;
using IssueTrackingSystem.Services.Interfaces;
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
        private readonly IAdminService repo;

        public AdminController(IAdminService repository)
        {
            this.repo = repository;
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(repo.GetAllSpaces());
        }

        // GET: Admin/ManageSpace/1
        public ActionResult ManageSpace(int id)
        {
            var users = repo.GetAllUsersForSpace(id);

            ViewBag.Spacename = repo.GetSpaceById(id).Name;

            return View(users);
        }

        // POST: Admin/ManageSpace/1?spacename=test
        [HttpPost]
        public ActionResult RemoveAccessForUser(int id, string spacename)
        {
            var user = repo.GetUserById(id);
            var space = repo.GetSpaceByName(spacename);

            bool isRemovedSuccessfully = repo.RemoveUserFromSpace(user, space);

            if (isRemovedSuccessfully)
            {
                return RedirectToAction("ManageSpace", new { id = space.Id });
            }
            else
            {
                return View("Error");
            }
        }

        // GET: Admin/AddAccessForUser?spacename=test
        public ActionResult AddAccessForUser(string spacename)
        {
            var users = repo.GetAllUsersWithoutAccessToSpace(spacename);

            ViewBag.Spacename = repo.GetSpaceByName(spacename).Name;

            return View(users);
        }

        // POST: Admin/AddAccessForUser/1?spacename=test
        [HttpPost]
        public ActionResult AddAccessForUser(int id, string spacename)
        {
            var space = repo.GetSpaceByName(spacename);
            var user = repo.GetUserById(id);

            var isAddedSuccessfully = repo.AddUserToSpace(user, space);
            if (isAddedSuccessfully)
            {
                return RedirectToAction("ManageSpace", new { id = space.Id });
            }
            else
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
            var isCreatedSuccessfully = repo.CreateSpace(space);
            if (isCreatedSuccessfully)
            { 
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}