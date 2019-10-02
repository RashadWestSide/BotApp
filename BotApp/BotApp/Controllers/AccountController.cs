using BotApp.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace BotApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = null;
                using (BotAppContext db = new BotAppContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);
                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Commands");
                }
                else
                {
                    ModelState.AddModelError("", "Поьзователя с таким логином и паролем нет в базе");
                }
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (BotAppContext db = new BotAppContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Login == model.Login);
                }
                if (user == null)
                {
                    // создаем нового пользователя
                    using (BotAppContext db = new BotAppContext())
                    {
                        db.Users.Add(new User { Login = model.Login, Password = model.Password, Name = model.UserName, RoleId = 2 });
                        db.SaveChanges();

                        user = db.Users.Where(u => u.Login == model.Login && u.Password == model.Password).FirstOrDefault();
                    }
                    // если пользовател успешно добавлен в БД
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, true);
                        return RedirectToAction("Index", "Commands");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }
            return View(model);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}