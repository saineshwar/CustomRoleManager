using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageRoles.Models;
using System.Linq.Dynamic;
using ManageRoles.ViewModels;

namespace ManageRoles.Repository
{
    public class SubMenuConcrete : ISubMenu
    {
        private readonly DatabaseContext _context;
        private bool _disposed = false;

        public SubMenuConcrete(DatabaseContext context)
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

        public int? AddSubMenu(SubMenuMaster subMenuMaster)
        {
            try
            {
                int? result = -1;

                if (subMenuMaster != null)
                {
                    subMenuMaster.CreateDate = DateTime.Now;
                    _context.SubMenuMasters.Add(subMenuMaster);
                    _context.SaveChanges();
                    result = subMenuMaster.MenuId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteSubMenu(int? subMenuId)
        {
            try
            {
                SubMenuMaster subMenuMaster = _context.SubMenuMasters.Find(subMenuId);
                if (subMenuMaster != null) _context.SubMenuMasters.Remove(subMenuMaster);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<SubMenuMaster> GetAllSubMenu()
        {
            try
            {
                return _context.SubMenuMasters.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public SubMenuMaster GetSubMenuById(int? subMenuId)
        {
            try
            {
                return _context.SubMenuMasters.Find(subMenuId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? UpdateSubMenu(SubMenuMaster subMenuMaster)
        {
            try
            {
                int? result = -1;

                if (subMenuMaster != null)
                {
                    subMenuMaster.CreateDate = DateTime.Now;
                    _context.Entry(subMenuMaster).State = EntityState.Modified;
                    _context.SaveChanges();
                    result = subMenuMaster.SubMenuId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckSubMenuNameExists(string subMenuName, int menuId)
        {
            try
            {
                var result = (from submenu in _context.SubMenuMasters
                              where submenu.SubMenuName == subMenuName && submenu.MenuId == menuId
                              select submenu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<SubMenuMasterViewModel> ShowAllSubMenus(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesSubMenuMasters = (from submenu in _context.SubMenuMasters
                                                join menuMaster in _context.MenuMaster on submenu.MenuId equals menuMaster.MenuId
                                                select new SubMenuMasterViewModel
                                                {
                                                    SubMenuName = submenu.SubMenuName,
                                                    MenuName = menuMaster.MenuName,
                                                    ActionMethod = submenu.ActionMethod,
                                                    ControllerName = submenu.ControllerName,
                                                    Status = submenu.Status,
                                                    SubMenuId = submenu.SubMenuId
                                                });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesSubMenuMasters = queryablesSubMenuMasters.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryablesSubMenuMasters = queryablesSubMenuMasters.Where(m => m.SubMenuName.Contains(search) || m.SubMenuName.Contains(search));
                }

                return queryablesSubMenuMasters;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SubMenuMaster> GetAllActiveSubMenu(int menuid)
        {
            var listofActiveMenu = (from submenu in _context.SubMenuMasters
                                    where submenu.Status == true && submenu.MenuId == menuid
                                    select submenu).ToList();

            listofActiveMenu.Insert(0, new SubMenuMaster()
            {
                SubMenuId = -1,
                SubMenuName = "---Select---"
            });

            return listofActiveMenu;
        }

        public List<SubMenuMaster> GetAllActiveSubMenuByMenuId(int menuid)
        {
            var listofActiveMenu = (from submenu in _context.SubMenuMasters
                where submenu.Status == true && submenu.MenuId == menuid
                select submenu).ToList();
            return listofActiveMenu;
        }


    }
}
