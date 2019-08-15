using IssueTrackingSystem.Models;
using IssueTrackingSystem.Services.Interfaces;
using IssueTrackingSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace IssueTrackingSystem.Services
{
    public class SpaceService : ISpaceService, IDisposable
    {
        private ITSDatabase _db = new ITSDatabase();

        public List<Space> GetSpacesForUser(int id)
        {
            return _db.spaces
                .Where(s =>
                    s.Users.Any(u =>
                        u.Id == id))
                .ToList();
        }

        public List<UserProfile> GetUsersWithAccessToSpace(string spacename)
        {
            return _db.users
                .Where(u =>
                    u.Spaces.Any(s =>
                        s.Name == spacename))
                .ToList();
        }

        public List<Ticket> GetTicketsBySpacename(string spacename)
        {
            return _db.tickets
                .Include("CreatedBy").Include("AssignedTo")
                .Where(t => t.Space.Name == spacename).ToList();
        }

        public List<Ticket> GetActiveTicketsByAssignedToId(int id)
        {
            return _db.tickets
                .Include("Space")
                .Where(t => t.AssignedTo.Id == id && t.Status != Status.Completed)
                .ToList();
        }

        public Ticket GetTicketById(int id)
        {
            return _db.tickets
                .Include("CreatedBy").Include("AssignedTo").Include("Comments")
                .Where(t => t.Id == id).Single();
        }

        public TicketViewModel GetTicketViewModel(string spacename, int id)
        {
            var ticket = GetTicketById(id);

            var ticketViewModel = Mapper.MapEntityToTicketViewModel(ticket);
            ticketViewModel.Users = GetUsersWithAccessToSpace(spacename);

            return ticketViewModel;
        }

        public bool UpdateTicket(TicketViewModel vm)
        {
            var createdBy = _db.users.Where(u => u.Username.Equals(vm.AssignedTo.Username)).Select(u => u.Id).FirstOrDefault();
            if (createdBy != 0)
            {
                var ticketEntity = _db.tickets.Where(t => t.Id == vm.Id).FirstOrDefault();
                ticketEntity.AssignedTo_Id = createdBy;
                ticketEntity.Eta = vm.Eta;
                ticketEntity.Description = vm.Description;
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddTicket(string spacename, TicketViewModel ticketViewModel)
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
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddComment(CommentViewModel commentViewModel)
        {
            Comment comment = Mapper.MapCommentViewModelToEntity(commentViewModel);
            comment.CreatedBy = _db.users
                    .Where(u => u.Id == WebSecurity.CurrentUserId).SingleOrDefault();
            comment.Ticket = _db.tickets
                    .Where(t => t.Id == commentViewModel.TicketId && t.Space.Name == commentViewModel.Spacename).SingleOrDefault();

            _db.Comments.Add(comment);
            try
            {
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTicketStatus(string id, string status)
        {
            var ticket = GetTicketById(Int32.Parse(id));
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
                    return false;
            }
            try
            {
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            if(_db != null)
            {
                _db.Dispose();
            }
        }
    }
}