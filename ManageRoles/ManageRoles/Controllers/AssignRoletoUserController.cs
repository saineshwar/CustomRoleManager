using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Filters;
using ManageRoles.Models;
using ManageRoles.Repository;
using ManageRoles.ViewModels;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    public class AssignRoletoUserController : Controller
    {
        private readonly IRole _role;
        private readonly ISavedAssignedRoles _savedAssignedRoles;
        private readonly IUserMaster _userMaster;
        public AssignRoletoUserController(IRole role, IUserMaster userMaster, ISavedAssignedRoles savedAssignedRoles)
        {
            _role = role;
            _userMaster = userMaster;
            _savedAssignedRoles = savedAssignedRoles;
        }

        // GET: AssignRoletoUser
        public ActionResult Assign()
        {
            try
            {
                AssignViewUserRoleModel assignViewUserRoleModel = new AssignViewUserRoleModel()
                {
                    ListRole = _role.GetAllActiveRoles(),
                    ListUsers = _userMaster.GetAllUsersActiveList()
                };
                return View(assignViewUserRoleModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Assign(AssignViewUserRoleModel assignViewUserRoleModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    assignViewUserRoleModel = new AssignViewUserRoleModel()
                    {
                        ListRole = _role.GetAllActiveRoles(),
                        ListUsers = _userMaster.GetAllUsersActiveList(),
                        RoleId = assignViewUserRoleModel.RoleId,
                        UserId = assignViewUserRoleModel.UserId
                    };
                    return View(assignViewUserRoleModel);
                }

                if (_savedAssignedRoles.CheckAssignedRoles(assignViewUserRoleModel.UserId))
                {
                    assignViewUserRoleModel = new AssignViewUserRoleModel()
                    {
                        ListRole = _role.GetAllActiveRoles(),
                        ListUsers = _userMaster.GetAllUsersActiveList(),
                        RoleId = assignViewUserRoleModel.RoleId,
                        UserId = assignViewUserRoleModel.UserId
                    };

                    TempData["AssignedErrorMessage"] = "Role is Already Assigned to User";
                    return View(assignViewUserRoleModel);
                }
                else
                {
                    SavedAssignedRoles savedAssignedRoles = new SavedAssignedRoles()
                    {
                        RoleId = assignViewUserRoleModel.RoleId,
                        UserId = assignViewUserRoleModel.UserId,
                        Status = true,
                        CreateDate = DateTime.Now,
                        AssignedRoleId = 0
                    };
                    _savedAssignedRoles.AddAssignedRoles(savedAssignedRoles);
                    TempData["AssignedMessage"] = "Role Assigned to User Successfully";
                    return RedirectToAction("Assign", "AssignRoletoUser");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}