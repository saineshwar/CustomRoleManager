using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Models;
using ManageRoles.ViewModels;
using ManageRoles.Filters;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    public class AllUsersController : Controller
    {
        // GET: AllUsers
        public ActionResult Show()
        {
            return View();
        }

        [HttpPost]//Gets the todo Lists.  
        public JsonResult UserList(string username, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {

            try
            {
                var userCount = GetUserCount();

                var roles = GetUserList(username, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = roles, TotalRecordCount = userCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetUserCount()
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

        public List<UsermasterViewModel> GetUserList(string username, int startIndex, int count, string sorting)
        {
            // Instance of DatabaseContext
            try
            {
                using (var db = new DatabaseContext())
                {
                    var data = from usermaster in db.Usermasters
                               where usermaster.Status == true
                               select new UsermasterViewModel()
                               {
                                   UserId = usermaster.UserId,
                                   UserName = usermaster.UserName,
                                   FirstName = usermaster.FirstName,
                                   LastName = usermaster.LastName,
                                   EmailId = usermaster.EmailId,
                                   Gender = usermaster.Gender,
                                   Status = usermaster.Status,
                                   MobileNo = usermaster.MobileNo
                               };

                    IEnumerable<UsermasterViewModel> query = data.ToList();

                    //Search
                    if (username != null)
                    {
                        query = query.Where(p => p.UserName.Contains(username));
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("UserId ASC"))
                    {
                        query = query.OrderBy(p => p.UserId);
                    }
                    else if (sorting.Equals("UserId DESC"))
                    {
                        query = query.OrderByDescending(p => p.UserId);
                    }
                    else if (sorting.Equals("UserName ASC"))
                    {
                        query = query.OrderBy(p => p.UserName);
                    }
                    else if (sorting.Equals("UserName DESC"))
                    {
                        query = query.OrderByDescending(p => p.UserName);
                    }

                    else if (sorting.Equals("FirstName ASC"))
                    {
                        query = query.OrderBy(p => p.FirstName);
                    }
                    else if (sorting.Equals("FirstName DESC"))
                    {
                        query = query.OrderByDescending(p => p.FirstName);
                    }

                    else if (sorting.Equals("LastName ASC"))
                    {
                        query = query.OrderBy(p => p.LastName);
                    }
                    else if (sorting.Equals("LastName DESC"))
                    {
                        query = query.OrderByDescending(p => p.LastName);
                    }

                    else if (sorting.Equals("EmailId ASC"))
                    {
                        query = query.OrderBy(p => p.EmailId);
                    }

                    else if (sorting.Equals("EmailId DESC"))
                    {
                        query = query.OrderByDescending(p => p.EmailId);
                    }


                    else if (sorting.Equals("Gender ASC"))
                    {
                        query = query.OrderBy(p => p.Gender);
                    }

                    else if (sorting.Equals("Gender DESC"))
                    {
                        query = query.OrderByDescending(p => p.Gender);
                    }

                    else if (sorting.Equals("Status ASC"))
                    {
                        query = query.OrderBy(p => p.Status);
                    }

                    else if (sorting.Equals("Status DESC"))
                    {
                        query = query.OrderByDescending(p => p.Status);
                    }

                    else if (sorting.Equals("MobileNo ASC"))
                    {
                        query = query.OrderBy(p => p.MobileNo);
                    }

                    else if (sorting.Equals("MobileNo DESC"))
                    {
                        query = query.OrderByDescending(p => p.MobileNo);
                    }

                    else
                    {
                        query = query.OrderBy(p => p.UserId); //Default!
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


        public JsonResult RemoveUser(int userId)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    var roleId = (from sar in db.SavedAssignedRoles
                        where sar.UserId == userId
                        select sar.RoleId).FirstOrDefault();   

                    if (roleId != null)
                    {
                        var role = db.RoleMasters.Find(roleId);

                        if (role != null && role.RoleId == Convert.ToInt32(ConfigurationManager.AppSettings["SuperAdminRolekey"]))
                        {
                            return Json(new { Result = "ERROR", Message = "Cannot Delete Super Admin" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var usermaster = db.Usermasters.Find(userId);
                            if (usermaster != null) db.Usermasters.Remove(usermaster);
                            db.SaveChanges();

                            var password = db.PasswordMaster.Find(userId);
                            if (password != null) db.PasswordMaster.Remove(password);
                            db.SaveChanges();

                            var savedAssignedRoles = db.SavedAssignedRoles.Find(userId);
                            if (savedAssignedRoles != null) db.SavedAssignedRoles.Remove(savedAssignedRoles);
                            db.SaveChanges();
                        }
                    }
                }
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}