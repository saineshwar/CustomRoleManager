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
    public class AllRolesController : Controller
    {
        private readonly IRole _role;
        // GET: AllRoles

        public AllRolesController(IRole role)
        {
            _role = role;
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
    }
}