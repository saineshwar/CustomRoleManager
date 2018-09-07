using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageRoles.Models;

namespace ManageRoles.Repository
{
    public interface ISavedAssignedRoles
    {
        long? AddAssignedRoles(SavedAssignedRoles savedAssignedRoles);
        bool CheckAssignedRoles(long? userId);
        SavedAssignedRoles GetAssignedRolesbyUserId(long? userId);
    }
}
