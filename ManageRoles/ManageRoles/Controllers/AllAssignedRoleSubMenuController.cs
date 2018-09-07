using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Models;
using System.Data.Entity;
using ManageRoles.Filters;
using ManageRoles.Repository;
using ManageRoles.ViewModels;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    public class AllAssignedRoleSubMenuController : Controller
    {
        private readonly IRole _role;
        // GET: AllMenu

        public AllAssignedRoleSubMenuController(IRole role)
        {
            _role = role;
        }
        public ActionResult RolesView()
        {
            return View();
        }

        [HttpPost]//Gets the todo Lists.  
        public JsonResult GetRoles(int? roleId, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {

            try
            {
                var rolesCount = GetRolesCount();

                var roles = GetRolesList(roleId, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = roles, TotalRecordCount = rolesCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetRolesCount()
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    return db.SavedMenuRoles.Count();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ViewSubMenuRoleModel> GetRolesList(int? roleId, int startIndex, int count, string sorting)
        {
            // Instance of DatabaseContext
            try
            {
                using (var db = new DatabaseContext())
                {
                    var data = from savedSubMenuRoles in db.SavedSubMenuRoles
                               join roleMaster in db.RoleMasters on savedSubMenuRoles.RoleId equals roleMaster.RoleId 
                               join menuMaster in db.MenuMaster on savedSubMenuRoles.MenuId equals menuMaster.MenuId
                               join subMenuMaster in db.SubMenuMasters on savedSubMenuRoles.SubMenuId equals subMenuMaster.SubMenuId
                    
                               select new ViewSubMenuRoleModel()
                               {
                                   SaveId = savedSubMenuRoles.SavedSubMenuRoleId,
                                   RoleName = roleMaster.RoleName,
                                   MenuName = menuMaster.MenuName,
                                   RoleId = savedSubMenuRoles.RoleId,
                                   SubMenuName =  subMenuMaster.SubMenuName,
                                   Status = savedSubMenuRoles.Status
                               };

                   
                    IEnumerable<ViewSubMenuRoleModel> query = data.ToList();

                    //Search
                    if (roleId != null)
                    {
                        query = query.Where(p => p.RoleId == roleId);
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("MenuId ASC"))
                    {
                        query = query.OrderBy(p => p.RoleId);
                    }
                    else if (sorting.Equals("MenuId DESC"))
                    {
                        query = query.OrderByDescending(p => p.RoleId);
                    }
                    else if (sorting.Equals("SaveId ASC"))
                    {
                        query = query.OrderBy(p => p.SaveId);
                    }
                    else if (sorting.Equals("SaveId DESC"))
                    {
                        query = query.OrderByDescending(p => p.SaveId);
                    }
                    else if (sorting.Equals("RoleName ASC"))
                    {
                        query = query.OrderBy(p => p.RoleName);
                    }
                    else if (sorting.Equals("RoleName DESC"))
                    {
                        query = query.OrderByDescending(p => p.RoleName);
                    }
                    else if (sorting.Equals("MenuName ASC"))
                    {
                        query = query.OrderBy(p => p.MenuName);
                    }
                    else if (sorting.Equals("MenuName DESC"))
                    {
                        query = query.OrderByDescending(p => p.MenuName);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.SaveId); //Default!
                    }

                    return count > 0
                               ? query.Skip(startIndex).Take(count).ToList()  //Paging
                               : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetAllRoles()
        {
            try
            {
                var result = new SelectList(_role.GetAllActiveRoles(), "RoleId", "RoleName");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult DeleteRole(int saveId)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var savedRoles = (from savedSubMenuRole in db.SavedSubMenuRoles
                        where savedSubMenuRole.SavedSubMenuRoleId == saveId
                        select savedSubMenuRole).FirstOrDefault();

                    if (savedRoles != null) db.SavedSubMenuRoles.Remove(savedRoles);
                    db.SaveChanges();
                }
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult UpdateRole([Bind(Include = "SaveId,Status")]ViewSubMenuRoleStatusUpdateModel viewMenuRoleStatusUpdateModel)
        {
            try
            {

                if (_role.UpdateSubMenuRoleStatus(viewMenuRoleStatusUpdateModel) > 0)
                {
                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}