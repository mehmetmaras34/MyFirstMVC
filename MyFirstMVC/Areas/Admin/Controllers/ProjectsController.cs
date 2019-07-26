using MyFirstMVC.Data;
using MyFirstMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstMVC.Areas.Admin.Controllers
{
    public class ProjectsController : Controller
    {
        // GET: Admin/Projects
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var projects = db.Projects.Include("Category").ToList();
                return View(projects);

            }
        }
        public ActionResult Create()
        {
            var project = new Project();
            using (var db = new ApplicationDbContext())
            {
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }
            return View(project);
        }
        [HttpPost]
        [ValidateInput(false)]//Bu actiona html/script etiketleri gönderilebilir.
        public ActionResult Create(Project project, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {


                using (var db = new ApplicationDbContext())
                {
                    //dosyayı upload etmeyi dene.
                    try
                    {
                        //Yüklenen dosyanın adını entity'deki alana ata.
                        project.Photo = UploadFile(upload);

                    }
                    catch (Exception ex)
                    {
                        //uploadd sırasında bir hata oluşursa Viewda görüntülemek üzere hatayı değişkene ekle
                        ViewBag.Error = ex.Message;
                        ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        //hata oluştuğu için projeyi veritabanına eklemek yerine View'ı tekrar göster ve metottan çık.
                        return View(project);
                    }
                    db.Projects.Add(project);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            using (var db = new ApplicationDbContext())
            {
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }
            return View(project);
        }

        public ActionResult Edit(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var project = db.Projects.Where(x => x.Id == id).FirstOrDefault();
                if (project != null)
                {
                    ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    return View(project);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Project project, HttpPostedFileBase upload, string sil)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    try
                    {
                        //Yüklenen dosyanın adını entity'deki alana ata.
                        project.Photo = UploadFile(upload);

                    }
                    catch (Exception ex)
                    {
                        //uploadd sırasında bir hata oluşursa Viewda görüntülemek üzere hatayı değişkene ekle
                        ViewBag.Error = ex.Message;
                        ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        //hata oluştuğu için projeyi veritabanına eklemek yerine View'ı tekrar göster ve metottan çık.
                        return View(project);
                    }
                    var oldproject = db.Projects.Where(x => x.Id == project.Id).FirstOrDefault();
                    if (oldproject!=null)
                    {                        
                        oldproject.Title = project.Title;
                        oldproject.Description = project.Description;
                        oldproject.Body = project.Body;
                        oldproject.CategoryId = project.CategoryId;
                        if (!string.IsNullOrEmpty(project.Photo))
                        {
                            oldproject.Photo = project.Photo;
                        }
                       
                        if (!string.IsNullOrEmpty(sil))
                        {
                            oldproject.Photo = null;
                        }
                        
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }                    
                }
            }
            using (var db = new ApplicationDbContext())
            {
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }

            return RedirectToAction("Index");
        }


        public ActionResult Delete(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var project = db.Projects.Where(x => x.Id == id).FirstOrDefault();
                if (project != null)
                {
                    db.Projects.Remove(project);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        public string UploadFile(HttpPostedFileBase upload)
        {
            //Yüklenmek istenen dosya varmı?
            if (upload != null && upload.ContentLength > 0)
            {
                //dosyanın uzantısını kontrol et.
                var extension = Path.GetExtension(upload.FileName).ToLower();
                if (extension == ".jpg" || extension == "jpeg" || extension == ".gif" || extension == ".png")
                {
                    //Uzantı doğruysa dosyanın yükleneceği uploads dizini var mı kontrolet.
                    if (Directory.Exists(Server.MapPath("~/Uploads")))
                    {
                        //Dosya adındaki geçersiz karakterleri düzelt.
                        string fileName = upload.FileName.ToLower();
                        fileName = fileName.Replace("İ", "i");
                        fileName = fileName.Replace("Ğ", "g");
                        fileName = fileName.Replace("ğ", "g");
                        fileName = fileName.Replace("Ş", "s");
                        fileName = fileName.Replace("ı", "i");
                        fileName = fileName.Replace(")", "");
                        fileName = fileName.Replace(")", "");
                        fileName = fileName.Replace("(", "");
                        fileName = fileName.Replace(" ", "-");
                        fileName = fileName.Replace(",", "");
                        fileName = fileName.Replace("ö", "o");
                        fileName = fileName.Replace("ü", "u");
                        fileName = fileName.Replace("`", "");
                        //aynı isimde dosya olabilir diye dosya adının önüne zaman pulu ekliyoruz.
                        fileName = DateTime.Now.Ticks.ToString() + fileName;
                        //Dosyayı Uploads dizinine yüle
                        upload.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), fileName));
                        //Yüklenen dosyanın adını geri döndür.
                        return fileName;
                    }
                    else
                    {
                        throw new Exception("Upload dizini mevcut değil.");
                    }
                }
                else
                {
                    throw new Exception("Dosya uzantısı .jpg, .gif ya da .png olmalıdır.");
                }
            }
            return null;
        }
    }
}
