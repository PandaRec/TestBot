using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using System;
using Telegram.Bot.Args;
using System.Collections.Generic;

namespace TestBot.Models.Menu
{
    class NearBars
    {
        private static TelegramBotClient Bot=Program.bot;
        public static string menu { get; } = "Вы в ближлижайших барах";
        public static string Greeting { get; } = "Выберите на сколько вам не лень идти";

        public static string AskLocation { get; } = "Разрешите нам глянуть где вы сейчас";

        public static string BarName { get; private set; }

        private static float LatitudePos;
        private static float LongitudePos;

        private static List<ModelOfBar> NearBarsList = new List<ModelOfBar>();

        private static Dictionary<long, int> counter = new Dictionary<long, int>();
        

        public static ReplyKeyboardMarkup ReplyKeyboardGeo { get; } = new ReplyKeyboardMarkup(new[] {
            new[]{
            new KeyboardButton("Показать где я сейчас") { RequestLocation=true}
            },
            new[]
            {
                new KeyboardButton("Назад")
            }
            
        },true);
        public static ReplyKeyboardMarkup ReplyKeyboard { get; } = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            new KeyboardButton("100m"),
                            new KeyboardButton("500m"),
                            new KeyboardButton("1km"),
                            new KeyboardButton("2km")
                         },
                        new[]
                        {
                            new KeyboardButton("3km"),
                            new KeyboardButton("5km"),
                            new KeyboardButton("8km"),
                            new KeyboardButton("Назад")
                        }

        }, true);

        public static bool Contains(Telegram.Bot.Types.Message message)
        {
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location) return true;
                if (message.Text.Equals("\U0001F43E") || message.Text.Equals("Показать где я сейчас")
                || message.Text.Equals("100m") || message.Text.Equals("500m") || message.Text.Equals("1km")
                || message.Text.Equals("2km") || message.Text.Equals("3km") || message.Text.Equals("5km")
                || message.Text.Equals("8km")) return true;
            else return false;
        }
        public static InlineKeyboardMarkup SetKeyboard(bool hasmenu)
        {
            if (hasmenu == true)
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Меню","Menu"),
                InlineKeyboardButton.WithCallbackData("Дальше","Next"),
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Фотогаллерея","Photos"),
            }
        });
                return Temp;

            }
            else
            {
                InlineKeyboardMarkup Temp = new InlineKeyboardMarkup(new[] {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Фотогаллерея","Photos"),
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Дальше","Next")
            }
        });

                return Temp;

            }


        }


        public static async void MessageRecived(object sender, MessageEventArgs e)
        {
            var message = e.Message;

            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location)
            {
                Console.WriteLine("ffff");
                if (message.Location == null)
                {
                    await Bot.SendTextMessageAsync(message.Chat.Id,"Может ты забыл включить геолокацию?");
                    return;
                }
                LatitudePos = message.Location.Latitude;
                LongitudePos = message.Location.Longitude;
                await Bot.SendTextMessageAsync(message.Chat.Id,Greeting,replyMarkup:ReplyKeyboard);

            }

            switch (message.Text)
            {
                
                /*case "Назад":
                    //в главное меню
                    await Bot.SendTextMessageAsync(message.Chat.Id,StartMenu.menu,replyMarkup:StartMenu.ReplyKeyboard);
                    break;
                    */
                case "100m":
                    if (FindNears(100).Count == 0)
                        await Bot.SendTextMessageAsync(message.Chat.Id, "На расстоянии 100m ничего не найдено");
                    else
                    {
                        //if()
                    }
                        //await Bot.SendTextMessageAsync(message.Chat.Id,NearBarsList[counter[message.Chat.Id]].BarName,SetKeyboard())
                    //тут вывод того, что удалось найти в FindNears
                    break;
                case "500m":
                    await Bot.SendTextMessageAsync(message.Chat.Id, "500m");
                    //FindNears(500);
                    //тут вывод того, что удалось найти в FindNears
                    break;
                case "1km":
                    await Bot.SendTextMessageAsync(message.Chat.Id, "1km");
                    FindNears(1000);
                    //тут вывод того, что удалось найти в FindNears
                    break;
                case "2km":
                    await Bot.SendTextMessageAsync(message.Chat.Id, "2km");
                    FindNears(2000);
                    //тут вывод того, что удалось найти в FindNears
                    break;
                case "3km":
                    await Bot.SendTextMessageAsync(message.Chat.Id, "3km");
                    FindNears(3000);
                    //тут вывод того, что удалось найти в FindNears
                    break;
                case "5km":
                    await Bot.SendTextMessageAsync(message.Chat.Id, "5km");
                    FindNears(5000);
                    //тут вывод того, что удалось найти в FindNears
                    break;
                case "8km":
                    await Bot.SendTextMessageAsync(message.Chat.Id, "8km");
                    FindNears(8000);
                    //тут вывод того, что удалось найти в FindNears
                    break;
                default:
                    break;

            }

        }

        private static List<ModelOfBar>  FindNears( int range)//float latitude,float longitude,
        {
            List<ModelOfBar> bars = new List<ModelOfBar>();
            foreach (var item in Program.BarInfo)
            {
                Console.WriteLine("lat - "+item.Lat);
                Console.WriteLine("lng - "+item.Lng);
                Console.WriteLine("me lat - "+LatitudePos);
                Console.WriteLine( "me lng - "+LongitudePos);

                double lat1 = LatitudePos * Math.PI / 180;
                double lat2 = item.Lat * Math.PI / 180;
                double lng1 = LongitudePos * Math.PI / 180;
                double lng2 = item.Lng * Math.PI / 180;

                double delta_lat = (lat2 - lat1);
                double delta_lng = (lng2 - lng1);

                double dist =  (6378137 * Math.Acos(Math.Cos( lat1) * Math.Cos( lat2) * Math.Cos( lng1 - lng2) + Math.Sin( lat1) * Math.Sin( lat2)));
                Console.WriteLine(Math.Round(dist));

                if (Math.Round(dist) < range) bars.Add(item);


               

            }
            return bars;

        }

        private static void ShowNearBars(List <ModelOfBar> nearbars, object sender, MessageEventArgs e)
        {
            var message = e.Message;
            
            while (!Contains(e.Message) || !message.Text.Equals("Назад"))
            {

            }
            Console.WriteLine("нажали клавишу то б выйти");
        }





    }
}
