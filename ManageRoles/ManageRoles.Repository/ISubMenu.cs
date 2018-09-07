using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageRoles.Models;
using ManageRoles.ViewModels;

namespace ManageRoles.Repository
{
    public interface ISubMenu : IDisposable
    {
        IEnumerable<SubMenuMaster> GetAllSubMenu();
        SubMenuMaster GetSubMenuById(int? subMenuId);
        int? AddSubMenu(SubMenuMaster subMenuMaster);
        int? UpdateSubMenu(SubMenuMaster subMenuMaster);
        void DeleteSubMenu(int? subMenuId);
        bool CheckSubMenuNameExists(string subMenuName, int menuId);
        IQueryable<SubMenuMasterViewModel> ShowAllSubMenus(string sortColumn, string sortColumnDir, string search);
        List<SubMenuMaster> GetAllActiveSubMenu(int menuid);
        List<SubMenuMaster> GetAllActiveSubMenuByMenuId(int menuid);
    }
}
