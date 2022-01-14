using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ClientIdentity
    {
        public ClientIdentity()
        {
        }

        public ClientIdentity(string userIdentifier, int? organizationId, bool fromKombit, string cvrIdentifier, bool hasPrivilege, string email)
        {
            UserIdentifier = userIdentifier;
            OrganizationId = organizationId;
            FromKombit = fromKombit;
            HasPrivilege = hasPrivilege;
            CvrIdentifier = cvrIdentifier;
            Email = email;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// This can be the email or authentication identifier.
        /// </value>
        public string UserIdentifier { get; set; }

        public int? OrganizationId { get; set; }

        public bool FromKombit { get; set; }

        public bool HasPrivilege { get; set; }

        public string CvrIdentifier { get; set; }
        public string Email { get; set; }
    }
}
