using DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public class DatawarehouseData : IDatawarehouseData
    {

        private static ILogger<DatawarehouseData> _logger;

        private readonly DatabaseContext _context;

        public DatawarehouseData(ILogger<DatawarehouseData> logger, DatabaseContext context)
        {
            _context = context;
            _logger = logger;
        }

        public DataTable GetData(string queryName, string email, int organizationId, DateTime startDate, DateTime endDate, int? countryId)
        {

            var dt = new DataTable();
            var conn = _context.Database.GetDbConnection();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var lort = countryId.GetValueOrDefault();
                    cmd.CommandText = "GetData";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("QueryName", queryName));
                    cmd.Parameters.Add(new SqlParameter("Email", email));
                    cmd.Parameters.Add(new SqlParameter("OrganizationId", organizationId));
                    cmd.Parameters.Add(new SqlParameter("StartDate", startDate));
                    cmd.Parameters.Add(new SqlParameter("EndDate", endDate));
                    cmd.Parameters.Add(new SqlParameter("CountryId", countryId.GetValueOrDefault() == 0 ? -1 : (int)countryId));
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }


            catch (Exception e)
            {
                // error handling
                throw;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed) conn.Close();
            }
            return dt;
        }


        public ICollection<DataQuery> GetDataQueries(int organizationId, int userId)
        {
            try
            {
                return _context.DataQueries
                    .Where(d => d.OrganizationId == organizationId)
                    .ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetDataQueries failed. Message: {ex.Message}");
                throw ex;
            }
        }
    }
}
