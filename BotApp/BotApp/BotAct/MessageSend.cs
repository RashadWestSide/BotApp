using xNet;
using BotApp.LongPoll;
using BotApp.Models;

namespace BotApp.BotAct
{
    public class MessageSend
    {
        public static void MesSend(string Message, string Media = "")
        {
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
            reqParams["access_token"] = Params.Token;
            reqParams["v"] = Params.vers;

            string response = httpRequest.Get("https://api.vk.com/method/messages.send?", reqParams).ToString();
        }
    }
}