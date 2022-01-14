using BLL.Interfaces;
using DAL.Interfaces;
using Microsoft.Extensions.Logging;
using Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BLL
{
    public class DatawarehouseManager : IDatawarehouseManager
    {

        private ILogger<DatawarehouseManager> _logger;
        private IDatawarehouseData _data;
        private IUserManager _userManager;

        public DatawarehouseManager(IDatawarehouseData data, ILogger<DatawarehouseManager> logger, IUserManager userManager)
        {
            _data = data;
            _logger = logger;
            _userManager = userManager;
        }

        public DataTable GetData(string queryName, DateTime? startDate, DateTime? endDate, int? countryId, ClientIdentity clientIdentity)
        {
            var user = _userManager.GetUserAndVerifyIdentity(ref clientIdentity);
            var organization = _userManager.GetRequestedOrganization(user, clientIdentity);

            //implemented to enable GlobalEvolution to fetch data with their own application.
            if (user.Email.ToLower() == "it@globalevolution.com")
            {
                //set the email as applicationid, as the sqlquries uses the email to fetch data. The application user have applicationid as useridentifier.
                user.Email = "ef09bc06-0e89-4919-82dc-8d5ed9644fdc";
            }

            return _data.GetData(queryName, user.Email, organization.Id, startDate ?? new DateTime(2000, 1, 1), endDate ?? DateTime.Now, countryId);
        }

        public ICollection<DataQuery> GetDataQueries(ClientIdentity clientIdentity)
        {
            var user = _userManager.GetUserAndVerifyIdentity(ref clientIdentity);
            var organization = _userManager.GetRequestedOrganization(user, clientIdentity);
            return _data.GetDataQueries(organization.Id, user.Id);
        }
    }
}
