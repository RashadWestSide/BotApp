using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net;
using xNet;
using BotApp.BotAct;
using BotApp.Models;
using System.Web.Mvc;

namespace BotApp.LongPoll
{
    [Authorize]
    public class LongPollServer : Controller
    {
        BotAppContext _db = new BotAppContext();

        public string[] getLongPollServer()
        {
            User user = _db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            Token token = _db.Tokens.Where(c => c.UserId == user.Id).First();
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

        public string reqLongPoll()
        {
            LongPollServer longPoll = new LongPollServer();

            string[] Data = longPoll.getLongPollServer();
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
                    else if (Convert.ToInt32(json["failed"]) == 2) { Data = getLongPollServer(); key = Data[1]; }
                    else if (Convert.ToInt32(json["failed"]) == 3) { Data = getLongPollServer(); key = Data[1]; ts = Convert.ToInt32(Data[2]); }
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
                            //commands.CheckCmd(Params.Title);
                        }
                    }
                }
                ts = Convert.ToInt32(json["ts"]);
            }
        }
    }
}