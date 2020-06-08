//ing System;
//uing System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using System.Collections.Generic;

namespace TestBot.Models.Menu
{
    class SittingsMenu
    {
        //public static ReplyKeyboardMarkup replyKeyboard { get { return replyKeyboard; } set { value = SetKeyboard(); } }// = new ReplyKeyboardMarkup();

        public static bool Choice { get; set; } = false;

        public static ReplyKeyboardMarkup SetKeyboard()
        {
            ReplyKeyboardMarkup replyKeyboard;
            if (Choice == false) replyKeyboard = new ReplyKeyboardMarkup(new[] {
                new []{
                new KeyboardButton("Геолокация в виде Гугл карт")
                },
                new []{
                new KeyboardButton("Очистить истрию с ботом")
                },
                new []{
                new KeyboardButton("Назад")
                },
                new []{
                new KeyboardButton("Гео"){RequestLocation=true }
                }

            });
            else replyKeyboard = new ReplyKeyboardMarkup(new[] {
                new []{
                new KeyboardButton("Геолокация в виде Яндекс карт")
                },
                new []{
                new KeyboardButton("Очистить истрию с ботом")
                },
                new []{
                new KeyboardButton("Назад")
                }
            });
            replyKeyboard.ResizeKeyboard = true;
            return replyKeyboard;

        }
        
        public static async void Clear(long chat_id,Telegram.Bot.Types.Message msg_id, TelegramBotClient tg_bot_client)
        {
            for (int i = msg_id.MessageId; i >=0; i--)
            {
                try { await tg_bot_client.DeleteMessageAsync(chat_id, i); }
                catch { System.Console.WriteLine($"отсутсвует сообщение с номером {0}",i); }
            }
           
        }
        



}
}
