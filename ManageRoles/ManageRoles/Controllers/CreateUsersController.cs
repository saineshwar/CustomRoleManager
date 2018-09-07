using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Algorithm;
using ManageRoles.Filters;
using ManageRoles.Models;
using ManageRoles.Repository;
using ManageRoles.ViewModels;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    public class CreateUsersController : Controller
    {
        private readonly IUserMaster _iUserMaster;
        private readonly IPassword _iPassword;
        private readonly ISavedAssignedRoles _savedAssignedRoles;
        private readonly IRole _iRole;
        public CreateUsersController(IUserMaster userMaster, IPassword password, ISavedAssignedRoles savedAssignedRoles, IRole role)
        {
            _iUserMaster = userMaster;
            _iPassword = password;
            _savedAssignedRoles = savedAssignedRoles;
            _iRole = role;
        }

        // GET: CreateUsers
        public ActionResult Create()
        {
            try
            {
                var createUserViewModel = new CreateUserViewModel()
                {
                    ListRole = _iRole.GetAllActiveRoles()
                };
                return View(createUserViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create(CreateUserViewModel createUserViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var isUser = _iUserMaster.CheckUsernameExists(createUserViewModel.UserName);
                    if (isUser)
                    {
                        ModelState.AddModelError("", "Username already exists");
                    }

                    AesAlgorithm aesAlgorithm = new AesAlgorithm();

                    var usermaster = AutoMapper.Mapper.Map<Usermaster>(createUserViewModel);
                    usermaster.Status = true;
                    usermaster.CreateDate = DateTime.Now;
                    usermaster.UserId = 0;
                    usermaster.CreatedBy = Convert.ToInt32(Session["UserID"]);

                    var userId = _iUserMaster.AddUser(usermaster);
                    if (userId != -1)
                    {
                        var passwordMaster = new PasswordMaster
                        {
                            CreateDate = DateTime.Now,
                            UserId = userId,
                            PasswordId = 0,
                            Password = aesAlgorithm.EncryptString(createUserViewModel.Password)
                        };

                        var passwordId = _iPassword.SavePassword(passwordMaster);
                        if (passwordId != -1)
                        {
                            var savedAssignedRoles = new SavedAssignedRoles()
                            {
                                RoleId = createUserViewModel.RoleId,
                                UserId = userId,
                                AssignedRoleId = 0,
                                Status = true,
                                CreateDate = DateTime.Now
                            };
                            _savedAssignedRoles.AddAssignedRoles(savedAssignedRoles);

                            TempData["MessageCreateUsers"] = "User Created Successfully";
                        }
                    }

                    return RedirectToAction("Create", "CreateUsers");
                }
                else
                {
                    return View("Create", createUserViewModel);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}