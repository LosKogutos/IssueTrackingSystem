using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTrackingSystem.Models
{
    public class CommentViewModel
    {
        public string Text { get; set; }
        public string Spacename { get; set; }
        public int TicketId { get; set; }
    }
}