using IssueTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTrackingSystem.Utils
{
    public static class Mapper
    {
        public static Ticket MapTicketViewModelToEntity (TicketViewModel viewmodel)
        {
            return new Ticket
            {
                Id = viewmodel.Id,
                Title = viewmodel.Title,
                Description = viewmodel.Description,
                Space = viewmodel.Space,
                AssignedTo = viewmodel.AssignedTo,
                CreatedBy = viewmodel.CreatedBy,
                CreatedDate = viewmodel.CreatedDate,
                Eta = viewmodel.Eta,
                Status = viewmodel.Status
            };
        }

        public static TicketViewModel MapEntityToTicketViewModel (Ticket entity)
        {
            return new TicketViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Space = entity.Space,
                AssignedTo = entity.AssignedTo,
                CreatedBy = entity.CreatedBy,
                CreatedDate = entity.CreatedDate,
                Eta = (DateTime)entity.Eta,
                Status = entity.Status,
                Comments = entity.Comments.ToList()
            };
        }

        internal static Comment MapCommentViewModelToEntity(CommentViewModel commentViewModel)
        {
            return new Comment
            {
                Text = commentViewModel.Text,
                CreatedDate = DateTime.Now
            };
        }
    }
}