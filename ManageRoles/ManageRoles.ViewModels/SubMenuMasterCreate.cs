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
    public class SubMenuMasterCreate
    {
        public int SubMenuId { get; set; }

        [Required(ErrorMessage = "Enter ControllerName")]
        public string ControllerName { get; set; }

        [Required(ErrorMessage = "Enter ActionMethod")]
        public string ActionMethod { get; set; }

        [Required(ErrorMessage = "Enter SubMenuName")]
        public string SubMenuName { get; set; }

        [Required(ErrorMessage = "Choose Status")]
        public bool Status { get; set; }
        public DateTime? CreateDate { get; set; }

        [DisplayName("Menu")]
        [Required(ErrorMessage = "Choose Menu")]
        [MenuValidate]
        public int MenuId { get; set; }
        public List<MenuMaster> MenuList { get; set; }
    }


    public class MenuValidate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Convert.ToString(value) == "-1")
            {
                var message = "Choose Menu";
                return new ValidationResult(message);
            }
            return ValidationResult.Success;
        }
    }


}
