using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageRoles.Models;

namespace ManageRoles.Repository
{
    public class SavedRolesMenuConcrete : ISavedMenuRoles
    {
        private readonly DatabaseContext _context;
        public SavedRolesMenuConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int SaveRole(SavedMenuRoles savedRoles)
        {
            try
            {
                int result = -1;
                if (savedRoles != null)
                {
                    savedRoles.Status = true;
                    _context.SavedMenuRoles.Add(savedRoles);
                    _context.SaveChanges();
                    result = savedRoles.SavedMenuRoleId;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckRoleAlreadyExists(SavedMenuRoles savedRoles)
        {
            try
            {
                var result = (from sroles in _context.SavedMenuRoles
                              where sroles.MenuId == savedRoles.MenuId && sroles.RoleId == savedRoles.RoleId
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
