using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TestBot.Models.Menu
{
    class StartMenu
    {
        public static ReplyKeyboardMarkup ReplyKeyboard { get; } = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("\U0001F43E"),
                            new KeyboardButton("\U0001F51D"),
                            new KeyboardButton("\U00002049")

                         },
                        new[]
                        {
                            new KeyboardButton("\U0001F50D"),
                            new KeyboardButton("\U0001F52E"),
                            new KeyboardButton("\U00002699")

                        }

        },true);

        

        public static string Greeting { get; } = @"Зарова!
Рады видеть тебя тут
Если что то будет не понятно как работает, то тыкай на этот значок:"+ "\n\U00002049"+
"\n ну или напиши /help";

        public static string menu { get; } = "Вы в главном меню";









    }
}
