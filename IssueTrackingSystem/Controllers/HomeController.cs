using IssueTrackingSystem.Models;
using IssueTrackingSystem.Services.Interfaces;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;


namespace IssueTrackingSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ISpaceService repo;

        public HomeController(ISpaceService repository)
        {
            this.repo = repository;
        }

        public ActionResult Index()
        {
            var userTickets = repo.GetActiveTicketsByAssignedToId(WebSecurity.CurrentUserId);

            var spaces = userTickets
                .Select(t => t.Space)
                .Distinct()
                .ToList();            

            foreach(var space in spaces)
            {
                space.Tickets = userTickets
                    .Where(t => t.Space.Id == space.Id)
                    .ToList();
            }

            return View(spaces);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}