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
        public static int LikeValue { get; private set; } 
        public static int DislikeValue { get; private set; } 
        

        private static TelegramBotClient Bot=Program.bot;
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
                            new KeyboardButton("Контакты"),
                            new KeyboardButton("Оцени нас")
                         },
                        new[]
                        {
                            new KeyboardButton("Сотрудничество"),
                            new KeyboardButton("Назад")
                        }

        }, true);

        public static InlineKeyboardMarkup RateUs { get; private set; } = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("\U00002764"+LikeValue,"like")
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("\U0001F494"+DislikeValue,"dislike")
            }
        });

        private static InlineKeyboardMarkup SetKeyboard()
        {
            InlineKeyboardMarkup Temp  = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("\U00002764"+LikeValue,"like")
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("\U0001F494"+DislikeValue,"dislike")
            }
        });
            return Temp;
   
        }



        public static void Execute()
        {
            Bot.OnCallbackQuery += BotOnCallbackQueryRecived;
            Bot.OnMessage += BotOnMessageRecived;

        }

        private static async void BotOnMessageRecived(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            switch (message.Text)
            {
                case "Оцени нас":
                    await Bot.SendTextMessageAsync(message.Chat.Id,"Оцени нас!\nНам просто интересно на скок это прикольно для тебя",replyMarkup: RateUs);
                    break;
                default:
                    break;
            }
        }

        private static async void BotOnCallbackQueryRecived(object sender, CallbackQueryEventArgs e)
        {
            var message = e.CallbackQuery.Data;
            
            switch (message)
            {
                case "like":
                    //like
                    //Console.WriteLine(LikeValue);
                    LikeValue = 1;
                    Console.WriteLine(LikeValue);
                    //bot.edit_message_text(chat_id = call.message.chat.id, message_id = call.message.message_id, text = "Пыщь")
                    //bot.answer_callback_query(callback_query_id = call.id, show_alert = False, text = "Пыщь!")
                    //await Bot.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id,e.CallbackQuery.Message.MessageId,"Спасибо за оценку",replyMarkup:RateUs);
                    await Bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,"lol",replyMarkup:SetKeyboard());
                    //Console.WriteLine(LikeValue);
                    break;
                case "dislike":
                    //dislike
                    await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,"lol",false);
                    break;
                default:
                    break;
            }
        }
    }
}
