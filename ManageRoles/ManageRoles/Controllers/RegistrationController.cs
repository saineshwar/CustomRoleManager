using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Algorithm;
using ManageRoles.Models;
using ManageRoles.Repository;
using ManageRoles.ViewModels;

namespace ManageRoles.Controllers
{
    [AllowAnonymous]
    public class RegistrationController : Controller
    {
        private readonly IUserMaster _iUserMaster;
        private readonly IPassword _iPassword;
        public RegistrationController(IUserMaster userMaster, IPassword password)
        {
            _iUserMaster = userMaster;
            _iPassword = password;

        }
        // GET: Registration

        // GET: Registration/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Registration/Register
        [HttpPost]
        public ActionResult Register(UsermasterView usermaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var autoUsermaster = AutoMapper.Mapper.Map<Usermaster>(usermaster);
                    var isUser = _iUserMaster.CheckUsernameExists(usermaster.UserName);
                    if (isUser)
                    {
                        ModelState.AddModelError("", "Username already exists");
                    }

                    AesAlgorithm aesAlgorithm = new AesAlgorithm();

                    var userId = _iUserMaster.AddUser(autoUsermaster);
                    if (userId != -1)
                    {
                        PasswordMaster passwordMaster = new PasswordMaster
                        {
                            CreateDate = DateTime.Now,
                            UserId = userId,
                            PasswordId = 0,
                            Password = aesAlgorithm.EncryptString(usermaster.Password)
                        };

                        var passwordId = _iPassword.SavePassword(passwordMaster);

                        if (passwordId != -1)
                        {
                            TempData["MessageRegistration"] = "Registration Successful";
                        }

                        
                    }

                    return RedirectToAction("Register", "Registration");
                }
                else
                {
                    return View("Register", usermaster);
                }
            }
            catch
            {
                throw;
            }


        }

        public JsonResult CheckUsername(string username)
        {
            try
            {
                var result = _iUserMaster.CheckUsernameExists(username);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
