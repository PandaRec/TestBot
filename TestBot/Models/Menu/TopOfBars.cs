using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace TestBot.Models.Menu
{
    class TopOfBars
    {
        public static bool Contains(Telegram.Bot.Types.Message message)
        {
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location) return false;
            if (message.Text.Equals("\U0001F51D")) return true;
            else return false;
        }
    }
}
