using goldStore.Areas.Panel.Models.Repository;
using goldStore.Models.ViewModel;
using System;
using goldStore.Areas.Panel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using goldStore.Areas.Panel.Models.Securety;
using System.Net.Configuration;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using Microsoft.SqlServer.Server;
using System.Web.Security;

namespace goldStore.Controllers
{
    public class UserController : Controller
    {
        CategoryRepository repoCategory = new CategoryRepository(new Areas.Panel.Models.goldstoreEntities());
        BrandRepository repoBrand = new BrandRepository(new Areas.Panel.Models.goldstoreEntities());

        UserRepository repoUser = new UserRepository(new Areas.Panel.Models.goldstoreEntities());
        // GET: User

        public PartialViewResult PartialNewArrivals()
        {

            return PartialView();
        }
        public PartialViewResult PartialBrands()
        {

            return PartialView(repoBrand.GetAll());
        }
        public ActionResult PartialCategory()
        {
            return PartialView(repoCategory.GetAll());

        }

        public ActionResult PartialPrice()
        {
            return PartialView();

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register register)
        {
            bool status = false;
            string message = "";
            if (ModelState.IsValid)
            {
                if (isExistUser(register.email))
                {
                    message = "bu mail var";
                    ViewBag.message = message;
                    return View();
                }

                user user = new user();
                user.email = register.email;

                user.password = Sifrele.Hash(register.password);
                user.rePassword = Sifrele.Hash(register.comfirmPassword);
                user.activationCode = Guid.NewGuid().ToString();
                user.roleId = 2;
                //oluşturulan kullanıcı mail doğrulama başlangıç olsun.
                user.isMailVerified = false;
                user.createdDate = DateTime.Now;
                repoUser.Save(user);
                SendVerificationLinkEmail(user.email, user.activationCode);
                message = "kayıt ok" + user.email + "adrese bakınız";
                status = true;
                ViewBag.message = message;
                ViewBag.status = status;

            }
            return View();
        }

        [NonAction]
        public bool isExistUser(string username)
        {

            var user = repoUser.GetAll().Where(a => a.email == username).FirstOrDefault();
            return user == null ? false : true;
        }
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            SmtpSection network = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            try
            {
                var verifyUrl = "/User/VerifyAccount/" + activationCode;
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
                var fromEmail = new MailAddress(network.Network.UserName, "Goldstore Üyeligi");
                var toEmail = new MailAddress(emailID);

                string subject = "Kuyumcuya Hosgeldiniz!";
                string body = "<br/><br/>goldstore hesabiniz basariyla olusturulmustur. Hesabiniz aktive etmek için asagidaki linke tiklayiniz" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
                var smtp = new SmtpClient
                {
                    Host = network.Network.Host,
                    Port = network.Network.Port,
                    EnableSsl = network.Network.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = network.Network.DefaultCredentials,
                    Credentials = new NetworkCredential(network.Network.UserName, network.Network.Password)
                };
                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult VerifyAccount (string id)
        {
            bool status = false;
            var result = repoUser.GetAll().Where(x => x.activationCode == new Guid(id).ToString()).FirstOrDefault();
            if(result !=null)
            {

                result.isMailVerified = true;
                repoUser.Update(result);
                status = true;
                ViewBag.status = status;
                ViewBag.message = "Hesabınız başarıyla aktifleşti.Giriş yap";
            }
            else
            {
                ViewBag.status = status;
                ViewBag.message = "geçersiz istek";

            }
            return View("Login");
        }


        public ActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Login(Login login, string ReturnUrl)
        {
            string message = "";
            int sayac = 0;
            bool status = false;
            if(ModelState.IsValid)
            {
                user user = repoUser.GetAll().Where(x => x.email == login.email).FirstOrDefault();
                if(user==null)
                {
                    message = "Email kaydı bulunamadı";
                    ViewBag.message = message;
                    ViewBag.status = status;
                    return View();
                }

                bool verify = user.isMailVerified ?? false;
                if(!verify)
                {
                    message = "email doğrulama yapmadınız";
                    ViewBag.message = message;
                    ViewBag.status = status;
                    sayac++;
                    user.loginAttempt = sayac;
                    repoUser.Update(user);
                    
                }
                if(user.isActive==false)
                {

                    sayac++;
                    message = "Hesabınız geçici olarak kapatıldı";
                    ViewBag.status = status;
                    user.loginAttempt = sayac;
                    repoUser.Update(user);

                }
                login.password = Sifrele.Hash(login.password);
                //şifre eşleşiyorsa
                if(string.Compare(login.password,user.password)==0)
                {

                    user.loginTime = DateTime.Now;
                    user.loginAttempt = sayac;
                    repoUser.Update(user);
                    Session["username"] = user.email;
                    int timeOut = login.rememberMe ? 60 : 10;
                    //form hatırla
                    var ticket = new FormsAuthenticationTicket(login.email, login.rememberMe, timeOut);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeOut);
                    cookie.HttpOnly = true;

                    FormsAuthentication.SetAuthCookie("username", login.rememberMe);
                    Response.Cookies.Add(cookie);

                    if(user.roleId==1)
                   

                        return Redirect("~/Panel/Category");
                    //return Url yerel bir url mi
                    if(Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {

                        return RedirectToAction("Index", "Shop");
                    }

                }


                else
                {

                    sayac++;
                    user.loginAttempt = sayac;
                    repoUser.Update(user);
                    message = "Parolayı hatalı girdiniz!!";
                }

            }


            return View();

        }

    }
}