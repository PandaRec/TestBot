using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TestBot.Models.Menu
{
    class InfoMenu
    {
       // public static Dictionary<long, bool> Like = new Dictionary<long, bool>();
        //public static Dictionary<long, bool> Dislike = new Dictionary<long, bool>();

        //public static int LikeValue { get; private set; } 
        //public static int DislikeValue { get; private set; }

        //public static bool Like { get; private set; } = false;
        //public static bool Dislike { get; private set; } = false;
        

        private static TelegramBotClient Bot=Program.bot;
        public static string Info { get; } = "Лови язык приложения:" +
            "\n" + "\U0001F43E" + "   -   Бары рядом" +
            "\n" + "\U0001F51D" + "   -   Топ баров по мнению пользователей" +
            "\n" + "\U00002049" + "   -   Тут можно написать нам, оценить и тд" +
            "\n" + "\U0001F50D" + "   -   Это поиск по названию бара" +
            "\n" + "\U0001F52E" + "   -   Бары у которых есть меню" +
            "\n" + "\U00002699" + "   -   А это у нас настройки";

        public static ReplyKeyboardMarkup ReplyKeyboard { get; } = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("Контакты"),
                            new KeyboardButton("Оцени нас")
                         },
                        new[]
                        {
                            new KeyboardButton("Сотрудничество"),
                            new KeyboardButton("Назад")
                        }

        }, true);

        private static InlineKeyboardMarkup KeyboardContacts = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithUrl("GitHub проекта",@"https://github.com/PandaRec/TestBot")
            }
        });
      
        /*
        public static InlineKeyboardMarkup SetKeyboard()
        {
            InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("\U00002764"+LikeValue,"like"),
                InlineKeyboardButton.WithCallbackData("\U0001F494"+DislikeValue,"dislike")

            }
        });
            
            return Temp;
   
        }
        */

        public static bool Contains(Telegram.Bot.Types.Message message)
        {
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location) return false;
            if (message.Text.Equals("Оцени нас") || message.Text.Equals("\U00002049")
               || message.Text.Equals("Контакты") || message.Text.Equals("Сотрудничество") ) return true;
            else return false;
        }
        public static async void MessageRecived(object sender, MessageEventArgs e)
        {

            var message = e.Message;
            /*
            if (!Like.ContainsKey(message.Chat.Id))
            {
                Like.Add(message.Chat.Id, false);
                Dislike.Add(message.Chat.Id, false);
            }
            */
                switch (message.Text)
            {
                case "Контакты":
                    await Bot.SendTextMessageAsync(message.Chat.Id,"Можешь глянуть если интересно",replyMarkup:KeyboardContacts);
                    break;
                case "Сотрудничество":
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Реклама, покупка и тд - сюда:\nPandarec13@yandex.ru");
                    break;
                    /*
                case "Оцени нас":
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Оцени нас", replyMarkup: SetKeyboard());
                    break;
                    */
                default:
                    break;
            }

        }
        /*
        private static async void BotOnMessageRecivedd(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            switch (message.Text)
            {
                case "Оцени нас":
                     await Bot.SendTextMessageAsync(message.Chat.Id, "Оцени нас",replyMarkup:SetKeyboard());

                    break;
                default:
                    break;
            }
        }
        */

        public static async void CallbackQueryRecived(object sender, CallbackQueryEventArgs e)
        {
            var message = e.CallbackQuery.Data;
            
            switch (message)
            {
                /*
                case "like":
                    //like
                    Console.WriteLine(LikeValue);
                    if (Like[e.CallbackQuery.Message.Chat.Id] == false)
                    {
                        if (Dislike[e.CallbackQuery.Message.Chat.Id] == true) DislikeValue -= 1;
                        LikeValue += 1; ;
                        await Bot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, "Спасибо за оценку",replyMarkup:SetKeyboard());
                        Like[e.CallbackQuery.Message.Chat.Id] = true;
                        Dislike[e.CallbackQuery.Message.Chat.Id] = false;
                    }
                    else
                    {
                        await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Вы уже проголосовали", false);
                    }
                    break;
                case "dislike":
                    //dislike
                    if (Dislike[e.CallbackQuery.Message.Chat.Id] == false)
                    {
                        if (Like[e.CallbackQuery.Message.Chat.Id] == true) LikeValue -= 1;
                        DislikeValue += 1;
                        await Bot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, "Спасибо за оценку", replyMarkup: SetKeyboard());
                        Dislike[e.CallbackQuery.Message.Chat.Id] = true;
                        Like[e.CallbackQuery.Message.Chat.Id] = false;

                    }
                    else
                    {
                        await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Вы уже проголосовали", false);
                    }

                    break;
                    */

                default:
                    break;
            }
        }
    }
}
