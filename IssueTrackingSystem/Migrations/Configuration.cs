namespace IssueTrackingSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using Models;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<ITSDatabase>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "IssueTrackingSystem.Models.ITSDatabase";
        }

        protected override void Seed(ITSDatabase context)
        {

            SeedMembership();
            //context.tickets.AddOrUpdate(t => t.Id, Bootstrapper.createReadyTicket(10).ToArray());
        }

        private void SeedMembership()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                "UserProfile", "Id", "Username", autoCreateTables: true);

            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.RoleExists("Admin"))
            {
                roles.CreateRole("Admin");
            }
            if (membership.GetUser("mkogut", false) == null)
            {
                membership.CreateUserAndAccount("mkogut", "qwerty");
            }
            if (!roles.GetRolesForUser("mkogut").Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] { "mkogut" }, new[] { "admin" });
            }
        }
    }
}
