using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Filters;
using ManageRoles.Repository;

namespace ManageRoles.Controllers
{
    [AuthorizeUser]
    public class UserDashboardController : Controller
    {
        private readonly IMenu _iMenu;

        public UserDashboardController(IMenu menu, ISubMenu subMenu)
        {
            _iMenu = menu;
        }

        // GET: UserDashboard
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