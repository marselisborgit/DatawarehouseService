using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IUserData
    {
        User GetUser(string userIdentifier, bool includeDeleted = false, string email = "");

        ICollection<Organization> GetAccessedOrganizations(int userId);
    }
}
