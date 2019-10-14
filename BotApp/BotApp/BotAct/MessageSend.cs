using xNet;
using BotApp.LongPoll;
using BotApp.Models;
using System.Web.Mvc;
using System.Linq;

namespace BotApp.BotAct
{
    [Authorize]
    public class MessageSend : Controller
    {
        BotAppContext _db = new BotAppContext();

        public void MesSend(string Message, string Media = "")
        {
            User user = _db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            Token token = _db.Tokens.Where(c => c.UserId == user.Id).First();
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
    }
}