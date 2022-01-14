using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Payload
    {
        /// <summary>
        /// The subject
        /// </summary>
        [JsonProperty("sub")]
        public string Sub { get; set; }

        /// <summary>
        /// The email
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Indicates whether the email property have been verified
        /// </summary>
        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("cvr_identifier")]
        public string CvrIdentifier { get; set; }

        [JsonProperty("user_identifier")]
        public string UserIdentifier { get; set; }

        [JsonProperty("has_privilege")]
        public bool HasPrivilege { get; set; }

        [JsonProperty("from_kombit")]
        public bool FromKombit { get; set; }
    }
}
