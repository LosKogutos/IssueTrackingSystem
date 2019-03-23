using IssueTrackingSystem.Models;
using IssueTrackingSystem.Utils;
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
                .Where(t => t.Space.Name == spacename).ToList();
            return View(tickets);
        }

        // GET: Space/supportteam/Ticket/8
        public ActionResult Ticket(string spacename, int id)
        {
            var ticket = _db.tickets
                .Where(t => t.Id == id && t.Space.Name == spacename).Single();
            return View(Mapper.MapEntityToTicketViewModel(ticket));
        }

        // GET: Space/supportteam/AddTicket
        public ActionResult AddTicket(string spacename)
        {
            var tvm = new TicketViewModel
            {
                Users = _db.users.ToList() // todo: linq to return only users with access to this spacename
            };
            return View(tvm);
        }
        
        // POST: Space/supportteam/AddTicket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTicket(string spacename, TicketViewModel ticketViewModel)
        {
            if (ModelState.IsValid)
            {
                ticketViewModel.Space = _db.spaces
                    .Where(s => s.Name == spacename).First();
                ticketViewModel.AssignedTo = _db.users
                    .Where(u => u.Id == ticketViewModel.SelectedAssignedTo).SingleOrDefault();
                //todo: authentication
                ticketViewModel.CreatedBy = Bootstrapper.AuthenticatedUser;
                ticketViewModel.CreatedDate = DateTime.Now;
                var ticketEntity = Mapper.MapTicketViewModelToEntity(ticketViewModel);

                try
                {
                    _db.tickets.Add(ticketEntity);
                    _db.SaveChanges();
                    return RedirectToAction("Cardwall", "Space",  new { spacename = spacename});
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error"); //todo send caution message
                }
            }
            return View(ticketViewModel);
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
