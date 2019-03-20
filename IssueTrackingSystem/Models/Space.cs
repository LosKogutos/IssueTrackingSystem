using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTrackingSystem.Models
{
    public class Space
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}