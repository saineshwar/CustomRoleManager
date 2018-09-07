using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageRoles.Models;

namespace ManageRoles.ViewModels
{
    public class AssignRoleViewModel
    {

        [Required(ErrorMessage = "Choose Menu")]
        [MenuValidate]
        [DisplayName("Menu")]
        public int MenuId { get; set; }
        public  List<MenuMaster> Menulist { get; set; }

        [Required(ErrorMessage = "Choose Role")]
        [RoleValidate]
        [DisplayName("Role")]
        public int RoleId { get; set; }
        public List<RoleMaster> RolesList { get; set; }  
   
    }

    public class AssignRoleViewModelSubMenu
    {
        [DisplayName("Menu")]
        [Required(ErrorMessage = "Choose Menu")]
        [MenuValidate]
        public int MenuId { get; set; }
        public List<MenuMaster> Menulist { get; set; }

        [DisplayName("SubMenu")]
        [Required(ErrorMessage = "Choose SubMenu")]
        [SubMenuValidate]
        public int SubMenuId { get; set; }
        public List<SubMenuMaster> SubMenulist { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Choose Role")]
        [RoleValidate]
        public int RoleId { get; set; }
        public List<RoleMaster> RolesList { get; set; }

    }

    public class SubMenuValidate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Convert.ToString(value) == "-1")
            {
                var message = "Choose SubMenu";
                return new ValidationResult(message);
            }
            return ValidationResult.Success;
        }
    }

    public class RoleValidate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Convert.ToString(value) == "-1")
            {
                var message = "Choose Role";
                return new ValidationResult(message);
            }
            return ValidationResult.Success;
        }
    }

}
