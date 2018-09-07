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
    public class AssignRolestoMenuController : Controller
    {
        private readonly IMenu _menu;
        private readonly ISubMenu _submenu;
        private readonly IRole _role;
        private readonly ISavedMenuRoles _savedRoles;


        public AssignRolestoMenuController(IMenu menu, ISubMenu submenu, IRole role, ISavedMenuRoles savedRoles)
        {
            _menu = menu;
            _submenu = submenu;
            _role = role;
            _savedRoles = savedRoles;
        }

        // GET: AssignRolestoMenu
        [HttpGet]
        public ActionResult Assign()
        {
            try
            {
                var menuList = _menu.GetAllActiveMenu();

                var rolesList = _role.GetAllActiveRoles();

                AssignRoleViewModel assignRoleViewModel = new AssignRoleViewModel()
                {
                    Menulist = menuList,
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
        public ActionResult Assign(AssignRoleViewModel assignRoleViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var autoUsermaster = AutoMapper.Mapper.Map<SavedMenuRoles>(assignRoleViewModel);

                    if (!_savedRoles.CheckRoleAlreadyExists(autoUsermaster))
                    {
                        _savedRoles.SaveRole(autoUsermaster);
                        TempData["MessageAssigned_Menu"] = "Menus Assigned to Roles Successfully";

                        return RedirectToAction("Assign", "AssignRolestoMenu");
                    }
                    else
                    {
                        TempData["AlreadyAssignedMessage_Menu"] = "Menu to this Role is already assigned";


                        var menuList = _menu.GetAllActiveMenu();

                        var rolesList = _role.GetAllActiveRoles();

                        assignRoleViewModel = new AssignRoleViewModel()
                        {
                            Menulist = menuList,
                            RolesList = rolesList,
                            MenuId = assignRoleViewModel.MenuId,
                            RoleId = assignRoleViewModel.RoleId
                        };
                    }


                    return View(assignRoleViewModel);
                }
                else
                {
                    var menuList = _menu.GetAllActiveMenu();


                    var rolesList = _role.GetAllActiveRoles();

                    assignRoleViewModel = new AssignRoleViewModel()
                    {
                        Menulist = menuList,
                        RolesList = rolesList,
                        MenuId = assignRoleViewModel.MenuId,
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