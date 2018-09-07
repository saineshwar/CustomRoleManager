using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageRoles.Models;

namespace ManageRoles.Repository
{
    public interface IPassword
    {
        long? SavePassword(PasswordMaster passwordMaster);
        string GetPasswordbyUserId(long userId);
    }
}
