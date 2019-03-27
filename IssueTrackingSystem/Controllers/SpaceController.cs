using IssueTrackingSystem.Models;
using IssueTrackingSystem.Utils;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebMatrix.WebData;

namespace IssueTrackingSystem.Controllers
{
    [Authorize]
    public class SpaceController : Controller
    {
        private ITSDatabase _db = new ITSDatabase();
        // GET: Space
        public ActionResult Index()
        {
            var spaces = _db.spaces
                .Where(s => 
                    s.Users.Any(u =>
                        u.Id == WebSecurity.CurrentUserId))
                .ToList();
                
            return View(spaces);
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
                .Include("CreatedBy").Include("AssignedTo")
                .Where(t => t.Id == id && t.Space.Name == spacename).Single();
            var ticketViewModel = Mapper.MapEntityToTicketViewModel(ticket);

            ticketViewModel.Users = _db.users
                .Where(u =>
                    u.Spaces.Any(s =>
                        s.Name == spacename))
                .ToList();

            return View(ticketViewModel);
        }

        // POST: Space/supportteam/Ticket/8?fieldname=status
        [HttpPost]
        public ActionResult Ticket(string spacename, string fieldname, TicketViewModel ticketViewModel)
        {
            var ticket = _db.tickets
                .Where(t => t.Id == ticketViewModel.Id).First();
            switch (fieldname)
            {
                case "status":
                    ticket.Status = ticketViewModel.Status;
                    break;
                case "assignedto":
                    ticket.AssignedTo = _db.users
                        .Where(u => u.Id == ticketViewModel.SelectedAssignedTo).SingleOrDefault();
                    break;
                default:
                    return View("Error"); 
            }
            _db.SaveChanges();
            var dict = new RouteValueDictionary
            {
                { "spacename", spacename }
            };
            return RedirectToAction("Cardwall", dict);
        }

        // GET: Space/supportteam/AddTicket
        public ActionResult AddTicket(string spacename)
        {
            var tvm = new TicketViewModel
            {
                Users = _db.users
                    .Where(u =>
                        u.Spaces.Any(s =>
                            s.Name == spacename))
                    .ToList()
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
                ticketViewModel.CreatedBy = _db.users
                    .Where(u => u.Id == WebSecurity.CurrentUserId).SingleOrDefault();
                ticketViewModel.CreatedDate = DateTime.Now;

                var ticketEntity = Mapper.MapTicketViewModelToEntity(ticketViewModel);

                try
                {
                    _db.tickets.Add(ticketEntity);
                    _db.SaveChanges();
                    return RedirectToAction("Cardwall", "Space",  new { spacename = spacename});
                }
                catch 
                {
                    return RedirectToAction("Error"); 
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
