using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageRoles.Models;

namespace ManageRoles.Repository
{
    public interface ISavedSubMenuRoles
    {
        int SaveRole(SavedSubMenuRoles savedRoles);
        bool CheckRoleAlreadyExists(SavedSubMenuRoles savedSubMenuRoles);

    }
}
