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
    public class AssignRolestoSubMenuController : Controller
    {
        private readonly IMenu _menu;
        private readonly ISubMenu _submenu;
        private readonly IRole _role;
        private readonly ISavedSubMenuRoles _savedRoles;

        public AssignRolestoSubMenuController(IMenu menu, ISubMenu submenu, IRole role, ISavedSubMenuRoles savedRoles)
        {
            _menu = menu;
            _submenu = submenu;
            _role = role;
            _savedRoles = savedRoles;
        }

        // GET: AssignRolestoSubMenu
        [HttpGet]
        public ActionResult Assign()
        {
            try
            {
                var menuList = _menu.GetAllActiveMenu();
                var submenuList = new List<SubMenuMaster>();

                submenuList.Insert(0, new SubMenuMaster()
                {
                    SubMenuId = -1,
                    SubMenuName = "---Select---"
                });
                var rolesList = _role.GetAllActiveRoles();

                AssignRoleViewModelSubMenu assignRoleViewModel = new AssignRoleViewModelSubMenu()
                {
                    Menulist = menuList,
                    SubMenulist = submenuList,
                    RolesList = rolesList
                };

                return View(assignRoleViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Assign(AssignRoleViewModelSubMenu assignRoleViewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var autoUsermaster = AutoMapper.Mapper.Map<SavedSubMenuRoles>(assignRoleViewModel);

                    if (!_savedRoles.CheckRoleAlreadyExists(autoUsermaster))
                    {
                        _savedRoles.SaveRole(autoUsermaster);
                        TempData["MessageAssigned"] = "SubMenu Assigned to Roles Successfully";
                        return RedirectToAction("Assign", "AssignRolestoSubMenu");
                    }
                    else
                    {
                        TempData["AlreadyAssignedMessage"] = "SubMenu to this Role is already assigned";

                        var menuList = _menu.GetAllActiveMenu();
                        var submenuList = new List<SubMenuMaster>();
                        submenuList.Insert(0, new SubMenuMaster()
                        {
                            SubMenuId = -1,
                            SubMenuName = "---Select---"
                        });
                        var rolesList = _role.GetAllActiveRoles();

                        assignRoleViewModel = new AssignRoleViewModelSubMenu()
                        {
                            Menulist = menuList,
                            SubMenulist = submenuList,
                            RolesList = rolesList,
                            MenuId = assignRoleViewModel.MenuId,
                            SubMenuId = assignRoleViewModel.SubMenuId,
                            RoleId = assignRoleViewModel.RoleId
                        };
                    }


                    return View(assignRoleViewModel);
                }
                else
                {
                    var menuList = _menu.GetAllActiveMenu();
                    var submenuList = new List<SubMenuMaster>();
                    submenuList.Insert(0, new SubMenuMaster()
                    {
                        SubMenuId = -1,
                        SubMenuName = "---Select---"
                    });

                    var rolesList = _role.GetAllActiveRoles();

                    assignRoleViewModel = new AssignRoleViewModelSubMenu()
                    {
                        Menulist = menuList,
                        SubMenulist = submenuList,
                        RolesList = rolesList,
                        MenuId = assignRoleViewModel.MenuId,
                        SubMenuId = assignRoleViewModel.SubMenuId,
                        RoleId = assignRoleViewModel.RoleId
                    };

                    return View(assignRoleViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetSubMenuList(string menuid)
        {
            try
            {

                if (string.IsNullOrEmpty(menuid))
                {

                    var submenuList = new List<SubMenuMaster>();
                    submenuList.Insert(0, new SubMenuMaster()
                    {
                        SubMenuId = -1,
                        SubMenuName = "---Select---"
                    });

                    return Json(data: submenuList, behavior: JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var submenuList = _submenu.GetAllActiveSubMenu(Convert.ToInt32(menuid));

                    return Json(data: submenuList, behavior: JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
  
    }
}