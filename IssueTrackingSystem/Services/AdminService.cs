using IssueTrackingSystem.Models;
using IssueTrackingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTrackingSystem.Services
{
    public class AdminService : IAdminService, IDisposable
    {
        private ITSDatabase _db = new ITSDatabase();

        public List<Space> GetAllSpaces()
        {
            return _db.spaces.ToList();
        }

        public List<UserProfile> GetAllUsersForSpace(int spaceId)
        {
            return _db.users
                .Where(u =>
                    u.Spaces.Any(s =>
                        s.Id == spaceId))
                .ToList();
        }

        public Space GetSpaceById(int spaceId)
        {
            return _db.spaces
                .Where(s => s.Id == spaceId).First();
        }

        public Space GetSpaceByName(string spacename)
        {
            return _db.spaces
                .Where(s => s.Name == spacename).First();
        }

        public UserProfile GetUserById(int userId)
        {
            return _db.users
                .Include("Spaces")
                .Where(u => u.Id == userId)
                .SingleOrDefault();
        }

        public bool RemoveUserFromSpace(UserProfile user, Space space)
        {
            try
            {
                user.Spaces.Remove(space);
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddUserToSpace(UserProfile user, Space space)
        {
            try
            {
                space.Users.Add(user);
                user.Spaces.Add(space);
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<UserProfile> GetAllUsersWithoutAccessToSpace(string spacename)
        {
            return _db.users
                .Where(u =>
                    u.Spaces.All(s =>
                        s.Name != spacename))
                .ToList();
        }

        public bool CreateSpace(Space space)
        {
            try
            {
                _db.spaces.Add(space);
                _db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }
    }
}