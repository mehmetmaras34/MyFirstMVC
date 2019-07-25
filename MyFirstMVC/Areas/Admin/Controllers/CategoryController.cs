using MyFirstMVC.Data;
using MyFirstMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstMVC.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var categories = db.Categories.ToList();
                return View(categories);

            }
        }
        public ActionResult Create()
        {
            var category = new Category();
            return View(category);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(category);
        }
        public ActionResult Edit(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var category = db.Categories.Where(x => x.Id == id).FirstOrDefault();
                if (category != null)
                {
                    return View(category);
                }
                else
                {
                    HttpNotFound();
                }
                return View(category);
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    var oldcategory = db.Categories.Where(x => x.Id == category.Id).FirstOrDefault();
                    if (oldcategory != null)
                    {
                        oldcategory.Name = category.Name;
                        oldcategory.Description = category.Description;

                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
            }
            return View(category);
        }
        public ActionResult Delete(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                Category category = db.Categories.Find(id);
                var projects = db.Projects.Where(x => x.CategoryId == id).ToList();
                foreach (var item in projects)
                {
                    item.CategoryId = null;
                }

                db.Categories.Remove(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }
    }

}
