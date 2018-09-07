using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageRoles.Models;

namespace ManageRoles.Repository
{
    public class SavedAssignedRolesConcrete : ISavedAssignedRoles
    {
        private readonly DatabaseContext _context;
        public SavedAssignedRolesConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public long? AddAssignedRoles(SavedAssignedRoles savedAssignedRoles)
        {
            try
            {
                int result = -1;
                if (savedAssignedRoles != null)
                {
                    _context.SavedAssignedRoles.Add(savedAssignedRoles);
                    _context.SaveChanges();
                    result = savedAssignedRoles.AssignedRoleId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckAssignedRoles(long? userId)
        {
            try
            {
                var checkIsRoleAlreadyAssigned = (from sar in _context.SavedAssignedRoles
                                                  where sar.UserId == userId
                                                  select sar).Any();

                return checkIsRoleAlreadyAssigned;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public SavedAssignedRoles GetAssignedRolesbyUserId(long? userId)
        {
            try
            {
                var checkIsRoleAlreadyAssigned = (from sar in _context.SavedAssignedRoles
                    where sar.UserId == userId
                    select sar).FirstOrDefault();

                return checkIsRoleAlreadyAssigned;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
