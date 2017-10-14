using PitWorks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PitWorks.Controllers
{
    public class AccountController : Controller
    {
        PitDBEntities db = new PitDBEntities();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Пользователь.FirstOrDefault(p => p.Логин == model.Login && p.Пароль == model.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    bool a = FormsAuthentication.IsEnabled;
                 
                    if (user.id_уровня_доступа == 0)
                        return RedirectToAction("HomePagePDS", "Home");
                    else if (user.id_уровня_доступа == 1)
                        return RedirectToAction("HomePageLPU", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем не существует");
                }
            }
            return View(model);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}