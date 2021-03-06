﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace IssueTrackingSystem.Models
{
    public class ITSDatabase : DbContext
    {
        public ITSDatabase() : base("name=DefaultConnection")
        {

        }

        public DbSet<Space> spaces { get; set; }
        public DbSet<Ticket> tickets { get; set; }
        public DbSet<UserProfile> users { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasRequired(u => u.AssignedTo)
                .WithMany()
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}