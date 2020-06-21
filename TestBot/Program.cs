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
using Telegram.Bot.Args;
using Npgsql;
using System.Data.Common;

namespace TestBot
{
    class Program
    {
        public static TelegramBotClient bot { get; private set; }
        private static Telegram.Bot.Types.Message answ = null;
        public static List<Telegram.Bot.Types.Message> MessagesFromBot { get; private set; } = new List<Telegram.Bot.Types.Message>();
        public static List<Telegram.Bot.Types.Message> MessagesFromUser { get; private set; } = new List<Telegram.Bot.Types.Message>();
        //private static Telegram.Bot.Types.Location UserLocation;
        public static List<Models.ModelOfBar> BarInfo { get; private set; } = new List<Models.ModelOfBar>();
        public static List<Models.ModelOfMenuItems> MenuItems { get; private set; } = new List<Models.ModelOfMenuItems>();
        public static List<Models.ModelOfUserRate> UserRate { get; private set; } = new List<Models.ModelOfUserRate>();
        
        static void Main(string[] args)
        {

            WebProxy proxyObject = new WebProxy("163.172.28.22:80/", true);
            bot = new TelegramBotClient("1257487397:AAHPbBjcQD1dzw8FYV7BXN3CD-1alTrR-kI");

            bot.OnMessage += BotOnMessageRecived;
            bot.OnCallbackQuery += BotOnCallbackQueryRecived;

            
            
           


            Console.WriteLine("lol");
            var me = bot.GetMeAsync().Result;
            Console.WriteLine(me.FirstName);

            ReadBD();

            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
 

    }

        private static void BotOnCallbackQueryRecived(object sender, CallbackQueryEventArgs e)
        {
            //InfoMenu.CallbackQueryRecived(sender, e);

            if (NearBars.ContainsData(e.CallbackQuery))
            {
                Console.WriteLine("ttt");
                NearBars.CallBackQRecived(sender,e);
            }
            else if (NearBars.CallBackData.Contains(e.CallbackQuery.Data))
            {
                NearBars.GetMenuItems(e.CallbackQuery.Data, e.CallbackQuery.Message.Chat.Id);
            }
        }

        private static async void BotOnMessageRecived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            
            var message = e.Message;
            Console.WriteLine(message.Text);
            //if (message.Text.Equals("/start")) { }
            //if (message.Text.Equals("/help")) { }
            //if (message.Text.Equals("/Назад")) { }
            //NearBars.Execute();
            //TopOfBars
            
