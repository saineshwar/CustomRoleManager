using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Models;
using ManageRoles.Repository;
using ManageRoles.ViewModels;
using AutoMapper;
using ManageRoles.Filters;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    public class SubMenuMasterController : Controller
    {
        private readonly ISubMenu _subMenu;
        private readonly IMenu _menu;
        public SubMenuMasterController(ISubMenu subMenu, IMenu menu)
        {
            _subMenu = subMenu;
            _menu = menu;
        }

        // GET: SubMenuMaster
        public ActionResult Index()
        {
            return View(_subMenu.GetAllSubMenu());
        }

        // GET: SubMenuMaster/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var subMenu = _subMenu.GetSubMenuById(id);
                var subMenuMaster = AutoMapper.Mapper.Map<SubMenuMasterViewModel>(subMenu);
                var menuMaster = _menu.GetMenuById(subMenuMaster.MenuId);
                subMenuMaster.MenuName = menuMaster.MenuName;
                return View(subMenuMaster);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: SubMenuMaster/Create
        public ActionResult Create()
        {
            try
            {
                var menuList = _menu.GetAllMenu();
                menuList.Insert(0, new MenuMaster()
                {
                    MenuId = -1,
                    MenuName = "---Select---"
                });

                SubMenuMasterCreate subMenu = new SubMenuMasterCreate()
                {
                    MenuList = menuList
                };

                return View(subMenu);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: SubMenuMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubMenuId,ControllerName,ActionMethod,SubMenuName,Status,CreateDate,MenuId")] SubMenuMasterCreate subMenuMasterVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var subMenuMaster = AutoMapper.Mapper.Map<SubMenuMaster>(subMenuMasterVm);
                    subMenuMaster.UserId = Convert.ToInt32(Session["UserID"]);
                    _subMenu.AddSubMenu(subMenuMaster);
                    return RedirectToAction("Index");
                }

                var menuList = _menu.GetAllMenu();
                menuList.Insert(0, new MenuMaster()
                {
                    MenuId = -1,
                    MenuName = "---Select---"
                });

                SubMenuMasterCreate subMenu = new SubMenuMasterCreate()
                {
                    MenuList = menuList
                };

                return View(subMenu);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: SubMenuMaster/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var subMenuMaster = _subMenu.GetSubMenuById(id);

                var menuList = _menu.GetAllMenu();
                menuList.Insert(0, new MenuMaster()
                {
                    MenuId = -1,
                    MenuName = "---Select---"
                });

                SubMenuMasterCreate subMenu = new SubMenuMasterCreate()
                {
                    MenuList = menuList,
                    SubMenuName = subMenuMaster.SubMenuName,
                    MenuId = subMenuMaster.MenuId,
                    SubMenuId = subMenuMaster.SubMenuId,
                    ActionMethod = subMenuMaster.ActionMethod,
                    ControllerName = subMenuMaster.ControllerName,
                    Status = subMenuMaster.Status
                };

                return View(subMenu);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: SubMenuMaster/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubMenuId,ControllerName,ActionMethod,SubMenuName,Status,CreateDate,MenuId")] SubMenuMasterCreate subMenuMasterCreate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var subMenuMaster = AutoMapper.Mapper.Map<SubMenuMaster>(subMenuMasterCreate);
                    subMenuMaster.UserId = Convert.ToInt32(Session["UserID"]);

                    _subMenu.UpdateSubMenu(subMenuMaster);
                    return RedirectToAction("Index");
                }
                return View(subMenuMasterCreate);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: SubMenuMaster/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                SubMenuMaster subMenuMaster = _subMenu.GetSubMenuById(id);
                if (subMenuMaster == null)
                {
                    return HttpNotFound();
                }
                return View(subMenuMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: SubMenuMaster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _subMenu.DeleteSubMenu(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult CheckSubMenuName(string menuName, int menuId)
        {
            try
            {
                var result = _subMenu.CheckSubMenuNameExists(menuName, menuId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult LoadallSubMenus()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                var rolesData = _subMenu.ShowAllSubMenus(sortColumn, sortColumnDir, searchValue);
                recordsTotal = rolesData.Count();
                var data = rolesData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
