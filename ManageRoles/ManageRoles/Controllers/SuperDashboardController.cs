using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Filters;
using ManageRoles.Repository;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    public class SuperDashboardController : Controller
    {
        private readonly IMenu _iMenu;
        private readonly ISubMenu _ISubMenu;

        public SuperDashboardController(IMenu menu, ISubMenu subMenu)
        {
            _iMenu = menu;
            _ISubMenu = subMenu;
        }

        // GET: SuperDashboard
        public ActionResult Dashboard()
        {
            return View();
        }


        public ActionResult ShowMenus()
        {
            try
            {
                var menuList = _iMenu.GetAllActiveMenuSuperAdmin();
                return PartialView("ShowMenu", menuList);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}