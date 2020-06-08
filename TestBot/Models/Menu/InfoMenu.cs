using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace TestBot.Models.Menu
{
    class InfoMenu
    {
        public static string Info { get; } = "Лови язык приложения:" +
            "\n" + "\U0001F43E" + "   -   Бары рядом" +
            "\n" + "\U0001F51D" + "   -   Топ баров по мнению пользователей" +
            "\n" + "\U00002049" + "   -   Тут можно написать нам, оценить и тд" +
            "\n" + "\U0001F50D" + "   -   Это поиск по названию бара" +
            "\n" + "\U0001F52E" + "   -   Это если не значешь точно что хочешь, но хотелось бы то-то и то-то. Кароче поиск по критериям))" +
            "\n" + "\U00002699" + "   -   А это у нас настройки";

        public static ReplyKeyboardMarkup ReplyKeyboard { get; } = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("Разрабы"),
                            new KeyboardButton("Контакты"),
                            new KeyboardButton("Напиши нам")
                         },
                        new[]
                        {
                            new KeyboardButton("Оцени нас"),
                            new KeyboardButton("Сотрудничество"),
                            new KeyboardButton("Назад"),


                        }

        }, true);


    }
}
