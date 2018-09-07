using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using Dapper;
using ManageRoles.Models;

namespace ManageRoles.Repository
{
    public class MenuConcrete : IMenu
    {
        private readonly DatabaseContext _context;
        private bool _disposed = false;

        public MenuConcrete(DatabaseContext context)
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

        public List<MenuMaster> GetAllMenu()
        {
            try
            {
                return _context.MenuMaster.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MenuMaster> GetAllActiveMenu()
        {
            try
            {
                var listofActiveMenu = (from menu in _context.MenuMaster
                                        where menu.Status == true
                                        select menu).ToList();

                listofActiveMenu.Insert(0, new MenuMaster()
                {
                    MenuId = -1,
                    MenuName = "---Select---"
                });

                return listofActiveMenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MenuMaster GetMenuById(int? menuId)
        {
            try
            {
                return _context.MenuMaster.Find(menuId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int? AddMenu(MenuMaster menuMaster)
        {
            try
            {
                int? result = -1;

                if (menuMaster != null)
                {
                    menuMaster.CreateDate = DateTime.Now;
                    _context.MenuMaster.Add(menuMaster);
                    _context.SaveChanges();
                    result = menuMaster.MenuId;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int? UpdateMenu(MenuMaster menuMaster)
        {
            try
            {
                int? result = -1;

                if (menuMaster != null)
                {
                    menuMaster.CreateDate = DateTime.Now;
                    _context.Entry(menuMaster).State = EntityState.Modified;
                    _context.SaveChanges();
                    result = menuMaster.MenuId;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteMenu(int? menuId)
        {
            try
            {
                MenuMaster menuMaster = _context.MenuMaster.Find(menuId);
                if (menuMaster != null) _context.MenuMaster.Remove(menuMaster);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckMenuNameExists(string menuName)
        {
            try
            {
                var result = (from menu in _context.MenuMaster
                              where menu.MenuName == menuName
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<MenuMaster> ShowAllMenus(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryableMenuMaster = (from register in _context.MenuMaster
                                           select register
                    );

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryableMenuMaster = queryableMenuMaster.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryableMenuMaster = queryableMenuMaster.Where(m => m.MenuName.Contains(search) || m.MenuName.Contains(search));
                }

                return queryableMenuMaster;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MenuMaster> GetAllActiveMenu(long roleId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@RoleID", roleId);
                    return con.Query<MenuMaster>("Usp_GetMenusByRoleID", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MenuMaster> GetAllActiveMenuSuperAdmin()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    return con.Query<MenuMaster>("Usp_GetMenusByRoleID_SuperAdmin", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
