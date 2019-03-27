using IssueTrackingSystem.Models;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;


namespace IssueTrackingSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //todo: add DI/IC container for dbContext 
        private ITSDatabase _db = new ITSDatabase();

        public ActionResult Index()
        {
            var userTickets =_db.tickets
                .Include("Space")
                .Where(t => t.AssignedTo.Id == WebSecurity.CurrentUserId && t.Status != Status.Completed)
                .ToList();

            var spaces = userTickets
                .Distinct()
                .Select(t => t.Space).ToList();            

            foreach(Space space in spaces)
            {
                space.Tickets = userTickets
                    .Where(t => t.Space.Id == space.Id)
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