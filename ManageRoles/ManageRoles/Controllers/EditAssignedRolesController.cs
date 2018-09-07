using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Models;
using ManageRoles.ViewModels;
using ManageRoles.Filters;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    public class EditAssignedRolesController : Controller
    {

        // GET: EditAssignedRoles
        public ActionResult Edit()
        {
            return View();
        }


        [HttpPost]//Gets the todo Lists.  
        public JsonResult AssignedRolesUserList(string username, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {

            try
            {
                var rolesCount = GetSavedAssignedRolesCount();

                var roles = GetAssignedRolesUserList(username, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = roles, TotalRecordCount = rolesCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetSavedAssignedRolesCount()
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    return db.SavedAssignedRoles.Count();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EditAssignedRolesViewModel> GetAssignedRolesUserList(string username, int startIndex, int count, string sorting)
        {
            // Instance of DatabaseContext
            try
            {
                using (var db = new DatabaseContext())
                {
                    var data = from usermaster in db.Usermasters
                               join sar in db.SavedAssignedRoles on usermaster.UserId equals sar.UserId
                               join roleMaster in db.RoleMasters on sar.RoleId equals roleMaster.RoleId
                               where usermaster.Status == true
                               select new EditAssignedRolesViewModel()
                               {
                                   AssignedRoleId = sar.AssignedRoleId,
                                   RoleName = roleMaster.RoleName,
                                   UserName = usermaster.UserName,
                                   Status = usermaster.Status
                               };


                    IEnumerable<EditAssignedRolesViewModel> query = data.ToList();

                    //Search
                    if (username != null)
                    {
                        query = query.Where(p => p.UserName.Contains(username));
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("AssignedRoleId ASC"))
                    {
                        query = query.OrderBy(p => p.AssignedRoleId);
                    }
                    else if (sorting.Equals("AssignedRoleId DESC"))
                    {
                        query = query.OrderByDescending(p => p.AssignedRoleId);
                    }
                    else if (sorting.Equals("RoleName ASC"))
                    {
                        query = query.OrderBy(p => p.RoleName);
                    }
                    else if (sorting.Equals("RoleName DESC"))
                    {
                        query = query.OrderByDescending(p => p.RoleName);
                    }
                    else if (sorting.Equals("UserName ASC"))
                    {
                        query = query.OrderBy(p => p.UserName);
                    }
                    else if (sorting.Equals("UserName DESC"))
                    {
                        query = query.OrderByDescending(p => p.UserName);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.AssignedRoleId); //Default!
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

        public JsonResult RemoveAssignedRole(int assignedRoleId)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var savedAssignedRoles = db.SavedAssignedRoles.Find(assignedRoleId);
                    if (savedAssignedRoles != null) db.SavedAssignedRoles.Remove(savedAssignedRoles);
                    db.SaveChanges();
                }
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}