using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using ManageRoles.Algorithm;
using ManageRoles.Repository;
using ManageRoles.ViewModels;

namespace ManageRoles.Controllers
{
    public class LoginController : Controller
    {
        private IUserMaster _iUserMaster;
        private IPassword _password;
        private ISavedAssignedRoles _savedAssignedRoles;
        public LoginController(IUserMaster userMaster, IPassword password, ISavedAssignedRoles savedAssignedRoles)
        {
            _iUserMaster = userMaster;
            _password = password;
            _savedAssignedRoles = savedAssignedRoles;
        }

        // GET: Login/Create
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!this.IsCaptchaValid("Captcha is not valid"))
                    {
                        ModelState.AddModelError("", "Error: captcha is not valid.");
                        return View(new LoginViewModel());
                    }

                    if (!_iUserMaster.CheckUsernameExists(loginViewModel.Username))
                    {
                        ModelState.AddModelError("", "Invalid Credentails");
                        return View(loginViewModel);
                    }

                    AesAlgorithm aesAlgorithm = new AesAlgorithm();
                    var usermasterModel = _iUserMaster.GetUserByUsername(loginViewModel.Username);
                    var storedpassword = aesAlgorithm.DecryptString(_password.GetPasswordbyUserId(usermasterModel.UserId));

                    if (storedpassword == loginViewModel.Password)
                    {
                        Session["UserID"] = usermasterModel.UserId;
                        Session["Username"] = usermasterModel.UserName;

                        if (_savedAssignedRoles.GetAssignedRolesbyUserId(usermasterModel.UserId) != null)
                        {
                            // 1 is SuperAdmin
                            if (_savedAssignedRoles.GetAssignedRolesbyUserId(usermasterModel.UserId).RoleId == Convert.ToInt32(ConfigurationManager.AppSettings["SuperAdminRolekey"]))
                            {
                                Session["Role"] = _savedAssignedRoles.GetAssignedRolesbyUserId(usermasterModel.UserId).RoleId;
                                return RedirectToAction("Dashboard", "SuperDashboard");
                            }

                            // 2 is User
                            if (_savedAssignedRoles.GetAssignedRolesbyUserId(usermasterModel.UserId).RoleId  == Convert.ToInt32(ConfigurationManager.AppSettings["UserRolekey"]))
                            {
                                Session["Role"] = _savedAssignedRoles.GetAssignedRolesbyUserId(usermasterModel.UserId).RoleId;
                                return RedirectToAction("Dashboard", "UserDashboard");
                            }

                            // 3 is Admin
                            if (_savedAssignedRoles.GetAssignedRolesbyUserId(usermasterModel.UserId).RoleId == Convert.ToInt32(ConfigurationManager.AppSettings["AdminRolekey"]))
                            {
                                Session["Role"] = _savedAssignedRoles.GetAssignedRolesbyUserId(usermasterModel.UserId).RoleId;
                                return RedirectToAction("Dashboard", "AdminDashboard");
                            }
                         
                        }
                        else
                        {
                            ModelState.AddModelError("", "Access Not Assigned");
                            return View(loginViewModel);
                        }

                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Credentails");
                        return View(loginViewModel);
                    }

                }
                else
                {
                    return View(loginViewModel);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {

            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                Response.Cache.SetNoStore();

                HttpCookie Cookies = new HttpCookie("WebTime");
                Cookies.Value = "";
                Cookies.Expires = DateTime.Now.AddHours(-1);
                Response.Cookies.Add(Cookies);
                HttpContext.Session.Clear();
                Session.Abandon();
                return RedirectToAction("Login", "Login");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        public void remove_Anonymous_Cookies()
        {
            try
            {

                if (Request.Cookies["WebTime"] != null)
                {
                    var option = new HttpCookie("WebTime");
                    option.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(option);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
