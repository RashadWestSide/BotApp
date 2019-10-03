using BotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BotApp.Controllers
{
    [Authorize]
    public class CommandsController : Controller
    {
        BotAppContext _db = new BotAppContext();

        
        public ActionResult Index()
        {
            User user = _db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            var commands = _db.Commands.Where(c => c.UserId == user.Id);

            return View(commands.ToList());
        }

        [HttpGet]
        public ActionResult AddCmd()
        {
            User user = _db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                return View();
            }
            return RedirectToAction("LogOut", "Account");
        }

        [HttpPost]
        public ActionResult AddCmd(Command command)
        {
            User user = _db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if(user == null)
            {
                return RedirectToAction("LogOut", "Account");
            }

            if (ModelState.IsValid)
            {
                command.UserId = user.Id;

                _db.Commands.Add(command);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(command);
        }

        public ActionResult GetToken(FormCollection col)
        {
            Params.Token = col["Token"];
            return View();
        }


        public EmptyResult GetStart()
        {
            LongPoll.LongPollServer.reqLongPoll();

            return new EmptyResult();
        }
    }
}