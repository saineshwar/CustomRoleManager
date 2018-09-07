using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManageRoles.Filters;
using ManageRoles.Models;
using ManageRoles.Repository;

namespace ManageRoles.Controllers
{
    [AuthorizeSuperAdminandAdmin]
    public class MenuMastersController : Controller
    {
        private readonly IMenu _iMenu;
        public MenuMastersController(IMenu menu)
        {
            _iMenu = menu;
        }

        // GET: MenuMasters
        public ActionResult Index()
        {
            return View(_iMenu.GetAllMenu());
        }

        // GET: MenuMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuMaster menuMaster = _iMenu.GetMenuById(id);
            if (menuMaster == null)
            {
                return HttpNotFound();
            }
            return View(menuMaster);
        }

        // GET: MenuMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MenuId,ControllerName,ActionMethod,MenuName,Status,CreateDate,IsCache")] MenuMaster menuMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    menuMaster.UserId = Convert.ToInt32(Session["UserID"]);
                    _iMenu.AddMenu(menuMaster);

                    return RedirectToAction("Index");
                }

                return View(menuMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: MenuMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                MenuMaster menuMaster = _iMenu.GetMenuById(id);
                if (menuMaster == null)
                {
                    return HttpNotFound();
                }
                return View(menuMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: MenuMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MenuId,ControllerName,ActionMethod,MenuName,Status,CreateDate,IsCache")] MenuMaster menuMaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    menuMaster.UserId = Convert.ToInt32(Session["UserID"]);
                    _iMenu.UpdateMenu(menuMaster);
                    return RedirectToAction("Index");
                }
                return View(menuMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: MenuMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                MenuMaster menuMaster = _iMenu.GetMenuById(id);
                if (menuMaster == null)
                {
                    return HttpNotFound();
                }
                return View(menuMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: MenuMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _iMenu.DeleteMenu(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult CheckMenuName(string menuName)
        {
            try
            {
                if (!string.IsNullOrEmpty(menuName))
                {
                    var result = _iMenu.CheckMenuNameExists(menuName);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult LoadallMenus()
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

                var rolesData = _iMenu.ShowAllMenus(sortColumn, sortColumnDir, searchValue);
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
