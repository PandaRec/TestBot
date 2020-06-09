using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace TestBot.Models.Menu
{
    class StartMenu
    {
        private static TelegramBotClient Bot = Program.bot;
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

        

        public static string Greeting { get; } = @"Здарова!
Рады видеть тебя тут
Если что то будет не понятно как работает, то тыкай на этот значок:"+ "\n\U00002049"+
"\n ну или напиши /help";

        public static string menu { get; } = "Вы в главном меню";


        public static void Execute()
        {
            Bot.OnMessage += BotOnMessageRecived;

        }

        private static async void BotOnMessageRecived(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            switch (message.Text)
            {
                case "\U0001F43E":
                    //near bars
                    NearBars.Execute();
                    await Bot.SendTextMessageAsync(message.Chat.Id, NearBars.AskLocation, replyMarkup: NearBars.ReplyKeyboardGeo);
                    Console.WriteLine("рядом");
                    break;

                case "\U0001F51D":
                    //top of bars
                    Console.WriteLine("топ");
                    break;

                case "\U00002049":
                    //info about application and other secondary things
                    await Bot.SendTextMessageAsync(message.Chat.Id, InfoMenu.Info, replyMarkup: InfoMenu.ReplyKeyboard);
                    //Program.MessagesFromBot.Add(answ);
                    Console.WriteLine("инфо");
                    break;

                case "\U0001F50D":
                    //search
                    SearchMenu.Execute();
                    await Bot.SendTextMessageAsync(message.Chat.Id, SearchMenu.Menu, replyMarkup: SearchMenu.ReplyKeyboard);
                    Console.WriteLine("поиск");
                    break;

                case "\U0001F52E":
                    //search by conditions
                    Console.WriteLine("поиск по критериям");
                    break;

                case "\U00002699":
                    //sittings
                    SittingsMenu.Execute();
                    await Bot.SendTextMessageAsync(message.From.Id, SittingsMenu.Greeting, replyMarkup: SittingsMenu.ReplyKeyboard);
                    //MessagesFromBot.Add(answ);
                    Console.WriteLine("настройки");
                    break;

                default:
                    break;

            }
        }
    }
}
