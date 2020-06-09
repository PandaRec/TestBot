﻿//ing System;
//uing System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using System.Collections.Generic;
using System;
using Telegram.Bot.Args;

namespace TestBot.Models.Menu
{
    class SittingsMenu
    {

        private static TelegramBotClient Bot=Program.bot;

        public static string Greeting { get; } = "Ты в настройках";

        public static ReplyKeyboardMarkup ReplyKeyboard { get; } = new ReplyKeyboardMarkup(new[] {

                new []{
                new KeyboardButton("Очистить истрию с ботом")
                },
                new []{
                new KeyboardButton("Назад")
                }

        }, true);

        public static void Execute()
        {
            Bot.OnMessage += BotOnMessageRecived;

        }

        private static async void BotOnMessageRecived(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            switch (message.Text)
            {
                case "Очистить истрию с ботом":
                    Clear(Program.MessagesFromUser,Program.MessagesFromBot);
                    break;
               /* case "Назад":
                    await Bot.SendTextMessageAsync(message.Chat.Id, StartMenu.menu, replyMarkup: StartMenu.ReplyKeyboard);
                    break;
                    */
                default:
                    break;

            }
        }

        private static async void Clear(List<Telegram.Bot.Types.Message> MsgFromUser,List<Telegram.Bot.Types.Message> MsgFromBot)
        {
            foreach (var item in MsgFromBot)
            {
                try { await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId); }
                catch{ }
            }
            Program.MessagesFromBot.Clear();

            foreach (var item in MsgFromUser)
            {
                try { await Bot.DeleteMessageAsync(item.Chat.Id, item.MessageId); }
                catch { }

            }
            Program.MessagesFromUser.Clear();

        }
        



}
}