            //InfoMenu.MessageRecived(sender,e);
            //SearchMenu.Execute();
            //SearchByConditions
            //SittingsMenu.Execute();
            if (InfoMenu.Contains(message))
            {
                if (message.Text.Equals("\U00002049"))
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, InfoMenu.Info, replyMarkup: InfoMenu.ReplyKeyboard);
                    InfoMenu.MessageRecived(sender, e);
                }
                else
                {
                    InfoMenu.MessageRecived(sender, e);
                }
                
            }
            else if (NearBars.Contains(message))
            {
                if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location)
                {
                    NearBars.MessageRecived(sender, e);

                }
                else if (message.Text.Equals("\U0001F43E"))
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, NearBars.AskLocation, replyMarkup: NearBars.ReplyKeyboardGeo);
                    NearBars.MessageRecived(sender, e);
                }
                else
                {
                    NearBars.MessageRecived(sender, e);
                }
            }
            else if (SearchMenu.Contains(message))
            {
                if (message.Text.Equals("\U0001F50D"))
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, SearchMenu.Menu, replyMarkup: SearchMenu.ReplyKeyboard);
                    SearchMenu.MessageRecived(sender, e);
                }
                
            }
            else if (message.Text.Equals("/start"))
            {
                answ = await bot.SendTextMessageAsync(message.Chat.Id, StartMenu.Greeting, replyMarkup: StartMenu.ReplyKeyboard);
            }
            else if (message.Text.Equals("/help"))
            {
                answ = await bot.SendTextMessageAsync(message.Chat.Id, InfoMenu.Info);
            }
            else if (message.Text.Equals("Назад"))
            {
                await bot.SendTextMessageAsync(message.Chat.Id, StartMenu.menu, replyMarkup: StartMenu.ReplyKeyboard);

            }

            else if (SittingsMenu.Contains(message))
            {
                if (message.Text.Equals("\U00002699"))
                {
                    await bot.SendTextMessageAsync(message.From.Id, SittingsMenu.Greeting, replyMarkup: SittingsMenu.ReplyKeyboard);
                    SittingsMenu.MessageRecived(sender, e);
                }
                else
                {
                    SittingsMenu.MessageRecived(sender, e);

                }
            }
            else if (TopOfBars.Contains(message))
            {
                if (message.Text.Equals("\U0001F51D"))
                {
                    await bot.SendTextMessageAsync(message.Chat.Id,"Тут будет топ баров");
                }
                else
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, "Тут будет топ баров");

                }
            }
            else if (SearchByConditions.Contains(message))
            {
                if (message.Text.Equals("\U0001F52E"))
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, "Тут будет клава с критериями");
                }
                else
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, "Тут будет клава с критериями");
                }
            }

            else
            {
                SearchMenu.MessageRecived(sender, e);

            }


            switch (message.Text)
            {
                /*
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
                    */
                    /*
                case "\U0001F43E":
                    //near bars
                    await bot.SendTextMessageAsync(message.Chat.Id, NearBars.AskLocation, replyMarkup: NearBars.ReplyKeyboardGeo);
                    NearBars.Execute();
                    Console.WriteLine("рядом");
                    break;
                    */
                    /*
                case "\U0001F51D":
                    //top of bars
                    Console.WriteLine("топ");
                    break;
                    */
/*
                case "\U00002049":
                    //info about application and other secondary things
                     //inf= new InfoMenu();
                    await bot.SendTextMessageAsync(message.Chat.Id, InfoMenu.Info, replyMarkup: InfoMenu.ReplyKeyboard);
                    InfoMenu.Execute(sender,e);
                    //Program.MessagesFromBot.Add(answ);
                    Console.WriteLine("инфо");
                    break;
                    */
                    /*
                case "\U0001F50D":
                    //search
                    await bot.SendTextMessageAsync(message.Chat.Id, SearchMenu.Menu, replyMarkup: SearchMenu.ReplyKeyboard);
                    SearchMenu.Execute();
                    Console.WriteLine("поиск");
                    break;
                    */
                    /*
                case "\U0001F52E":
                    //search by conditions
                    Console.WriteLine("поиск по критериям");
                    break;
                    */
                    /*
                case "\U00002699":
                    //sittings
                    await bot.SendTextMessageAsync(message.From.Id, SittingsMenu.Greeting, replyMarkup: SittingsMenu.ReplyKeyboard);
                    SittingsMenu.Execute();
                    //MessagesFromBot.Add(answ);
                    Console.WriteLine("настройки");
                    break;
                    */
                default:
                    //StartMenu.Execute();
                    //await bot.SendTextMessageAsync(message.From.Id,"Это что то незнакомое...");
                    break;


            }




        }
        /// <summary>
        /// Чтение данных из бд
        /// </summary>
        private static void ReadBD()
        {
            //выборка данных из бд
            Models.ModelOfBar bar = new Models.ModelOfBar();
            Models.ModelOfMenuItems items = new Models.ModelOfMenuItems();
            Models.ModelOfUserRate rate = new Models.ModelOfUserRate();

            string connectionString = "Server=localhost;Port=5432;User ID=postgres;Password=3400430;Database=Menu;";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            Console.WriteLine("Соединение с БД открыто");
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand("SELECT * FROM barinfo", npgSqlConnection);
            NpgsqlDataReader npgSqlDataReader = npgSqlCommand.ExecuteReader();
            if (npgSqlDataReader.HasRows)
            {
                Console.WriteLine("Таблица: BarInfo");
                Console.WriteLine("");
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                {
                    bar = new Models.ModelOfBar();
                    //Console.WriteLine(dbDataRecord["BarName"] + "   " + dbDataRecord["Latitude"] + "   " + dbDataRecord["Longitude"] + "   " + dbDataRecord["Phone"] + "   " + dbDataRecord["WorkTime"] + "   " + dbDataRecord["Pictures"]);
                    bar.BarName = dbDataRecord["BarName"].ToString();
                    bar.Lat = Convert.ToDouble(dbDataRecord["Latitude"]);
                    bar.Lng = Convert.ToDouble(dbDataRecord["Longitude"]);
                    bar.Phone= dbDataRecord["Phone"].ToString();
                    bar.WorkTime= dbDataRecord["WorkTime"].ToString();
                    bar.HasMenu = Convert.ToBoolean(dbDataRecord["HasMenu"]);
                    string[] temp = dbDataRecord["Pictures"].ToString().Split("|");
                    foreach (var item in temp)
                        if(item!="")
                        bar.PictureLinks.Add(item);
                    BarInfo.Add(bar);
                }
            }
            else
                Console.WriteLine("Запрос не вернул строк в BarInfo");
            npgSqlDataReader.Close();



            npgSqlCommand = new NpgsqlCommand("SELECT * FROM menuitems", npgSqlConnection);
            npgSqlDataReader = npgSqlCommand.ExecuteReader();
            if (npgSqlDataReader.HasRows)
            {
                Console.WriteLine("Таблица: MenuItems");
                Console.WriteLine("");
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                {
                    items = new Models.ModelOfMenuItems();
                    // Console.WriteLine(dbDataRecord["Title"] + "   " + dbDataRecord["Subtitle"] + "   " + dbDataRecord["Subtitle_2"] + "   " + dbDataRecord["Dish"] + "   " + dbDataRecord["Price"] + "   " + dbDataRecord["BarName"]);
                    items.Title = dbDataRecord["Title"].ToString();
                    items.Subtitle = dbDataRecord["Subtitle"].ToString();
                    items.Subtitle_2 = dbDataRecord["Subtitle_2"].ToString();
                    items.Dish = dbDataRecord["Dish"].ToString();
                    items.Price = Convert.ToInt32(dbDataRecord["Price"]);
                    items.BarName = dbDataRecord["BarName"].ToString();
                    MenuItems.Add(items);
                }
            }
            else
                Console.WriteLine("Запрос не вернул строк в MenuItems");
            npgSqlDataReader.Close();


            npgSqlCommand = new NpgsqlCommand("SELECT * FROM UserRate", npgSqlConnection);
            npgSqlDataReader = npgSqlCommand.ExecuteReader();
            if (npgSqlDataReader.HasRows)
            {
                Console.WriteLine("Таблица: UserRate");
                Console.WriteLine("");
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                {
                    rate = new Models.ModelOfUserRate();
                    // Console.WriteLine(dbDataRecord["Title"] + "   " + dbDataRecord["Subtitle"] + "   " + dbDataRecord["Subtitle_2"] + "   " + dbDataRecord["Dish"] + "   " + dbDataRecord["Price"] + "   " + dbDataRecord["BarName"]);
                    rate.BarName = dbDataRecord["BarName"].ToString();

                    string[] temp = dbDataRecord["Likes"].ToString().Split(",");
                    foreach (var item in temp)
                    {
                        if (item != "") 
                            rate.Likes.Add(Convert.ToInt64(item));
                    }

                    temp = dbDataRecord["Dislikes"].ToString().Split(",");
                    foreach (var item in temp)
                    {
                        if (item != "")
                            rate.Dislikes.Add(Convert.ToInt64(item));
                    }

                    
                    UserRate.Add(rate);
                }
            }
            else
                Console.WriteLine("Запрос не вернул строк в MenuItems");


            Console.WriteLine("чтение завершено");
            npgSqlDataReader.Close();
            npgSqlConnection.Close();
                
        }


    }
}
