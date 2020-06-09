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
        private static TelegramBotClient bot;
        private static Telegram.Bot.Types.Message answ = null;
        private static List<Telegram.Bot.Types.Message> MessagesFromBot = new List<Telegram.Bot.Types.Message>();
        private static List<Telegram.Bot.Types.Message> MessagesFromUser = new List<Telegram.Bot.Types.Message>();
        private static Telegram.Bot.Types.Location UserLocation;

        static void Main(string[] args)
        {
           

            WebProxy proxyObject = new WebProxy("38.91.100.171:3128/", true);


            bot = new TelegramBotClient("1257487397:AAHPbBjcQD1dzw8FYV7BXN3CD-1alTrR-kI",proxyObject);
            bot.OnMessage += BotOnMessageRecived;
            bot.OnCallbackQuery += BotOnCallbackQueryRecived;

            
            
           


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
            Console.WriteLine(message.Type);
            /*
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location)
            {
                if (message.Location == null)
                {
                    await bot.SendTextMessageAsync(message.Chat.Id,"Может ты забыл включить геолокацию?");
                    return;
                }
                UserLocation = message.Location;
                BotRecivedLocation(message);
                await bot.SendLocationAsync(message.Chat.Id, message.Location.Latitude, message.Location.Longitude);
                await bot.SendVenueAsync(message.Chat.Id,message.Location.Latitude,message.Location.Longitude,"твоя локация","адресс");
            }
            */
            //Console.WriteLine(answ.Location);
            //Console.WriteLine(message.Text);
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
                case "\U0001F43E":
                    //near bars
                    NearBars.Execute(bot);
                    answ=await bot.SendTextMessageAsync(message.Chat.Id,NearBars.AskLocation,replyMarkup: NearBars.ReplyKeyboardGeo);
                    Console.WriteLine("рядом");
                   
                    
                    //if (message.Location != null) { Console.WriteLine("location"); }
                    //Console.WriteLine(answ.Location.Latitude);
                    break;

                case "\U0001F51D":
                    //top of bars
                    Console.WriteLine("топ");
                    break;

                case "\U00002049":
                    //info about application and other secondary things
                    //await bot.SendTextMessageAsync(message.Chat.Id, InfoMenu.Info);
                    answ=await bot.SendTextMessageAsync(message.Chat.Id,InfoMenu.Info,replyMarkup: InfoMenu.ReplyKeyboard);
                    MessagesFromBot.Add(answ);
                    Console.WriteLine("инфо");
                    break;

                case "\U0001F50D":
                    //search
                    await bot.SendTextMessageAsync(message.Chat.Id, SearchMenu.Menu, replyMarkup: SearchMenu.ReplyKeyboard);
                    SearchMenu.Execute(bot);
                    Console.WriteLine("поиск");
                    break;

                case "\U0001F52E":
                    //search by conditions
                    Console.WriteLine("поиск по критериям");
                    break;

                case "\U00002699":
                    //sittings
                    answ=await bot.SendTextMessageAsync(message.From.Id, "Вы вошли в настройки",replyMarkup: SittingsMenu.SetKeyboard());
                    MessagesFromBot.Add(answ);
                    Console.WriteLine("настройки");
                    break;
                        case "Геолокация в виде Гугл карт":
                                SittingsMenu.Choice = true;
                                answ=await bot.SendTextMessageAsync(message.From.Id, "Теперь данные будут скидываться тебе в виде Яндекс карт", replyMarkup: SittingsMenu.SetKeyboard());
                                await bot.DeleteMessageAsync(message.From.Id, answ.MessageId-1);
                            break;
                        case "Геолокация в виде Яндекс карт":
                                SittingsMenu.Choice = false;
                                answ=await bot.SendTextMessageAsync(message.From.Id, "Теперь данные будут скидываться тебе в виде Гугл карт", replyMarkup: SittingsMenu.SetKeyboard());
                                await bot.DeleteMessageAsync(message.From.Id, answ.MessageId - 1);

                    break;
                        //case "Назад":
                          //      answ=await bot.SendTextMessageAsync(message.Chat.Id, StartMenu.menu, replyMarkup: StartMenu.ReplyKeyboard);
                    //MessagesFromBot.Add(answ);

                    //break;
                case "Очистить истрию с ботом":
                    Console.WriteLine("очистить истрию");
                    
                    SittingsMenu.Clear(message.Chat.Id, answ, bot);

                    break;

               

                default:
                    //await bot.SendTextMessageAsync(message.From.Id,"Это что то незнакомое...");

                    break;


            }




        }

        private static void BotRecivedLocation(Telegram.Bot.Types.Message msg)
        {
            Console.WriteLine(msg.Location.Latitude+" "+msg.Location.Longitude);
            
        }
    }
}
