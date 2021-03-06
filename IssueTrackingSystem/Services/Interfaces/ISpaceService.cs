﻿using IssueTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTrackingSystem.Services.Interfaces
{
    public interface ISpaceService
    {
        List<Space> GetSpacesForUser(int id);
        List<Ticket> GetTicketsBySpacename(string spacename);
        Ticket GetTicketById(int id);
        List<UserProfile> GetUsersWithAccessToSpace(string spacename);
        TicketViewModel GetTicketViewModel(string spacename, int id);
        bool UpdateTicket(TicketViewModel vm);
        bool AddTicket(string spacename, TicketViewModel ticketViewModel);
        bool UpdateTicketStatus(string id, string status);
        bool AddComment(CommentViewModel commentViewModel);
        List<Ticket> GetActiveTicketsByAssignedToId(int id);
    }
}