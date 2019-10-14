using BotApp.BotAct;
using BotApp.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using xNet;

namespace BotApp.Controllers
{
    [Authorize]
    public class LongPollServerController : Controller
    {
        BotAppContext _db = new BotAppContext();

        [HttpGet]
        public ActionResult AddToken()
        {
            User user = _db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                return View();
            }
            return RedirectToAction("LogOut", "Account");
        }

        [HttpPost]
        public ActionResult AddToken(Token token)
        {
            User user = _db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("LogOut", "Account");
            }

            if (ModelState.IsValid)
            {
                token.UserId = user.Id;

                _db.Tokens.Add(token);
                _db.SaveChanges();

                return RedirectToAction("Index","Commands");
            }
            return View(token);
        }


        public string[] getLongPollServer(User currentUser)
        {           
            Token token = _db.Tokens.Where(c => c.UserId == currentUser.Id).First();
            var _token = token.mtoken;

            var httpRequest = new HttpRequest
            {
                UserAgent = Http.ChromeUserAgent(),
                Cookies = new CookieDictionary(),
                KeepAlive = true
            };

            string response = httpRequest.Get("https://api.vk.com/method/messages.getLongPollServer?"
                + "&" + "need_pts=" + 0
                + "&" + "lp_version=" + 3
                + "&" + "access_token=" + _token
                + "&" + "v=" + Params.vers).ToString();

            JObject json = JObject.Parse(response);

            return new string[]
            {
                    json["response"]["server"].ToString(),
                    json["response"]["key"].ToString(),
                    json["response"]["ts"].ToString()
            };
        }

        public string reqLongPoll(User currentUser)
        {
            LongPollServerController longPoll = new LongPollServerController();

            string[] Data = longPoll.getLongPollServer(currentUser);
            string server = Data[0], key = Data[1];
            int ts = Convert.ToInt32(Data[2]);

            HttpRequest httpRequest = new HttpRequest();

            while (true)
            {
                string resp = httpRequest.Get($"https://{server}?act=a_check&key={key}&ts={ts}&wait=25&mode=2&version=3").ToString();

                JObject json = JObject.Parse(resp);
                if (resp.Contains("failed"))
                {
                    if (Convert.ToInt32(json["failed"]) == 1) { ts = Convert.ToInt32(json["ts"]); }
                    else if (Convert.ToInt32(json["failed"]) == 2) { Data = getLongPollServer(currentUser); key = Data[1]; }
                    else if (Convert.ToInt32(json["failed"]) == 3) { Data = getLongPollServer(currentUser); key = Data[1]; ts = Convert.ToInt32(Data[2]); }
                }
                else
                {
                    for (int i = 0; i < json["updates"].Count(); i++)
                    {
                        if (json["updates"][i].Count() == 8)
                        {
                            //{"ts":1820350874,"updates":[­[4,1619489,561,123456,1464958914,"hello",{"title":" ... "},{"attach1_type":"photo","attach1":"123456_414233177", "attach2_type":"audio","attach2":"123456_456239018"}]]}
                            Params.Title = json["updates"][i][5].ToString();
                            Params.IdMes = Convert.ToInt32(json["updates"][i][1]);
                            Params.User_id = Convert.ToInt32(json["updates"][i][3]);
                            //Console.WriteLine(Params.Title);

                            Cmd commands = new Cmd();
                            commands.CheckCmd(Params.Title, currentUser);
                        }
                    }
                }
                ts = Convert.ToInt32(json["ts"]);
            }
        }

        public void MesSend(string Message, User currentUser, string Media = "")
        {

            Token token = _db.Tokens.Where(c => c.UserId == currentUser.Id).First();
            var _token = token.mtoken;

            var httpRequest = new HttpRequest
            {
                Cookies = new CookieDictionary(),
                KeepAlive = true,
                UserAgent = Http.ChromeUserAgent()
            };

            RequestParams reqParams = new RequestParams();
            reqParams["message"] = Message;
            reqParams["user_id"] = Params.User_id;
            reqParams["attachment"] = Media;
            reqParams["forward_messages"] = Params.IdMes;
            reqParams["access_token"] = _token;
            reqParams["v"] = Params.vers;

            string response = httpRequest.Get("https://api.vk.com/method/messages.send?", reqParams).ToString();
        }





        //public static string[] GetLongPollServer()
        //{
        //    var httpRequest = new HttpRequest
        //    {
        //        UserAgent = Http.ChromeUserAgent(),
        //        Cookies = new CookieDictionary(),
        //        KeepAlive = true
        //    };

        //    string response = httpRequest.Get("https://api.vk.com/method/messages.getLongPollServer?"
        //        + "&" + "need_pts=" + 0
        //        + "&" + "lp_version=" + 3
        //        + "&" + "access_token=" + Params.Token
        //        + "&" + "v=" + Params.vers).ToString();

        //    JObject json = JObject.Parse(response);

        //    return new string[]
        //    {
        //            json["response"]["server"].ToString(),
        //            json["response"]["key"].ToString(),
        //            json["response"]["ts"].ToString()
        //    };
        //}


        //public static string ReqLongPoll()
        //{
        //    string[] Data = GetLongPollServer();
        //    string server = Data[0], key = Data[1];
        //    int ts = Convert.ToInt32(Data[2]);

        //    HttpRequest httpRequest = new HttpRequest();

        //    while (true)
        //    {
        //        string resp = httpRequest.Get($"https://{server}?act=a_check&key={key}&ts={ts}&wait=25&mode=2&version=3").ToString();

        //        JObject json = JObject.Parse(resp);
        //        if (resp.Contains("failed"))
        //        {
        //            if (Convert.ToInt32(json["failed"]) == 1) { ts = Convert.ToInt32(json["ts"]); }
        //            else if (Convert.ToInt32(json["failed"]) == 2) { Data = GetLongPollServer(); key = Data[1]; }
        //            else if (Convert.ToInt32(json["failed"]) == 3) { Data = GetLongPollServer(); key = Data[1]; ts = Convert.ToInt32(Data[2]); }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < json["updates"].Count(); i++)
        //            {
        //                if (json["updates"][i].Count() == 8)
        //                {
        //                    //{"ts":1820350874,"updates":[­[4,1619489,561,123456,1464958914,"hello",{"title":" ... "},{"attach1_type":"photo","attach1":"123456_414233177", "attach2_type":"audio","attach2":"123456_456239018"}]]}
        //                    Params.Title = json["updates"][i][5].ToString();
        //                    Params.IdMes = Convert.ToInt32(json["updates"][i][1]);
        //                    Params.User_id = Convert.ToInt32(json["updates"][i][3]);
        //                    //Console.WriteLine(Params.Title);

        //                    Cmd commands = new Cmd();
        //                    commands.CheckCmd(Params.Title);
        //                }
        //            }
        //        }
        //        ts = Convert.ToInt32(json["ts"]);
        //    }
        //}
    }
}