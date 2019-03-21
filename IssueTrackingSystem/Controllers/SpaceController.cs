using IssueTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IssueTrackingSystem.Controllers
{
    public class SpaceController : Controller
    {
        private ITSDatabase _db = new ITSDatabase();
        // GET: Space
        public ActionResult Index()
        {
            //todo: load spaces available for user 
            var model = _db.spaces.ToList();
            return View(model);
        }

        // GET: Space/supportteam/Cardwall
        public ActionResult Cardwall(string spacename)
        {            
            if (spacename.Equals(null) || spacename.Equals(""))
            {
                return View("Error");
            }

            ViewBag.Space = spacename;
            var tickets = _db.tickets
                .Where(t => t.space.Name == spacename).ToList();
            return View(tickets);
        }

        // GET: Space/supportteam/Ticket/8
        public ActionResult Ticket(string spacename, int id)
        {
            var ticket = _db.tickets
                .Where(t => t.Id == id && t.space.Name == spacename).Single();
            return View(ticket);
        }

        // GET: Space/supportteam/AddTicket
        public ActionResult AddTicket(string spacename)
        {
            return View();
        }
        
        // POST: Space/supportteam/AddTicket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTicket(string spacename, Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.space = _db.spaces
                .Where(s => s.Name == spacename).First();

                //todo: authentication
                ticket.CreatedBy = Bootstrapper.AuthenticatedUser;

                try
                {
                    _db.tickets.Add(ticket);
                    _db.SaveChanges();
                    return RedirectToAction("Cardwall", "Space",  new { spacename = spacename});
                }
                catch
                {
                    return RedirectToAction("Error"); //todo send caution message
                }
            }
            return View(ticket);
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
