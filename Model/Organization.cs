using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Organization : SimpleType
    {
        public int BusinessId { get; set; }
       public int? GroupId { get; set; }

        public int? AuthorityId { get; set; }

        public string Thumbprint { get; set; }

        public string CVR { get; set; }

        public DateTime? Deleted { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
