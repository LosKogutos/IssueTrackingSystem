using IssueTrackingSystem.Models;
using IssueTrackingSystem.Services.Interfaces;
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
        private readonly ISpaceService repo; 

        public SpaceController(ISpaceService repository)
        {
            this.repo = repository;
        }

        // GET: Space
        public ActionResult Index()
        {                
            return View(repo.GetSpacesForUser(WebSecurity.CurrentUserId));
        }

        // GET: Space/supportteam/Cardwall
        public ActionResult Cardwall(string spacename)
        {            
            if (spacename.Equals(null) || spacename.Equals(""))
            {
                return View("Error");
            }

            ViewBag.Space = spacename;

            return View(repo.GetTicketsBySpacename(spacename));
        }

        // GET: Space/supportteam/Ticket/8
        public ActionResult Ticket(string spacename, int id)
        {
            return View(repo.GetTicketViewModel(spacename,id));
        }

        // POST: Space/supportteam/Ticket/8?fieldname=status
        [HttpPost]
        public ActionResult Ticket(string spacename, string fieldname, TicketViewModel ticketViewModel)
        {
            bool updatedSuccessfully = repo.UpdateTicketField(fieldname, ticketViewModel);
            if (!updatedSuccessfully)
            {
                return View("Error");
            }

            var dict = new RouteValueDictionary
            {
                { "spacename", spacename }
            };
            return RedirectToAction("Cardwall", dict);
        }

        // GET: Space/supportteam/AddTicket
        public ActionResult AddTicket(string spacename)
        {
            ViewBag.Spacename = spacename;

            return View(new TicketViewModel
            {
                Users = repo.GetUsersWithAccessToSpace(spacename)
            });
        }
        
        // POST: Space/supportteam/AddTicket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTicket(string spacename, TicketViewModel ticketViewModel)
        {
            if (ModelState.IsValid)
            {
                var savedSuccessfully = repo.AddTicket(spacename, ticketViewModel);
                if (savedSuccessfully)
                {
                    return RedirectToAction("Cardwall", "Space", new { spacename = spacename });
                }
                else
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
            if (ModelState.IsValid)
            {
                var savedSuccessfully = repo.AddComment(commentViewModel);
                if (savedSuccessfully)
                {
                    return RedirectToAction("Ticket", new { spacename = commentViewModel.Spacename, id = commentViewModel.TicketId });
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            return View(commentViewModel);
        }

        [HttpPost]
        public ActionResult ChangeStatus(string id, string status)
        {
            bool updatedSuccessfully = repo.UpdateTicketStatus(id, status);
            return Json(new { isStatusUpdated = updatedSuccessfully } );
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
