﻿using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace TestBot.Models.Menu
{
    class SearchMenu
    {
        private static TelegramBotClient Bot;

        public static ReplyKeyboardMarkup ReplyKeyboard { get; } = new ReplyKeyboardMarkup(new[] { new KeyboardButton("Назад") }, true);

        public static string Menu { get; } = "Введите название бара";
        public static void Execute(TelegramBotClient bot)
        {
            Bot = bot;
            Bot.OnMessage += BotOnMessageRecived;

        }

        private static async void BotOnMessageRecived(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            //тут запрос к бд на поиск результата

            switch (message.Text)
            {
                case "Назад":
                    await Bot.SendTextMessageAsync(message.Chat.Id, StartMenu.menu, replyMarkup: StartMenu.ReplyKeyboard);
                    break;
                default:
                    Console.WriteLine("Запрос к бд на поиск по названию");
                    await Bot.SendTextMessageAsync(message.Chat.Id, "тут бы появился бар по названию. Или же бары");
                    break;
            }

        }
    }
}
