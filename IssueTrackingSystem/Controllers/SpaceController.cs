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
                .Include("CreatedBy").Include("AssignedTo")
                .Where(t => t.Space.Name == spacename).ToList();
            return View(tickets);
        }

        // GET: Space/supportteam/Ticket/8
        public ActionResult Ticket(string spacename, int id)
        {
            var ticket = _db.tickets
                .Include("CreatedBy").Include("AssignedTo").Include("Comments")
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

            ViewBag.Spacename = spacename;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(CommentViewModel commentViewModel)
        {
            //save ticket to database 
            Comment comment = Mapper.MapCommentViewModelToEntity(commentViewModel);
            comment.CreatedBy = _db.users
                    .Where(u => u.Id == WebSecurity.CurrentUserId).SingleOrDefault();
            comment.Ticket = _db.tickets
                    .Where(t => t.Id == commentViewModel.TicketId && t.Space.Name == commentViewModel.Spacename).SingleOrDefault();

            _db.Comments.Add(comment);
            _db.SaveChanges();

            return RedirectToAction("Ticket", new { spacename = commentViewModel.Spacename, id = commentViewModel.TicketId });
        }

        [HttpPost]
        public ActionResult ChangeStatus(string id, string status)
        {
            int ticketId = Int32.Parse(id);
            var ticket = _db.tickets
                .Where(t => t.Id == ticketId).FirstOrDefault();
            switch (status)
            {
                case "Backlog":
                    ticket.Status = Status.Backlog;
                    break;
                case "CurrentlyWorkingOn":
                    ticket.Status = Status.CurrentlyWorkingOn;
                    break;
                case "Test":
                    ticket.Status = Status.Test;
                    break;
                case "Completed":
                    ticket.Status = Status.Completed;
                    break;
                default:
                    break;
            }
            _db.SaveChanges();
            return Json(new { id=ticket.Id, status=ticket.Status} );
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
