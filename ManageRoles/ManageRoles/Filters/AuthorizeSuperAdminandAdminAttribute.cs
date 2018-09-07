using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ManageRoles.Models;
using ManageRoles.Repository;

namespace ManageRoles.Filters
{
    public class AuthorizeSuperAdminandAdminAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                DatabaseContext context = new DatabaseContext();

                var role = Convert.ToString(filterContext.HttpContext.Session["Role"]);

                if (!string.IsNullOrEmpty(role))
                {
                    var roleValue = Convert.ToInt32(role);

                    var roleMasterDetails = (from rolemaster in context.RoleMasters
                                             where rolemaster.RoleId == roleValue
                                             select rolemaster).FirstOrDefault();

                    if (roleMasterDetails != null && !(roleMasterDetails.RoleName.ToLower() == "admin" ||
                                                       roleMasterDetails.RoleName.ToLower() == "superadmin"))
                    {
                        filterContext.HttpContext.Session.Abandon();

                        filterContext.Result = new RedirectToRouteResult
                        (
                            new RouteValueDictionary
                                (new
                                { controller = "Error", action = "Error" }
                            ));
                    }
                }
                else
                {
                    filterContext.HttpContext.Session.Abandon();

                    filterContext.Result = new RedirectToRouteResult
                    (
                        new RouteValueDictionary
                        (new
                        { controller = "Error", action = "Error" }
                        ));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}