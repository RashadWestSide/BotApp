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

        public void CheckCmd(string message)
        {
            message = message.ToLower();

            var curCmd = _db.Commands.Where(c => c.Request == message);
            foreach(var ans in curCmd)
            {
                MessageSend.MesSend(ans.Response);
            }
        }
    }
}