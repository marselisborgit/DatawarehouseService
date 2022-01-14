using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BLL.Interfaces
{
    public interface IDatawarehouseManager
    {
        DataTable GetData(string queryName, DateTime? startDate, DateTime? endDate, int? countryId, ClientIdentity clientIdentity);

        ICollection<DataQuery> GetDataQueries(ClientIdentity clientIdentity);
   
    }
}
