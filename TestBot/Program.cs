using System;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InlineQueryResults;
using TestBot.Models.Menu;
using System.Collections.Generic;
using Yandex.Geocoder;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json;

namespace TestBot
{
    class Program
    {
        public static TelegramBotClient bot { get; private set; }
        private static Telegram.Bot.Types.Message answ = null;
        public static List<Telegram.Bot.Types.Message> MessagesFromBot { get; private set; } = new List<Telegram.Bot.Types.Message>();
        public static List<Telegram.Bot.Types.Message> MessagesFromUser { get; private set; } = new List<Telegram.Bot.Types.Message>();
        private static Telegram.Bot.Types.Location UserLocation;



        static void Main(string[] args)
        {
           

            WebProxy proxyObject = new WebProxy("176.53.40.222:3128/", true);


            bot = new TelegramBotClient("1257487397:AAHPbBjcQD1dzw8FYV7BXN3CD-1alTrR-kI",proxyObject);
            bot.OnMessage += BotOnMessageRecived;
           // bot.OnCallbackQuery += BotOnCallbackQueryRecived;

            
            
           


            Console.WriteLine("lol");
            var me = bot.GetMeAsync().Result;
            Console.WriteLine(me.FirstName);
            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
 

    }

        
        private static void BotOnCallbackQueryRecived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            string text = e.CallbackQuery.Data;
            Console.WriteLine(text);
        }

        private static async void BotOnMessageRecived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            
            var message = e.Message;
            Console.WriteLine(message.Text);
            switch (message.Text)
            {
                case "/start":
                    answ=await bot.SendTextMessageAsync(message.Chat.Id, StartMenu.Greeting, replyMarkup: StartMenu.ReplyKeyboard);
                    MessagesFromBot.Add(answ);
                    break;
                case "/help":
                    answ=await bot.SendTextMessageAsync(message.Chat.Id, InfoMenu.Info);
                    MessagesFromBot.Add(answ);
                    break;
                case "Назад":
                    //в главное меню
                    await bot.SendTextMessageAsync(message.Chat.Id, StartMenu.menu, replyMarkup: StartMenu.ReplyKeyboard);
                    break;
                case "\U0001F43E":
                    //near bars
                    await bot.SendTextMessageAsync(message.Chat.Id, NearBars.AskLocation, replyMarkup: NearBars.ReplyKeyboardGeo);
                    NearBars.Execute();
                    Console.WriteLine("рядом");
                    break;

                case "\U0001F51D":
                    //top of bars
                    Console.WriteLine("топ");
                    break;

                case "\U00002049":
                    //info about application and other secondary things
                    await bot.SendTextMessageAsync(message.Chat.Id, InfoMenu.Info, replyMarkup: InfoMenu.ReplyKeyboard);
                    InfoMenu.Execute();
                    //Program.MessagesFromBot.Add(answ);
                    Console.WriteLine("инфо");
                    break;

                case "\U0001F50D":
                    //search
                    await bot.SendTextMessageAsync(message.Chat.Id, SearchMenu.Menu, replyMarkup: SearchMenu.ReplyKeyboard);
                    SearchMenu.Execute();
                    Console.WriteLine("поиск");
                    break;

                case "\U0001F52E":
                    //search by conditions
                    Console.WriteLine("поиск по критериям");
                    break;

                case "\U00002699":
                    //sittings
                    await bot.SendTextMessageAsync(message.From.Id, SittingsMenu.Greeting, replyMarkup: SittingsMenu.ReplyKeyboard);
                    SittingsMenu.Execute();
                    //MessagesFromBot.Add(answ);
                    Console.WriteLine("настройки");
                    break;
                default:
                    //StartMenu.Execute();
                    //await bot.SendTextMessageAsync(message.From.Id,"Это что то незнакомое...");
                    break;


            }




        }

       
    }
}
