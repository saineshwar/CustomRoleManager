using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageRoles.Models;
using System.Linq.Dynamic;

namespace ManageRoles.Repository
{
    public class UserMasterConcrete : IUserMaster
    {
        private readonly DatabaseContext _context;
        private bool _disposed = false;

        public UserMasterConcrete(DatabaseContext context)
        {
            _context = context;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public List<Usermaster> GetAllUsers()
        {
            try
            {
                return _context.Usermasters.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Usermaster GetUserById(int? userId)
        {
            try
            {
                return _context.Usermasters.Find(userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long? AddUser(Usermaster usermaster)
        {
            try
            {
                long? result = -1;

                if (usermaster != null)
                {
                    usermaster.Status = true;
                    usermaster.CreateDate = DateTime.Now;
                    _context.Usermasters.Add(usermaster);
                    _context.SaveChanges();
                    result = usermaster.UserId;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long? UpdateUser(Usermaster usermaster)
        {
            try
            {

                long? result = -1;

                if (usermaster != null)
                {
                    usermaster.CreateDate = DateTime.Now;
                    _context.Entry(usermaster).State = EntityState.Modified;
                    _context.SaveChanges();
                    result = usermaster.UserId;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteUser(int? userId)
        {
            try
            {
                Usermaster usermaster = _context.Usermasters.Find(userId);
                if (usermaster != null) _context.Usermasters.Remove(usermaster);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckUsernameExists(string username)
        {
            try
            {
                var result = (from menu in _context.Usermasters
                              where menu.UserName == username
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Usermaster GetUserByUsername(string username)
        {
            try
            {
                var result = (from usermaster in _context.Usermasters
                              where usermaster.UserName == username
                              select usermaster).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<Usermaster> ShowAllUsers(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryableUsermaster = (from usermaster in _context.Usermasters
                                           select usermaster
                    );

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryableUsermaster = queryableUsermaster.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryableUsermaster = queryableUsermaster.Where(m => m.UserName.Contains(search) || m.UserName.Contains(search));
                }

                return queryableUsermaster;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DropdownUsermaster> GetAllUsersActiveList()
        {
            try
            {
                var listactiveUsers = (from usermaster in _context.Usermasters
                                       where usermaster.Status == true
                                       select new DropdownUsermaster
                                       {
                                           UserId = usermaster.UserId,
                                           UserName = usermaster.UserName
                                       }).ToList();

                listactiveUsers.Insert(0, new DropdownUsermaster()
                {
                    UserId = -1,
                    UserName = "---Select---"
                });

                return listactiveUsers;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
