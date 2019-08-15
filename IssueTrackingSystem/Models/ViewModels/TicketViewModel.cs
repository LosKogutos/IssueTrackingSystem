using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTrackingSystem.Models
{
    public enum StatusTicketViewModel
    {
        Backlog,
        CurrentlyWorkingOn,
        Test,
        Completed
    }

    public class TicketViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        public UserProfile CreatedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Eta { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime CreatedDate { get; set; }
        public Space Space { get; set; }

        public UserProfile AssignedTo { get; set; }
        public int SelectedAssignedTo { get; set; }
        public List<UserProfile> Users { get; set; }
        public List<Comment> Comments { get; set; }

    }
}