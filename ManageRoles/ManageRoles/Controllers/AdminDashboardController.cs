using System;
using ManageRoles.Repository;
using System.Web.Mvc;
using ManageRoles.Filters;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    public class AdminDashboardController : Controller
    {
        private readonly IMenu _iMenu;

        public AdminDashboardController(IMenu menu, ISubMenu subMenu)
        {
            _iMenu = menu;
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
                var menuList = _iMenu.GetAllActiveMenu(Convert.ToInt32(Session["Role"]));
                return PartialView("ShowMenu", menuList);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}