using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class UserData : IUserData
    {
        private static ILogger<UserData> _logger;
        private DatabaseContext _context;
        public UserData(ILogger<UserData> logger, DatabaseContext context)
        {
            //_config = DataConverter._config;
            _logger = logger;
            _context = context;
        }


        public ICollection<Organization> GetAccessedOrganizations(int userId)
        {
            try
            {

                return _context.UserAccess
                    .Where(a => a.UserId == userId && !a.Deleted.HasValue)
                    .Include(x => x.Organization)
                    .Select(u => u.Organization)
                    .ToList();


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAccessedOrganizations(int) failed [UserId:{userId}]");
            }

            return null;
        }

        public User GetUser(string userIdentifier, bool includeDeleted = false, string email = "")
        {
            try
            {

                var user = _context.Users
                    .Where(u => u.UserIdentifier == userIdentifier && (includeDeleted || !u.Deleted.HasValue))
                    .Include(x => x.Organization)
                    .OrderByDescending(u => u.Deleted)
                    .FirstOrDefault();

                if (user != null)
                {
                    if (user.Email.Equals("Temporary@mail.com") && !string.IsNullOrEmpty(email))
                    {
                        var updatedUser = _context.Users
                        .Where(u => u.UserIdentifier == userIdentifier && (includeDeleted || !u.Deleted.HasValue))
                        .OrderByDescending(u => u.Deleted)
                        .FirstOrDefault();
                        updatedUser.Email = email;
                        updatedUser.CaseWorkerIdentifier = email;
                        //context.Entry(updatedUser).State = EntityState.Modified;
                        _context.SaveChanges();

                        user.Email = email;

                    }

                    //user.Department = user.DepartmentName;
                    return user;


                }

                _logger.LogWarning($"GetUser(string) failed - user not found or is deleted [Argument:'{userIdentifier}'].");


                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetUser(string) failed [Argument:'{userIdentifier}'].");
            }

            return null;
        }
    }
}
