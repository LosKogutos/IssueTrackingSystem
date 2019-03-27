using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTrackingSystem.Models
{
    public class Bootstrapper
    {
        private static ITSDatabase _db = new ITSDatabase();
        private static readonly Random getRandom = new Random();
        //private static UserProfile _authUser;
        //public static UserProfile AuthenticatedUser
        //{
        //    get
        //    {
        //        if (_authUser != null)
        //            return _authUser;

        //        else authenticateRandomUser();
        //        return _authUser;
        //    }
        //    set {
        //        _authUser = value;
        //    }
        //}

        //public static void authenticateRandomUser()
        //{
        //    int id = getRandom.Next(10);
        //    AuthenticatedUser = _db.users
        //        .Where(u => u.Id == id)
        //        .First();
        //}

        //public static IEnumerable<Space> createSpaces(int amount)
        //{
        //    List<Space> spaceList = new List<Space>();
        //    for(int i = 0; i < amount; i++)
        //    {
        //        spaceList.Add(new Space
        //        {
        //            Id = i+1,
        //            Name = "Space" + (i+1)
        //        });
        //    }
        //    return spaceList;
        //}

        //public static IEnumerable<Ticket> createTickets(int amount)
        //{
        //    var ticketList = new List<Ticket>();
        //    for (int i = 0; i < amount; i++)
        //    {
        //        ticketList.Add(new Ticket
        //        {
        //            Id = i + 1,
        //            Title = "title" + (i + 1),
        //            Description = "description" + (i + 1),
        //            Eta = DateTime.Now,
        //            Status = (Status)getRandom.Next(3),
        //            CreatedDate = DateTime.Now
        //        });
        //    }
        //    return ticketList;
        //}

        //public static IEnumerable<UserProfile> createUsers(int amount)
        //{
        //    var userList = new List<UserProfile>();
        //    for (int i = 0; i < amount; i++)
        //    {
        //        userList.Add(new UserProfile
        //        {
        //            Id = i + 1,
        //            Email = "emailAddress" + (i + 1),
        //            Name = "name" + (i + 1),
        //            Lastname = "lastname" + (i + 1)
        //        });
        //    }
        //    return userList;
        //}

        public static IEnumerable<Ticket> createReadyTicket(int amount)
        {
            var tickets = new List<Ticket>();
            for(int i = 0; i < amount; i++)
            {
                tickets.Add(new Ticket
                {
                    Status = (Status)getRandom.Next(3),
                    Title = "title" + (i + 1),
                    Description = "description" + (i + 1),
                    CreatedDate = DateTime.Now,
                    Eta = DateTime.Now,
                    Space = new Space
                    {
                        Name = "Space" + (i + 1)
                    },
                    AssignedTo = new UserProfile
                    {
                        Username = "mkogut" + (i+1),
                        Email = "email" + (i + 1),
                        Name = "name" + (i + 1),
                        Lastname = "lastname" + (i + 1)
                    },
                    CreatedBy = new UserProfile
                    {
                        Username = "mkogut" + (i + 1),
                        Email = "email" + (i + 2),
                        Name = "name" + (i + 2),
                        Lastname = "lastname" + (i + 2)
                    }
                });
            }
            return tickets;
        }

        //public static IEnumerable<Space> createSpacesWithTickets(int amount,
        //    List<Ticket> tickets)
        //{
        //    var spaceList = new List<Space>();
        //    for (int i = 0; i < amount; i++)
        //    {
        //        spaceList.Add(new Space
        //        {
        //            Id = i + 1,
        //            Name = "Space" + (i + 1),
        //            Tickets = tickets.Skip(i).Take(2).ToList()
        //        });
        //    }
        //    return spaceList;
        //}

        
    }
}