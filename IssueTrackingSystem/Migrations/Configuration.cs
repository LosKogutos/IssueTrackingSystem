namespace IssueTrackingSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<IssueTrackingSystem.Models.ITSDatabase>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "IssueTrackingSystem.Models.ITSDatabase";
        }

        protected override void Seed(IssueTrackingSystem.Models.ITSDatabase context)
        {
            //var space1 = new Space
            //{
            //    Id = 1,
            //    Name = "Support Space"
            //};
            
            context.tickets.AddOrUpdate(t => t.Id, Bootstrapper.createReadyTicket(10).ToArray());
           //context.spaces.AddOrUpdate(s => s.Id, spaces.ToArray());
        }
    }
}
