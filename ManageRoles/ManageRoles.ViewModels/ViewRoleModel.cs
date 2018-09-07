using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.ViewModels
{
    public class ViewRoleModel
    {
        public int SaveId { get; set; }
        public int RoleId { get; set; }
        public string MenuName { get; set; }
        public string RoleName { get; set; }
        public string SubMenuName { get; set; }     
     }

    public class ViewMenuRoleModel
    {
        public int SaveId { get; set; }
        public int RoleId { get; set; }
        public string MenuName { get; set; }
        public string RoleName { get; set; }
        public bool Status { get; set; }
    }

    public class ViewMenuRoleStatusUpdateModel
    {
        public int SaveId { get; set; }
        public bool Status { get; set; }
    }

    public class ViewSubMenuRoleModel
    {
        public int SaveId { get; set; }
        public int RoleId { get; set; }
        public string MenuName { get; set; }
        public string RoleName { get; set; }
        public string SubMenuName { get; set; }
        public bool Status { get; set; }
    }

    public class ViewSubMenuRoleStatusUpdateModel
    {
        public int SaveId { get; set; }
        public bool Status { get; set; }
    }

}
