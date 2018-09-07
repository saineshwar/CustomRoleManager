using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageRoles.Models;

namespace ManageRoles.Repository
{
    public class SavedRolesSubMenuConcrete : ISavedSubMenuRoles
    {
        private readonly DatabaseContext _context;
        public SavedRolesSubMenuConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int SaveRole(SavedSubMenuRoles savedRoles)
        {
            try
            {
                int result = -1;
                if (savedRoles != null)
                {
                    savedRoles.Status = true;
                    _context.SavedSubMenuRoles.Add(savedRoles);
                    _context.SaveChanges();
                    result = savedRoles.SavedSubMenuRoleId;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckRoleAlreadyExists(SavedSubMenuRoles savedSubMenuRoles)
        {
            try
            {
                var result = (from sroles in _context.SavedSubMenuRoles
                              where sroles.MenuId == savedSubMenuRoles.MenuId && sroles.SubMenuId == savedSubMenuRoles.SubMenuId && sroles.RoleId == savedSubMenuRoles.RoleId
                              select sroles).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
