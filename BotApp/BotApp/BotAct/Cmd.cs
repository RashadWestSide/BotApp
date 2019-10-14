using BotApp.Controllers;
using BotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotApp.BotAct
{
    public class Cmd
    {
        BotAppContext _db = new BotAppContext();

        LongPollServerController mS = new LongPollServerController();

        public void CheckCmd(string message, User currentUser)
        {
            message = message.ToLower();

            var curCmd = _db.Commands.Where(c => c.Request == message);
            foreach(var ans in curCmd)
            {
                mS.MesSend(ans.Response, currentUser);
            }
        }
    }
}