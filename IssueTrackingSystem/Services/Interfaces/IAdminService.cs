using IssueTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTrackingSystem.Services.Interfaces
{
    public interface IAdminService
    {
        List<Space> GetAllSpaces();
        Space GetSpaceById(int spaceId);
        Space GetSpaceByName(string spacename);
        List<UserProfile> GetAllUsersForSpace(int spaceId);
        UserProfile GetUserById(int userId);
        bool RemoveUserFromSpace(UserProfile user, Space space);
        List<UserProfile> GetAllUsersWithoutAccessToSpace(string spacename);
        bool AddUserToSpace(UserProfile user, Space space);
        bool CreateSpace(Space space);

    }
}
