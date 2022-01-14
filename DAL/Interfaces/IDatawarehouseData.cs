using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DAL.Interfaces
{
    public interface IDatawarehouseData
    {
        DataTable GetData(string queryName, string email, int organizationId, DateTime startDate, DateTime endDate, int? countryId);

        ICollection<DataQuery> GetDataQueries(int organizationId, int userId);
    }
}
