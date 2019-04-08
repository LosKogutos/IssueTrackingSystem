using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ?Eta { get; set; }
        public DateTime CreatedDate { get; set; }
        public Space Space { get; set; }

        public int CreatedBy_Id { get; set; }
        public int AssignedTo_Id { get; set; }

        [ForeignKey("CreatedBy_Id")]
        [InverseProperty("CreatedTickets")]
        public UserProfile CreatedBy { get; set; }

        [ForeignKey("AssignedTo_Id")]
        [InverseProperty("AssignedTickets")]
        public UserProfile AssignedTo { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}