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
    public class AllAssignedRoleMenuController : Controller
    {
        private readonly IRole _role;
        // GET: AllMenu

        public AllAssignedRoleMenuController(IRole role)
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

        public List<ViewMenuRoleModel> GetRolesList(int? roleId, int startIndex, int count, string sorting)
        {
            // Instance of DatabaseContext
            try
            {
                using (var db = new DatabaseContext())
                {
                    var data = from savedroles in db.SavedMenuRoles
                               join roleMaster in db.RoleMasters on savedroles.RoleId equals roleMaster.RoleId
                               join menuMaster in db.MenuMaster on savedroles.MenuId equals menuMaster.MenuId
                               select new ViewMenuRoleModel()
                               {
                                   SaveId = savedroles.SavedMenuRoleId,
                                   RoleName = roleMaster.RoleName,
                                   MenuName = menuMaster.MenuName,
                                   RoleId = savedroles.RoleId,
                                   Status = savedroles.Status
                               };


                    IEnumerable<ViewMenuRoleModel> query = data.ToList();

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

        public JsonResult DeleteRole(int SaveId)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var savedRoles = db.SavedMenuRoles.Find(SaveId);
                    if (savedRoles != null) db.SavedMenuRoles.Remove(savedRoles);
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
        public JsonResult UpdateRole([Bind(Include = "SaveId,Status")]ViewMenuRoleStatusUpdateModel viewMenuRoleStatusUpdateModel)
        {
            try
            {

                if (_role.UpdateRoleStatus(viewMenuRoleStatusUpdateModel) > 0)
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