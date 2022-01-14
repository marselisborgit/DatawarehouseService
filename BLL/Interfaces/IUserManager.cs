using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface IUserManager
    {
        User GetUserAndVerifyIdentity(ref ClientIdentity clientIdentity);
        Organization GetRequestedOrganization(User user, ClientIdentity clientIdentity);
    }
}
