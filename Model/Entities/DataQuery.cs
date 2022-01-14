using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    [Table("DataQuery", Schema = "dbo")]
    public class DataQuery : SimpleType
    {
        public string SQLQuery { get; set; }
        public int? OrganizationId { get; set; }
    }
}
