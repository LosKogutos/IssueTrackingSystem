using IssueTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IssueTrackingSystem.Controllers
{
    public class HomeController : Controller
    {
        //todo: add DI/IC container for dbContext 
        private ITSDatabase _db = new ITSDatabase();

        public ActionResult Index()
        {
            //todo: get logged in user from session storage instead of bootstraping him
            Bootstrapper.authenticateRandomUser();

            var user = Bootstrapper.AuthenticatedUser;

            var userTickets =_db.tickets
                .Include("Space")
                .Where(t => t.AssignedTo.Id == user.Id && t.Status != Status.Completed)
                .ToList();

            var spaces = userTickets
                .Distinct()
                .Select(t => t.space).ToList();            

            foreach(Space space in spaces)
            {
                space.Tickets = userTickets
                    .Where(t => t.space.Id == space.Id)
                    .ToList();
            }

            return View(spaces);
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