using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IssueTrackingSystem.Models
{ 
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }

        public virtual ICollection<Ticket> AssignedTickets { get; set; }
        
        public virtual ICollection<Ticket> CreatedTickets { get; set; }

        public virtual ICollection<Space> Spaces { get; set; } 

        public virtual ICollection<Comment> Comments { get; set; }

    }
}