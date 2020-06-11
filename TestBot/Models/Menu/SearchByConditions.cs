using System;
using System.Collections.Generic;
using System.Text;

namespace TestBot.Models.Menu
{
    class SearchByConditions
    {
        public static bool Contains(Telegram.Bot.Types.Message message)
        {
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location) return false;
            if (message.Text.Equals("\U0001F52E")) return true;
            else return false;
        }
    }
}
