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

        // GET: Space/Cardwall/supportteam
        public ActionResult Cardwall(string spaceName)
        {            
            ViewBag.Space = spaceName;
            var tickets = _db.tickets
                .Where(t => t.space.Name == spaceName).ToList();
            return View(tickets);
        }

        // GET: Space/Ticket/8
        public ActionResult Ticket(string ticketId)
        {
            //todo: load ticket details by it's Id 
            var ticket = Bootstrapper.createTickets(1).Single();
            return View(ticket);
        }

        // GET: Space/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Space/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Space/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Space/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Space/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Space/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
