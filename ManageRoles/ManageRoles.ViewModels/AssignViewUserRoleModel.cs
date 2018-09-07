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
    public class AssignViewUserRoleModel
    {
        [DisplayName("Role")]
        [Required(ErrorMessage = "Choose Role")]
        [RoleValidate]
        public int? RoleId { get; set; }

        [DisplayName("User")]
        [Required(ErrorMessage = "Choose User")]
        [UserValidate]
        public long? UserId { get; set; }
        public List<RoleMaster> ListRole { get; set; }
        public List<DropdownUsermaster> ListUsers { get; set; }
    }

    public class UserValidate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Convert.ToString(value) == "-1")
            {
                var message = "Choose Username";
                return new ValidationResult(message);
            }
            return ValidationResult.Success;
        }
    }
}
