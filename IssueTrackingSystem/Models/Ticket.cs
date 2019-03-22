using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTrackingSystem.Models
{
    public enum Status
    {
        Backlog,
        CurrentlyWorkingOn,
        Test,
        Completed
    }

    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        public UserProfile CreatedBy { get; set; }
        public UserProfile AssignedTo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ?Eta { get; set; }
        public DateTime CreatedDate { get; set; }
        public Space space { get; set; }
    }
}