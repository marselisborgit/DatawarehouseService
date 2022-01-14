using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Country : SimpleType
    {
        public string IdentificationCode { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public DateTime? Deleted { get; set; }
    }
}
