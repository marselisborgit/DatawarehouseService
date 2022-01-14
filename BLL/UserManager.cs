using BLL.Interfaces;
using DAL.Interfaces;
using Microsoft.Extensions.Logging;
using Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class UserManager : IUserManager
    {
        private ILogger<UserManager> _logger;
        private IUserData _userData;

        public UserManager(IUserData userData, ILogger<UserManager> logger)
        {
            _logger = logger;
            _userData = userData;
        }

        public User GetUserAndVerifyIdentity(ref ClientIdentity clientIdentity)
        {
            var user = _userData.GetUser(clientIdentity?.UserIdentifier);

            if (user == null)
            {
               // _logger.Info($"Method failed. Unknown user. [UserId:{clientIdentity?.UserIdentifier}]");
                throw new UnauthorizedAccessException("Unknown user");
            }

            clientIdentity.OrganizationId = GetRequestedOrganization(user, clientIdentity).Id;

            return user;
        }

        public Organization GetRequestedOrganization(User user, ClientIdentity clientIdentity)
        {
            if (clientIdentity.OrganizationId.HasValue)
            {
                var organizations = GetOrganizations(user);

                var organization = organizations.FirstOrDefault(o => o.Id == (clientIdentity.OrganizationId ?? -1) && !o.Deleted.HasValue);

                if (organization != null)
                    return organization;

                throw new UnauthorizedAccessException("Organization");
            }

            if (user.Organization.Deleted.HasValue)
            {
                throw new UnauthorizedAccessException("Organization");
            }

            return user.Organization;
        }

        public IEnumerable<Organization> GetOrganizations(User user)
        {
            if (user == null)
                return null;

            var organizations = _userData.GetAccessedOrganizations(user.Id);

            if (organizations.All(o => o.Id != user.OrganizationId))
            {
                // Add user organization only if not already exist in the dataset
                organizations.Add(user.Organization);
            }

            return organizations;
        }
    }
}
