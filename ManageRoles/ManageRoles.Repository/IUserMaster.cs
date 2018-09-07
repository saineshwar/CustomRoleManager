using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageRoles.Models;

namespace ManageRoles.Repository
{
    public interface IUserMaster
    {
        List<Usermaster> GetAllUsers();
        Usermaster GetUserById(int? userId);
        long? AddUser(Usermaster usermaster);
        long? UpdateUser(Usermaster usermaster);
        void DeleteUser(int? userId);
        bool CheckUsernameExists(string username);
        Usermaster GetUserByUsername(string username);
        IQueryable<Usermaster> ShowAllUsers(string sortColumn, string sortColumnDir, string search);
        List<DropdownUsermaster> GetAllUsersActiveList();
    }
}
