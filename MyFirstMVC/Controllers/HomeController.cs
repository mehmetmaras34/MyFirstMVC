using MyFirstMVC.Data;
using MyFirstMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();

                    mailMessage.From = new System.Net.Mail.MailAddress("mehmetmaras34@gmail.com", "Mehmet MARAŞ");
                    mailMessage.Subject = "İletişim Formu: " + model.FirstName + " " + model.LastName;

                    mailMessage.To.Add("mehmetmaras34@gmail.com");

                    string body;
                    body = "Ad: " + model.FirstName + "<br />";
                    body = "Soyad: " + model.LastName + "<br />";
                    body += "Telefon: " + model.Phone + "<br />";
                    body += "E-posta: " + model.Email + "<br />";
                    body += "Mesaj: " + model.Message + "<br />";
                    body += "Tarih: " + DateTime.Now.ToString("dd MMMM yyyy HH:mm") + "<br />";
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = body;

                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new System.Net.NetworkCredential("mehmetmaras34@gmail.com", "Mail şifresi(buraya yazılacak)");
                    smtp.EnableSsl = true;
                    smtp.Send(mailMessage);
                    ViewBag.Message = "Mesajınız gönderildi. Teşekkür ederiz.";
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Form gönderimi başarısız oldu. Lütfen daha sonra tekrar deneyiniz.";
                    
                }
                
            }
            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Aşağıdaki Formu doldurarak bize ulaşabilirsiniz..";

            return View();
        }
        public ActionResult Project()
        {
            using (var db=new ApplicationDbContext())
            {

                var projects = db.Projects.ToList();
                return View(projects);
            }

            
        }
        
        public ActionResult Kvkk()
        {
            ViewBag.Message = "Kvkk Sayfası";

            return View();
        }

    }
}