using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TestBot.Models.Menu
{
    class SearchMenu
    {

        public static void Find(TelegramBotClient bot)
        {
            bot.OnMessage += BotOnMessageRecived;
            
        }

        private static void BotOnMessageRecived(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            YandexAPI.Maps.GeoCode geoCode = new YandexAPI.Maps.GeoCode();
            string result = geoCode.SearchObject(message.Text); // 76.904529 43.254999
            Console.WriteLine(result);
            //Алматы, ул.Айтиева, 42
        }
    }
}
